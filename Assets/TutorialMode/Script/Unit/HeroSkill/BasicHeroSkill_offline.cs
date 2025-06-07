using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Tutorial
{
    public class BasicHeroSkill_offline : HeroSkill_offline,IDisposable
    {
        private const float healMe = 50; //回復量
        private const float healAlly = 25;
        private const float healRange = 50; //回復範囲

        private UnitBaseData_offline MyUnitBaseData;
        private UnitStatus_offline MyUnitStatus;
        
        public IObservable<float> skillObservable_offline => skillSubject_offline;
        private Subject<float> skillSubject_offline = new Subject<float>();

        void Start()
        {
            //コンポーネント取得
            MyUnitBaseData = GetComponent<UnitBaseData_offline>();
            MyUnitStatus = GetComponent<UnitStatus_offline>();

            //コルーチン
            StartCoroutine(CheckUseSkill(MyUnitBaseData.skillTime));
        }

        /// <summary>
        /// パッシブスキル
        /// </summary>
        public override void PassiveSkill()
        {
            Debug.Log("PassiveSkill() : 基本兵");
        }

        /// <summary>
        /// アクティブスキル
        /// </summary>
        public override void ActiveSkill()
        {
            Debug.Log("ActiveSkill() : 基本兵");
            //自身を回復
            MyUnitStatus.ChangeHP(MyUnitStatus.HP.Value + healMe);

            //半径50m以内のプレイヤーヒーローユニットを回復
            var allUnit = GameObject.FindGameObjectsWithTag("Unit"); //全てのユニットを取得

            foreach (var no in allUnit)
            {
                //コンポーネント取得
                if (no.TryGetComponent<UnitBaseData_offline>(out var TargetUnitBaseData)
                    && no.TryGetComponent<UnitStatus_offline>(out var TargetUnitStatus))
                {
                    //条件判定
                    if (TargetUnitBaseData.isHasInputAuthority() //プレイヤー
                        && TargetUnitBaseData != gameObject //このユニットではない
                        && TargetUnitBaseData.type != UnitType_offline.Minion //ミニオンではない
                        && Vector3.Distance(transform.position, no.transform.position) < healRange) //50m以内
                    {
                        //味方を回復
                        TargetUnitStatus.ChangeHP(TargetUnitStatus.HP.Value + healAlly);
                    }
                }
            }
        }

        /// <summary>
        /// アクティブスキル監視
        /// </summary>
        /// <param name="skillTime"></param>
        /// <returns></returns>
        public override IEnumerator CheckUseSkill(float skillTime)
        {
            while (true)
            {
                //発動ボタン
                yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.E) //Eボタン
                    || (MyUnitStatus.HP.Value / MyUnitBaseData.hp < 0.20f))); //hpが20%以下

                ActiveSkill();

                skillSubject_offline.OnNext(skillTime);
                //クールタイム
                yield return new WaitForSeconds(skillTime);
            }
        }

        public void Dispose()
        {
            skillSubject_offline.Dispose();
        }
    }
}
