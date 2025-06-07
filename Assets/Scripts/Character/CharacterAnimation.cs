using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;
using UnityEngine.AI;

namespace Unit
{
    public class CharacterAnimation : NetworkBehaviour
    {
        private CharacterBaseData _myCharacterBaseData;
        private CharacterStatus _myCharacterStatus;
        private CharacterMove _myCharacterMove;
        private MinionMove _myMinionMove;
        private Animator _myAnimator;
        private NavMeshAgent _agent;
        private LogManager _deadLogManager;
        public override void Spawned()
        {
            _myAnimator = GetComponent<Animator>();
            _myCharacterBaseData = GetComponent<CharacterBaseData>();
            _myCharacterStatus = GetComponent<CharacterStatus>();
            _agent = GetComponent<NavMeshAgent>();
            _deadLogManager = FindObjectOfType<LogManager>();

            _myCharacterBaseData
                .OninitialSetting
                .Where(value => value == true)
                .Subscribe(_ => Init()).AddTo(this);
        }

        public void Init()
        {
            _myCharacterMove = GetComponent<CharacterMove>();
            _myCharacterMove
                .OnMoveCompleted
                .Where(Completed => Completed == true)
                .Subscribe(_ =>
                {
                    _myCharacterStatus.ChangeCharacterState(CharacterState.Idle);
                }).AddTo(this);


            _myCharacterStatus
                .OnCharacterStateChanged
                .Where(characterState => characterState != CharacterState.Dead)
                .Subscribe(CharacterState => changeAnimation(CharacterState))
                .AddTo(this);
        }

        private void changeAnimation(CharacterState state)
        {
            var ownerType = _myCharacterBaseData.GetCharacterOwnerType();

            switch (state)
            {
                case CharacterState.Idle:
                    _myAnimator.SetBool("Move", false);
                    _myAnimator.SetBool("VigilancMove", false);
                    _myAnimator.SetBool("Attack", false);

                    _myAnimator.SetTrigger("Idle");

                    break;
                    
                case CharacterState.Attack:
                    _myAnimator.SetBool("Move", false);
                    _myAnimator.SetBool("Attack", true);
                    if (ownerType == OwnerType.Player) _myCharacterMove.StopMove(true);

                    break;
                case CharacterState.Move:
                    _agent.updateRotation = true;
                    _myAnimator.SetBool("Move", true);
                    _myAnimator.SetBool("Attack", false);

                    if (ownerType == OwnerType.Player) _myCharacterMove.StopMove(false);

                    break;
                case CharacterState.MoveAttack:
                    _agent.updateRotation = false;

                    _myAnimator.SetBool("Move", false);
                    _myAnimator.SetBool("VigilancMove", false);
                    _myAnimator.SetBool("Attack", true);
                    break;
                case CharacterState.VigilanceMove:
                    _agent.updateRotation = true;
                    _myAnimator.SetBool("VigilancMove", true);
                    _myAnimator.SetBool("Attack", false);
                    if (ownerType == OwnerType.Player) _myCharacterMove.StopMove(false);
                    
                    break;
                case CharacterState.Reload:
                    _myAnimator.SetTrigger("Reload");
                    if (ownerType == OwnerType.Player) _myCharacterMove.StopMove(true);
                    
                    break;
                case CharacterState.Dead:
                    //if (MyCharacterBaseData.GetCharacterOwnerType() == OwnerType.Player) _deadLogManager.AddText(gameObject.name + "‚ªŽ€–S‚µ‚Ü‚µ‚½", transform.position);
                    Debug.Log("Ž€–S‚µ‚Ü‚µ‚½");
                    _myAnimator.SetTrigger("Die");
                    break;
            }
        }
    }
}
