using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Fusion;

namespace Unit
{
    public class CharacterAutoHeal : NetworkBehaviour
    {
        [SerializeField]
        CharacterBaseData _myCharacterBaseData;
        CharacterStatus MyCharacterStatus;
        private float _maxHp;
        private float _healValue = 1;

        [Networked] private TickTimer damageTimer { get; set; }
        [Networked] private TickTimer healTimer { get; set; }

        void Start()
        {
            MyCharacterStatus = GetComponent<CharacterStatus>();
            _myCharacterBaseData
                .OninitialSetting
                .Where(value => value)
                .Subscribe(_ => Init()).AddTo(this);
        }
        
        void Init()
        {
            damageTimer = TickTimer.CreateFromSeconds(Runner, 0.1f);
            healTimer = TickTimer.CreateFromSeconds(Runner, 1.0f);
            _maxHp = _myCharacterBaseData.MaxHP;
            _healValue = _myCharacterBaseData.MyRecoverySpeed;

            if(MyCharacterStatus.HasStateAuthority)
            {
                MyCharacterStatus
                   .OnDamagedChanged
                   .Where(Damaged => true)
                   .Subscribe(_ =>
                   {
                       if(!MyCharacterStatus.GetCharacterState().Equals(CharacterState.Dead))
                       {
                           RPC_HealReset();
                       }
                   }).AddTo(this);
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (_myCharacterBaseData.isHasStateAuthority())
            { 
                if (damageTimer.Expired(Runner))
                {
                    if (healTimer.Expired(Runner))
                    {
                        if(MyCharacterStatus.CurrentHp < _myCharacterBaseData.MaxHP)
                        {
                            MyCharacterStatus.HpHeal(_healValue);
                        }
                        healTimer = TickTimer.CreateFromSeconds(Runner, 1.0f);
                    }
                }
            }          
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_HealReset(RpcInfo info = default)
        {
            damageTimer = TickTimer.CreateFromSeconds(Runner, 10.0f);
        }
    }
}