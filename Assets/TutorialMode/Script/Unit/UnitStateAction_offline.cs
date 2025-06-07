using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class UnitStateAction_offline : MonoBehaviour
    {
        private Animator _animator; //アニメーター
        private UnitState_offline _prevState = UnitState_offline.Idle; //前入力の状態

        void Start()
        {
            //コンポーネント設定

            _animator = GetComponent<Animator>();
        }

        public void StateAction(UnitState_offline state)
        {
            switch (state)
            {
                case UnitState_offline.Idle: //アイドル時
                    if (_prevState != UnitState_offline.Idle)
                        _animator.SetTrigger("ToIdle");
                    break;
                case UnitState_offline.Move: //移動時
                    if (_prevState != UnitState_offline.Move)
                        _animator.SetTrigger("ToMove");
                       
                    break;
                case UnitState_offline.VigilanceMove: //警戒移動
                    if (_prevState != UnitState_offline.VigilanceMove)
                        _animator.SetTrigger("ToVigilanceMove");
                    break;
                case UnitState_offline.Attack: //攻撃（アイドル）
                    if (_prevState != UnitState_offline.Attack
                        && _prevState != UnitState_offline.Attack)
                        _animator.SetTrigger("ToAttack");
                    break;
                case UnitState_offline.MoveAttack: //攻撃（移動）
                    if (_prevState != UnitState_offline.Attack
                        && _prevState != UnitState_offline.MoveAttack)
                        _animator.SetTrigger("ToAttack");
                    break;
                case UnitState_offline.Reload: //リロード
                    if (_prevState != UnitState_offline.Reload)
                        _animator.SetTrigger("ToReload");
                    break;
                case UnitState_offline.Dead: //死亡
                    if (_prevState != UnitState_offline.Dead)
                        _animator.SetTrigger("ToDead");
                    break;
            }

            _prevState = state;
        }
    }
}
