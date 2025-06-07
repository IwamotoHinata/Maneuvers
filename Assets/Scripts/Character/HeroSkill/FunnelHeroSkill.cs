using Online;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using Fusion;

namespace Unit
{
    public class FunnelHeroSkill : HeroSkill
    {
        [SerializeField] GameObject _funnelPrefab;//ファンネルのプレハブ情報
        private NetworkObject _funnel;//召喚されているファンネル

        [SerializeField] CharacterData _data; //ファンネルのデータ構造
        private CharacterBaseData _profile;  //ファンネルのBaseData
        private CharacterRPCManager _manager;//ファンネルのRPCManager
        private CharacterMove _move;

        private bool _isViewer = false;//ファンネルのプレビューを表示するか否か
        private GameObject _prefabViewer;//ファンネルのプレビュー用オブジェクト
        private Vector3 _viewerPos;//プレビューの座標.スキル使えるかの条件にも使用

        private void Start()
        {
            StartCoroutine(checkUseSkillFunneler());            
        }

        private void Update()
        {
            if (_isViewer)
            {
                //ファンネルプレビューの座標を指定
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);

                //viewerPosにrayが当たった座標を格納
                if (hit.point == null)
                    _viewerPos = new Vector3(0,100,0);
                else
                    _viewerPos = hit.point;

                //初回は生成,スキルが実行or解除されるまでマウスポインタへ移動
                if (_prefabViewer == null)
                    _prefabViewer = Instantiate(_funnelPrefab, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.Euler(0f, 0f, 0f));
                else
                    _prefabViewer.transform.position = hit.point;

            }
        }

        public override void ActiveSkill()
        {
            //スキルの内容を記述
            _isViewer = false;
            Destroy(_prefabViewer);
            funnelerSkill();
        }
        
        /// <summary>
        /// ファンネルのスキルの処理を記述
        /// </summary>
        private void funnelerSkill()
        {
            //ファンネルを作成
            //事前にファンネルが作成されていれば前のファンネルを破壊          
            if (_funnel != null)
            {
                GameLauncher.Instance._spawnedCharacters.Remove(_funnel.Id);
                Runner.Despawn(_funnel);
            }

            RPC_SpawnFunnel();
            _move.RPC_SetMovePosition(_viewerPos, CharacterState.VigilanceMove, false);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        public void RPC_SpawnFunnel(RpcInfo info = default)
        {
            _funnel = GameLauncher.Runner.Spawn(
                    _funnelPrefab,
                    this.transform.position,
                    Quaternion.identity,
                    Object.InputAuthority,
                    ((runner, obj) =>
                    {
                        var agent = obj.GetComponent<NavMeshAgent>();
                        _profile = obj.GetComponent<CharacterBaseData>();
                        _manager = obj.GetComponent<CharacterRPCManager>();
                        _move = obj.GetComponent<CharacterMove>();
                        _manager.InitBaseData(_data);
                        _profile.SetCharacterOwnerType(OwnerType.Player);
                        _profile.local = Object.InputAuthority;
                        GameLauncher.Instance._spawnedCharacters.Add(obj.Id, obj);

                        var status = obj.GetComponent<CharacterStatus>();
                        status
                            .OnCharacterStateChanged
                            .Where(characterState => characterState == CharacterState.Dead)
                            .Subscribe(_ =>
                            {
                                GameLauncher.Instance._spawnedCharacters.Remove(obj.Id);
                                Runner.Despawn(obj);
                            }
                        ).AddTo(this);
                    }));
        }

        private IEnumerator checkUseSkillFunneler()
        {
            yield return null;
            myCharacterBaseData = GetComponent<CharacterBaseData>();
            myCharacterMove = GetComponent<CharacterMove>();
            _basicLeader = FindObjectOfType<BasicLeader>();
            yield return new WaitUntil(() => myCharacterBaseData.MySkillTime != 0);
            float skillTime = myCharacterBaseData.MySkillTime;

            while (true)
            {
                yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.E) && myCharacterMove.ReturnIsSelect() && myCharacterBaseData.isHasInputAuthority()));
                {
                    //funnelのプレビュー可視化
                    _isViewer = true;

                    myCharacterMove.isUseSkill = true;
                    Debug.Log("ファンネルスキル起動");
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1));
                    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse0))//スキル使用中止
                    {
                        _isViewer = false;
                        Destroy(_prefabViewer);
                        myCharacterMove.isUseSkill = false;

                        Debug.Log("スキルの使用が中止されました。");
                    }
                    else if (Input.GetKeyDown(KeyCode.Mouse1))//スキル使用
                    {
                        if (_viewerPos != new Vector3(0, 100, 0))
                        {
                            _basicLeader.ReduceUltPoint();
                            ActiveSkill();
                            RPC_SkillUsed();
                            myCharacterMove.isUseSkill = false;
                            yield return new WaitForSeconds(skillTime);
                            Debug.Log("funnelスキル使用可能");
                        }
                        else
                        {
                            _isViewer = false;
                            Destroy(_prefabViewer);
                            myCharacterMove.isUseSkill = false;
                            Debug.Log("funnelを出すことができません.使用中止");
                        }
                    }
                }
            }
        }
        
        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        void RPC_SkillUsed()
        {
            skillSubject.OnNext(myCharacterBaseData.MySkillTime);
        }
    }
}