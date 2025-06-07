using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class UnitStateAction_offline : MonoBehaviour
    {
        private Animator _animator; //�A�j���[�^�[
        private UnitState_offline _prevState = UnitState_offline.Idle; //�O���͂̏��

        void Start()
        {
            //�R���|�[�l���g�ݒ�

            _animator = GetComponent<Animator>();
        }

        public void StateAction(UnitState_offline state)
        {
            switch (state)
            {
                case UnitState_offline.Idle: //�A�C�h����
                    if (_prevState != UnitState_offline.Idle)
                        _animator.SetTrigger("ToIdle");
                    break;
                case UnitState_offline.Move: //�ړ���
                    if (_prevState != UnitState_offline.Move)
                        _animator.SetTrigger("ToMove");
                       
                    break;
                case UnitState_offline.VigilanceMove: //�x���ړ�
                    if (_prevState != UnitState_offline.VigilanceMove)
                        _animator.SetTrigger("ToVigilanceMove");
                    break;
                case UnitState_offline.Attack: //�U���i�A�C�h���j
                    if (_prevState != UnitState_offline.Attack
                        && _prevState != UnitState_offline.Attack)
                        _animator.SetTrigger("ToAttack");
                    break;
                case UnitState_offline.MoveAttack: //�U���i�ړ��j
                    if (_prevState != UnitState_offline.Attack
                        && _prevState != UnitState_offline.MoveAttack)
                        _animator.SetTrigger("ToAttack");
                    break;
                case UnitState_offline.Reload: //�����[�h
                    if (_prevState != UnitState_offline.Reload)
                        _animator.SetTrigger("ToReload");
                    break;
                case UnitState_offline.Dead: //���S
                    if (_prevState != UnitState_offline.Dead)
                        _animator.SetTrigger("ToDead");
                    break;
            }

            _prevState = state;
        }
    }
}
