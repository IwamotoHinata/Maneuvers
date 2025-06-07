using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Fusion;
using Online;
using UnityEngine.AI;

namespace Unit
{
    public class CharacterSpawner : NetworkBehaviour
    {
        [SerializeField] private List<GameObject> playerPrefabs;
        [SerializeField] private GameObject minionPrefab;
        private GameObject _unitPrefab;
        private CharacterBaseData _profile;
        private CharacterRPCManager _manager;

        [SerializeField]
        private GameObject spawnEffect;
        
        private CharacterData _instantiateCharacterData;
        private OwnerType _instantiateOwnerType;
        private GameObject _characters;

        private ObserveCharacters _observeCharacters;
        private NetworkObject _spawnedUnit;//スポーンさせたユニット
        private bool _unitAlive = true;//ユニットが生きているかどうか

        public GameObject SpawnedUnit { get => _spawnedUnit.gameObject; }
        public bool UnitAlive { get => _unitAlive; }

        private void Start()
        {
            _characters = GameObject.Find("Characters");
            _observeCharacters = FindObjectOfType<ObserveCharacters>();
        }

        public void Init(CharacterData data, OwnerType type)
        {
            _instantiateCharacterData = data;
            _instantiateOwnerType = type;
            if (_instantiateCharacterData.characterID >= 0)
                _unitPrefab = playerPrefabs[_instantiateCharacterData.characterID];
            
            if (GameLauncher.Runner.GameMode == GameMode.Host)
            {
                StartCoroutine(ReSpawn());
            }
        }

        private IEnumerator ReSpawn()
        {
            yield return new WaitForSeconds(3.3f);
            
            var position = transform.position;
            RPC_SpawnEffect();
            yield return new WaitForSeconds(1.7f);

            _unitAlive = true;
            _spawnedUnit = GameLauncher.Runner.Spawn(
                _unitPrefab, 
                position, 
                Quaternion.identity, 
                Object.InputAuthority,
                ((runner, obj) =>
                {
                    var agent = obj.GetComponent<NavMeshAgent>();
                    agent.enabled = false;
                    _profile = obj.GetComponent<CharacterBaseData>();
                    _manager = obj.GetComponent<CharacterRPCManager>();
                    _manager.InitBaseData(_instantiateCharacterData);
                    _profile.SetCharacterOwnerType(_instantiateOwnerType);
                    _profile.local = Object.InputAuthority;
                    GameLauncher.Instance._spawnedCharacters.Add(obj.Id, obj);
                    var status = obj.GetComponent<CharacterStatus>();
                    status
                        .OnCharacterStateChanged
                        .Where(characterState => characterState == CharacterState.Dead)
                        .Subscribe(_ =>
                        {
                            _unitAlive = false;
                            Debug.Log("キャラクター死亡：" + obj.name);
                            SortRemoveCharacters(obj);
                            GameLauncher.Instance._spawnedCharacters.Remove(obj.Id);
                            Runner.Despawn(obj);
                            StartCoroutine(ReSpawnTimer());
                        }
                    ).AddTo(this);
                    SortAddCharacters(obj);
                }));
            yield return new WaitForSeconds(0.5f);
        }

        private IEnumerator ReSpawnTimer()
        {
            yield return new WaitForSeconds(10f);
            StartCoroutine(ReSpawn());
        }

        private void SortAddCharacters(NetworkObject obj)
        {
            obj.transform.parent = _characters.transform;
            if (obj.HasInputAuthority)
            {
                _observeCharacters.MyCharacters.Add(obj.gameObject);
            }
            else if(!obj.HasInputAuthority)
            {
                _observeCharacters.EnemyCharacters.Add(obj.gameObject);
            }
        }

        private void SortRemoveCharacters(NetworkObject obj)
        {
            if (obj.HasInputAuthority)
            {
                _observeCharacters.MyCharacters.Remove(obj.gameObject);
            }
            else if (!obj.HasInputAuthority)
            {
                _observeCharacters.EnemyCharacters.Remove(obj.gameObject);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SpawnEffect()
        {
            if (Object.InputAuthority)
            {
                SoundManager.Instance.shotSe(SeType.Spawn);
                var effect = Instantiate(spawnEffect, transform.position, Quaternion.identity);
                Destroy(effect, 3);
            }
        }
    }
}
