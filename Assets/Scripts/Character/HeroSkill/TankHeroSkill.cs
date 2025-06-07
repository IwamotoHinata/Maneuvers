
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Online;
using UnityEngine.AI;
using UniRx;



namespace Unit
{
    public class TankHeroSkill : HeroSkill
    {
        private CharacterBaseData MyCharacterBaseData;
        [SerializeField] GameObject _shieldPrefab;//�V�[���h�̃v���n�u���
        private NetworkObject shield;//�V�[���h

        private CharacterRPCManager _manager;
        private CharacterStatus _characterStatus;
        private bool isTankMode = false;//���݂��h�`�Ԃ��ǂ���
        private int _enemyCount = 5;//�G�l�~�[�̗�
        private int currentCount;
        private int lastCount;
        private List<CharacterSpawner> _characterSpawner = new List<CharacterSpawner>();

        void Start()
        {
            _characterStatus = GetComponent<CharacterStatus>();
            MyCharacterBaseData = GetComponent<CharacterBaseData>();
            StartCoroutine(CheckUseSkill(MyCharacterBaseData.MySkillTime));
            SetEnemySpawner();
            StartCoroutine(HeroSkill());
        }

        /// <summary>
        ///�p�b�V�u�X�L���̏������L�q�i�g�p����ꏊ�͔C���܂��j
        /// </summary>
        public override void PassiveSkill()
        {
            //�p�b�V�u�N���X�̓��e���L�q
            Debug.Log("�^���N");
        }

        /// <summary>
        /// �A�N�e�B�u�X�L���̏������L�q
        /// </summary>
        public override void ActiveSkill()
        {
            TankSkill();
            //�X�L���̓��e���L�q
            Debug.Log("�^���N-active-");
        }

        /// <summary>
        /// �^���N�̃X�L���̏������L�q
        /// </summary>
        private void TankSkill()
        {
            isTankMode = !isTankMode;
            Debug.Log($"TankSkill{isTankMode}");
            RPC_TankSkillChangeStatus(isTankMode);
        }

        public void SetEnemySpawner()
        {
            var _allSpawner = FindObjectsOfType<CharacterSpawner>();
            while (_characterSpawner.Count < _enemyCount)
            {
                foreach (var item in _allSpawner)
                {
                    if (_characterSpawner.Contains(item))
                    {
                        continue;//�ǉ����邱�Ƃ��\�ȃ��j�b�g
                    }
                    if (!item.HasInputAuthority && MyCharacterBaseData.isHasInputAuthority())
                    {
                        _characterSpawner.Add(item);//���葤�̃��j�b�g���Ԃ�����ł���B
                    }
                    else if (item.HasInputAuthority && !MyCharacterBaseData.isHasInputAuthority())
                    {
                        _characterSpawner.Add(item);
                    }
                }
            }
        }

        IEnumerator HeroSkill()
        {
            while (true)
            {
                yield return new WaitUntil(() => isTankMode);
                yield return new WaitUntil(() => DiffAliveCount());
                Debug.Log("�G�̌��������m");
                if (isTankMode)
                {
                    Debug.Log("�̗͂���");
                    _characterStatus.HpHeal(MyCharacterBaseData.MyMaxHp / 2);
                }
            }
        }

        private int AliveCount()
        {
            int count = 0;
            for (int i = 0; i < _characterSpawner.Count; i++)
            {
                if(_characterSpawner[i].UnitAlive) count++;
            }
            return count;
        }

        private bool DiffAliveCount()
        {
            lastCount = currentCount;
            currentCount = AliveCount();
            int diff = lastCount - currentCount;
            return diff > 0 ? true : false;
        }
        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        public void RPC_TankSkillChangeStatus(bool SkillState, RpcInfo info = default)    //�^���N�̃X�L���ɂ��X�e�[�^�X�̕ύX
        {
            //�X�e�[�^�X�̕ύX����
            if (SkillState)   //�X�e�[�^�X�ύX
            {
                Debug.Log("�^���N�͖h��`�ԂɈڍs");
                _characterStatus.UpdateStatus(CurrentMoveSpeed: MyCharacterBaseData.MyMoveSpeed * 0.1f, DamageCut: 50);
            }
            else   //�X�e�[�^�X���f�t�H���g�ɖ߂�
            {
                Debug.Log("�^���N�͒ʏ�`�ԂɈڍs");
                _characterStatus.UpdateStatus();
            }
        }
    }
}