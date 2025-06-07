using UnityEngine;
using System.Threading.Tasks;
using UniRx;
using System;
using Fusion;
using Online;
using UnityEngine.AI;

namespace Unit
{
    public class CharacterStateAction : NetworkBehaviour
    {
        private CharacterBaseData _myCharacterBaseData;
        private CharacterStatus _myCharacterStatus;
        private CharacterMove _myCharacterMove;
        private MinionMove _myMinionMove;
        private Animator _myAnimator;
        private NavMeshAgent _agent;
        private LogManager _deadLogManager;

        [SerializeField] private CharacterState nowState;
        public override void Spawned()
        {
            //base.Spawned();
            _myAnimator = GetComponent<Animator>();
            _myCharacterBaseData = GetComponent<CharacterBaseData>();
            _myCharacterStatus = GetComponent<CharacterStatus>();
            _agent = GetComponent<NavMeshAgent>();
            _deadLogManager = FindObjectOfType<LogManager>();

            _myCharacterBaseData
                .OninitialSetting
                .Where(value => value == true)
                .Subscribe(_ => Init()).AddTo(this);

            StateAction(CharacterState.Idle);
        }

        public void Init()
        {
            var ownerType = _myCharacterBaseData.GetCharacterOwnerType();
            if (ownerType == OwnerType.Player)
            {
                _myCharacterMove = GetComponent<CharacterMove>();
                _myCharacterMove
                    .OnMoveCompleted
                    .Where(Completed => Completed == true)
                    .Subscribe(_ =>
                    {
                        _myCharacterStatus.ChangeCharacterState(CharacterState.Idle);
                    }).AddTo(this);
            }
            else if (ownerType == OwnerType.Minion)
            {
                _myMinionMove = GetComponent<MinionMove>();
            }

            _myCharacterStatus
                .OnCharacterStateChanged
                .Where(characterState => characterState != CharacterState.Dead)
                .Subscribe(_ => StateAction(_myCharacterStatus.GetCharacterState()))
                .AddTo(this);

            _myCharacterStatus.ChangeCharacterState(CharacterState.Idle);
        }
        
        /// <summary>
        /// 各状態の行動
        /// </summary>
        private async void StateAction(CharacterState State)//状態を変更する関数
        {
            
            if (Object.HasInputAuthority)
            {
                RPC_Render(State);
            }

            nowState = _myCharacterStatus.GetCharacterState();
            var ownerType = _myCharacterBaseData.GetCharacterOwnerType();

            
            switch (State)
            {
                case CharacterState.Idle:
                    _myAnimator.SetBool("Move", false);
                    _myAnimator.SetBool("VigilancMove", false);
                    _myAnimator.SetTrigger("Idle");
                    
                    if (ownerType == OwnerType.Player && !_myCharacterMove.isArrival())
                    {
                        _myCharacterStatus.ChangeCharacterState(CharacterState.Move);
                    }

                    await Task.Delay(1000);
                    if (ownerType == OwnerType.Enemy)
                    {
                        _myCharacterStatus.ChangeCharacterState(CharacterState.VigilanceMove);
                        _myCharacterMove.RandomTargetPosition();
                        _myCharacterMove.MoveTarget();
                    }
                    break;
                case CharacterState.Attack:
                    _myAnimator.SetBool("Move", false);
                    if (ownerType == OwnerType.Player) _myCharacterMove.StopMove(true);
                    else if (ownerType == OwnerType.Minion) _myMinionMove.IsStopped = true;

                    _myAnimator.SetTrigger("Attack");
                    break;
                case CharacterState.Move:
                    _agent.updateRotation = true;
                    _myAnimator.SetBool("Move", true);
                    if (ownerType == OwnerType.Player) _myCharacterMove.StopMove(false);
                    else if (ownerType == OwnerType.Minion) _myMinionMove.IsStopped = false;
                    break;
                case CharacterState.MoveAttack:
                    _agent.updateRotation = false;

                    _myAnimator.SetBool("Move", false);
                    _myAnimator.SetBool("VigilancMove", false);
                    _myAnimator.SetTrigger("Attack");
                    break;
                case CharacterState.VigilanceMove:
                    _agent.updateRotation = true;
                    _myAnimator.SetBool("VigilancMove", true);
                    if (ownerType == OwnerType.Player) _myCharacterMove.StopMove(false);
                    else if (ownerType == OwnerType.Minion) _myMinionMove.IsStopped = false;
                    break;
                case CharacterState.Reload:
                    _myAnimator.SetTrigger("Reload");
                    if (ownerType == OwnerType.Player) _myCharacterMove.StopMove(true);
                    else if (ownerType == OwnerType.Minion) _myMinionMove.IsStopped = true;
                    break;
                case CharacterState.Dead:
                    //if (MyCharacterBaseData.GetCharacterOwnerType() == OwnerType.Player) _deadLogManager.AddText(gameObject.name + "が死亡しました", transform.position);
                    Debug.Log("死亡しました");
                    _myAnimator.SetTrigger("Die");
                    break;
            }
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        private void RPC_Render(CharacterState State)
        {
            _myCharacterStatus.ChangeCharacterState(State);
        }

    }
}