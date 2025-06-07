using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using Fusion;

namespace Unit
{
    public class CharacterRPCManager : NetworkBehaviour
    {
        [Header("��{�f�[�^�̎Q��")]
        [SerializeField]
        private CharacterStateAction _myCharacterStateAction;
        [SerializeField]
        private CharacterBaseData _myCharacterBaseData;
        [SerializeField]
        private CharacterStatus _myCharacterStatus;
        [SerializeField]
        private CharacterVisible _myCharacterVisible;
        [SerializeField]
        private DisplayProfile MyDisplayProfile;
        private void Start()
        {
            _myCharacterBaseData
                .OninitialSetting
                .Where(value => value == true)
                .Subscribe(_ =>
                {
                    Debug.Log("CharacterRPCManager:�����l�̐ݒ肪����܂���");
                    Init();
                })
                .AddTo(this);
        }

        /// <summary>
        /// �����ݒ�
        /// </summary>
        private void Init()
        {
            Debug.Log("CharacterRPCManager: Init()");
            if (_myCharacterBaseData.isHasStateAuthority())
            {
                _myCharacterStatus
                    .OniVisibleChanged
                    .Where(_ => !_myCharacterStatus.GetCharacterState().Equals(CharacterState.Dead))
                    .Subscribe(value =>
                    {
                        if (_myCharacterBaseData.HasStateAuthority) { RPC_ChangeVisible(value); }
                    }
                ).AddTo(this);

                
                _myCharacterStatus
                     .OnCharacterHPChanged
                     .Where(_ => !_myCharacterStatus.GetCharacterState().Equals(CharacterState.Dead))
                     .Subscribe(characterHP =>
                     {
                             if (characterHP < 0) characterHP = 0;
                             RPC_ShareHp(characterHP);
                     }
                 ).AddTo(this);


                //���ݏ�Ԃ̓���
                _myCharacterStatus
                    .OnCharacterStateChanged
                    .Where(_ => !_myCharacterStatus.GetCharacterState().Equals(CharacterState.Dead))
                    .Subscribe(CharacterState =>
                    {
                        RPC_StateShow(CharacterState.ToString());
                        RPC_SetCharacterState(CharacterState);
                    }
                 ).AddTo(this);
                _myCharacterStatus
                    .OnDeadEvent
                    .Where(isDead => isDead && !_myCharacterStatus.GetCharacterState().Equals(CharacterState.Dead))
                    .Subscribe(_ =>
                    {
                        Debug.Log("Dead");
                        RPC_ShareHp(0);
                        RPC_StateShow("Dead");
                        RPC_SetDeadCharacterState();
                    }).AddTo(this);


                //�U���͈͂̓���
                _myCharacterStatus
                    .OnCurrentAttackRange
                    .Where(_ => !_myCharacterStatus.GetCharacterState().Equals(CharacterState.Dead))
                    .Subscribe(CurrentAttackRange =>
                    {
                        RPC_SetCharacterAttackRange(CurrentAttackRange);
                    }
                 ).AddTo(this);


                //���G�͈͂̓���
                _myCharacterStatus
                    .OnCurrentSearchRange
                    .Where(_ => !_myCharacterStatus.GetCharacterState().Equals(CharacterState.Dead))
                    .Subscribe(CurrentSearchRange =>
                    {
                        RPC_SetCharacterSearchRange(CurrentSearchRange);
                    }
                 ).AddTo(this);
            }
        }


        public void InitBaseData(CharacterData Data)
        {
            StartCoroutine(delaySet(Data));
        }

        IEnumerator delaySet(CharacterData Data)
        {
            yield return new WaitForSeconds(1f);
            RPC_SetBaseData(Data.characterID,
                            Data.characterName,
                            Data.hp, Data.recoverySpeed,
                            Data.moveSpeed,
                            Data.attackRange,
                            Data.attackRate,
                            Data.attackPower,
                            Data.armor,
                            Data.staticHitRate,
                            Data.moveHitRate,
                            Data.reloadSpeed,
                            Data.searchRange,
                            Data.skillTime,
                            Data.type.ToString());
        }

        /// <summary>
        /// ���o���̐؂�ւ�
        /// </summary>
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_ChangeVisible(bool value)
        {
            if (_myCharacterVisible != null)
            {
                Debug.Log($"MyCharacterVisible��{value}�ɐ؂�ւ��܂���");
                if (_myCharacterBaseData.isHasInputAuthority() == false) _myCharacterVisible.Visible(value);
                if (_myCharacterBaseData.isHasStateAuthority() == false) _myCharacterStatus.ChangeiVisible(value);
            }
            else
            {
                Debug.LogWarning("MyDisplayProfile = Null");
            }
        }

        /// <summary>
        /// ��Ԃ̍X�V
        /// </summary>
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_StateShow(string value)
        {
            if (MyDisplayProfile != null)
            {
                if (!_myCharacterBaseData.isHasStateAuthority())
                {
                    MyDisplayProfile.StateShow(value);
                }
            }
            else
            {
                Debug.LogWarning("MyDisplayProfile = Null");
            }
        }

        /// <summary>
        /// �̗͂̍X�V
        /// </summary>
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_ShareHp(float value)
        {
            if (_myCharacterStatus != null && _myCharacterBaseData != null)
            {
                if (!_myCharacterBaseData.isHasStateAuthority())
                {
                    _myCharacterStatus.ChangeHPValue(value);
                }
            }
            else
            {
                Debug.LogWarning("MyDisplayProfile = Null");
            }
        }

        /// <summary>
        /// ��Ԃ̋��L
        /// </summary>
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SetCharacterState(CharacterState State)
        {
            if (_myCharacterStatus != null && !_myCharacterStatus.Equals(CharacterState.Dead))
            {
                if (!_myCharacterBaseData.isHasStateAuthority())
                {
                    _myCharacterStatus.ChangeCharacterState(State);
                }
            }
            else
            {
                Debug.LogWarning("MyCharacterStatus = Null");

            }
        }

        /// <summary>
        /// ���S��Ԃ̋��L
        /// </summary>
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SetDeadCharacterState()
        {
            if (_myCharacterStatus != null && !_myCharacterStatus.Equals(CharacterState.Dead))
            {
                    _myCharacterStatus.ChangeCharacterState(CharacterState.Dead);
            }
            else
            {
                Debug.LogWarning("MyCharacterStatus = Null");

            }
        }

        /// <summary>
        /// �U���͈͂̋��L
        /// </summary>
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SetCharacterAttackRange(float AttackRange)
        {
            if (MyDisplayProfile != null)
            {
                MyDisplayProfile.setAttackRangeImage(AttackRange);
            }
            else
            {
                Debug.LogWarning("MyDisplayProfile = Null");
            }
        }

        /// <summary>
        /// ���G�͈͂̋��L
        /// </summary>
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SetCharacterSearchRange(float SearchRange)
        {
            if (MyDisplayProfile != null)
            {
                MyDisplayProfile.setSearchRangeImage(SearchRange);
            }
            else
            {
                Debug.LogWarning("MyDisplayProfile = Null");
            }
        }


        /// <summary>
        /// ��{�f�[�^�̋��L
        /// </summary>
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SetBaseData(int characterID, string characterName, float hp, int recoverySpeed, float moveSpeed, float attackRange, float attackRate, float attackPower, float armor, float staticHitRate, float moveHitRate, float reloadSpeed, float searchRange, float skillTime, string type)
        {
            CharacterData newData = ScriptableObject.CreateInstance<CharacterData>();
            newData.characterID = characterID;
            newData.characterName = characterName;
            newData.recoverySpeed = recoverySpeed;
            newData.moveSpeed = moveSpeed;
            newData.hp = hp;
            newData.attackPower = attackPower;
            newData.attackRange = attackRange;
            newData.attackRate = attackRate;
            newData.staticHitRate = staticHitRate;
            newData.armor = armor;
            newData.moveHitRate = moveHitRate;
            newData.reloadSpeed = reloadSpeed;
            newData.skillTime = skillTime;
            newData.searchRange = searchRange;

            newData.type = (UnitType)characterID;

            if (_myCharacterBaseData != null)
            {
                _myCharacterBaseData.Init(newData);
            }
            else
            {
                Debug.LogWarning("MyCharacterBaseData = Null");
            }
        }
    }
}