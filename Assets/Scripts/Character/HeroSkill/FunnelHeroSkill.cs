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
        [SerializeField] GameObject _funnelPrefab;//�t�@���l���̃v���n�u���
        private NetworkObject _funnel;//��������Ă���t�@���l��

        [SerializeField] CharacterData _data; //�t�@���l���̃f�[�^�\��
        private CharacterBaseData _profile;  //�t�@���l����BaseData
        private CharacterRPCManager _manager;//�t�@���l����RPCManager
        private CharacterMove _move;

        private bool _isViewer = false;//�t�@���l���̃v���r���[��\�����邩�ۂ�
        private GameObject _prefabViewer;//�t�@���l���̃v���r���[�p�I�u�W�F�N�g
        private Vector3 _viewerPos;//�v���r���[�̍��W.�X�L���g���邩�̏����ɂ��g�p

        private void Start()
        {
            StartCoroutine(checkUseSkillFunneler());            
        }

        private void Update()
        {
            if (_isViewer)
            {
                //�t�@���l���v���r���[�̍��W���w��
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);

                //viewerPos��ray�������������W���i�[
                if (hit.point == null)
                    _viewerPos = new Vector3(0,100,0);
                else
                    _viewerPos = hit.point;

                //����͐���,�X�L�������sor���������܂Ń}�E�X�|�C���^�ֈړ�
                if (_prefabViewer == null)
                    _prefabViewer = Instantiate(_funnelPrefab, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.Euler(0f, 0f, 0f));
                else
                    _prefabViewer.transform.position = hit.point;

            }
        }

        public override void ActiveSkill()
        {
            //�X�L���̓��e���L�q
            _isViewer = false;
            Destroy(_prefabViewer);
            funnelerSkill();
        }
        
        /// <summary>
        /// �t�@���l���̃X�L���̏������L�q
        /// </summary>
        private void funnelerSkill()
        {
            //�t�@���l�����쐬
            //���O�Ƀt�@���l�����쐬����Ă���ΑO�̃t�@���l����j��          
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
                    //funnel�̃v���r���[����
                    _isViewer = true;

                    myCharacterMove.isUseSkill = true;
                    Debug.Log("�t�@���l���X�L���N��");
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1));
                    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse0))//�X�L���g�p���~
                    {
                        _isViewer = false;
                        Destroy(_prefabViewer);
                        myCharacterMove.isUseSkill = false;

                        Debug.Log("�X�L���̎g�p�����~����܂����B");
                    }
                    else if (Input.GetKeyDown(KeyCode.Mouse1))//�X�L���g�p
                    {
                        if (_viewerPos != new Vector3(0, 100, 0))
                        {
                            _basicLeader.ReduceUltPoint();
                            ActiveSkill();
                            RPC_SkillUsed();
                            myCharacterMove.isUseSkill = false;
                            yield return new WaitForSeconds(skillTime);
                            Debug.Log("funnel�X�L���g�p�\");
                        }
                        else
                        {
                            _isViewer = false;
                            Destroy(_prefabViewer);
                            myCharacterMove.isUseSkill = false;
                            Debug.Log("funnel���o�����Ƃ��ł��܂���.�g�p���~");
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