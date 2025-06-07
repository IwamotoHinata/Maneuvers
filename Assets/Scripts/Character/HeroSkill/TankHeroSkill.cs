
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
        [SerializeField] GameObject _shieldPrefab;//シールドのプレハブ情報
        private NetworkObject shield;//シールド

        private CharacterRPCManager _manager;
        private CharacterStatus _characterStatus;
        private bool isTankMode = false;//現在が防形態かどうか
        private int _enemyCount = 5;//エネミーの量
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
        ///パッシブスキルの処理を記述（使用する場所は任せます）
        /// </summary>
        public override void PassiveSkill()
        {
            //パッシブクラスの内容を記述
            Debug.Log("タンク");
        }

        /// <summary>
        /// アクティブスキルの処理を記述
        /// </summary>
        public override void ActiveSkill()
        {
            TankSkill();
            //スキルの内容を記述
            Debug.Log("タンク-active-");
        }

        /// <summary>
        /// タンクのスキルの処理を記述
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
                        continue;//追加することが可能なユニット
                    }
                    if (!item.HasInputAuthority && MyCharacterBaseData.isHasInputAuthority())
                    {
                        _characterSpawner.Add(item);//相手側のユニットをぶち込んでいる。
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
                Debug.Log("敵の減少を検知");
                if (isTankMode)
                {
                    Debug.Log("体力を回復");
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
        public void RPC_TankSkillChangeStatus(bool SkillState, RpcInfo info = default)    //タンクのスキルによるステータスの変更
        {
            //ステータスの変更処理
            if (SkillState)   //ステータス変更
            {
                Debug.Log("タンクは防御形態に移行");
                _characterStatus.UpdateStatus(CurrentMoveSpeed: MyCharacterBaseData.MyMoveSpeed * 0.1f, DamageCut: 50);
            }
            else   //ステータスをデフォルトに戻す
            {
                Debug.Log("タンクは通常形態に移行");
                _characterStatus.UpdateStatus();
            }
        }
    }
}