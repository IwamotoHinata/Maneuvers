using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class UnitSearch_offline : MonoBehaviour, IFixedUpdated
    {
        private const float unitHeight = 1f; //ユニットの高さ
        private const float closeArea = 10f; //ユニットの間合い

        private UnitManager_offline unitManager;
        private UnitBaseData_offline MyUnitBaseData;
        private UnitStatus_offline MyUnitStatus;
        private UnitAttack_offline MyUnitAttack;
        private int enterBushCount = 0;

        void Start()
        {
            //UpdateManager
            GameObject.Find("UpdateManager").
                GetComponent<UpdateManager_offline>().
                fix.Add(this);

            //コンポーネント取得
            unitManager = GameObject.Find("UnitManager").
                GetComponent<UnitManager_offline>();
            MyUnitBaseData = GetComponent<UnitBaseData_offline>();
            MyUnitStatus = GetComponent<UnitStatus_offline>();
            MyUnitAttack = GetComponent<UnitAttack_offline>();
        }

        public bool UpdateRequest() { return MyUnitStatus.iActive; }
        public void FixedUpdated()
        {
            VisibleSearch();
            AttackSearch();
        }

        /// <summary>
        /// 索敵処理
        /// </summary>
        private void VisibleSearch()
        {
            var allUnit = GameObject.FindGameObjectsWithTag("Unit"); //すべてのユニットを取得

            foreach (var no in allUnit)
            {
                //コンポーネント取得
                if (no.TryGetComponent<UnitStatus_offline>(out var TargetUnitStatus)
                    && no.TryGetComponent<UnitBaseData_offline>(out var TargetUnitBaseData))
                {
                    //視線にヒットしたオブジェクトの情報
                    int hitObj = CheckSightPass(no);

                    //条件判定
                    if (!MyUnitBaseData.isHasAuthority(TargetUnitBaseData) //相手陣営
                        && Vector3.Distance(transform.position, no.transform.position) <= MyUnitBaseData.searchRange //視界範囲内
                        && hitObj != -1 //壁を貫通していない
                        && (Vector3.Distance(transform.position, no.transform.position) <= closeArea //間合いである
                        || InBushSightPass(TargetUnitStatus, hitObj))) //草むらの特殊な条件判定
                    {
                        //発見
                        if (MyUnitBaseData.isHasInputAuthority()) //プレイヤー陣営
                        {
                            if (!unitManager.enemyVisible.Contains(no))
                            {
                                unitManager.enemyVisible.Add(no);
                                TargetUnitStatus.SetIDiscovered(true);
                            }
                        }
                        else //敵陣営
                        {
                            if (!unitManager.playerVisible.Contains(no))
                            {
                                unitManager.playerVisible.Add(no);
                                TargetUnitStatus.SetIDiscovered(true);
                            }
                        }

                        //デバッグ
                        /*Debug.DrawLine(transform.position + new Vector3(0, unitHeight, 0),
                            no.transform.position + new Vector3(0, unitHeight, 0),
                            Color.blue,
                            0.05f);*/
                    }
                    else
                    {
                        //未発見
                        if (MyUnitBaseData.isHasInputAuthority()) //プレイヤー陣営
                        {
                            if (unitManager.enemyVisible.Contains(no))
                            {
                                unitManager.enemyVisible.Remove(no);
                                TargetUnitStatus.SetIDiscovered(false);
                            }
                        }
                        else //敵陣営
                        {
                            if (unitManager.playerVisible.Contains(no))
                            {
                                unitManager.playerVisible.Remove(no);
                                TargetUnitStatus.SetIDiscovered(false);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 攻撃処理
        /// </summary>
        private void AttackSearch()
        {
            var allUnit = GameObject.FindGameObjectsWithTag("Unit"); //すべてのユニットを取得
            List<GameObject> inAttackRangeUnit = new List<GameObject>(); //攻撃範囲内のユニット
            List<GameObject> inAttackRangeMinion = new List<GameObject>(); //攻撃範囲内のミニオン

            foreach (var no in allUnit)
            {
                //コンポーネント取得
                if (no.TryGetComponent<UnitBaseData_offline>(out var TargetUnitBaseData)
                    && no.TryGetComponent<UnitStatus_offline>(out var TargetUnitStatus))
                {
                    //ヒットしたオブジェクト情報
                    int hitObj = CheckSightPass(no);

                    //条件判定
                    if (TargetUnitStatus.iActive //相手がactive
                        && !MyUnitBaseData.isHasAuthority(TargetUnitBaseData) //相手陣営
                        && (TargetUnitStatus.iDiscovered //相手を発見している
                        || MyUnitStatus.iState == UnitState_offline.VigilanceMove) //警戒移動中or
                        && Vector3.Distance(transform.position, no.transform.position) <= MyUnitBaseData.attackRange //攻撃範囲内
                        && hitObj != -1 //壁を貫通していない
                        && (Vector3.Distance(transform.position, no.transform.position) <= closeArea //間合いor
                        || InBushSightPass(TargetUnitStatus, hitObj))) //草むらの特殊判定
                    {
                        //攻撃範囲内
                        inAttackRangeUnit.Add(no);
                        if (TargetUnitBaseData.type == UnitType_offline.Minion)
                            inAttackRangeMinion.Add(no);

                        //デバッグ用
                        /*Debug.DrawLine(transform.position + new Vector3(0, unitHeight, 0),
                            no.transform.position + new Vector3(0, unitHeight, 0),
                            Color.red,
                            0.05f);*/
                    }
                }
            }

            //ターゲットの指定
            GameObject attackTarget = null;

            if (MyUnitBaseData.type == UnitType_offline.Minion) //ミニオンから選択
                attackTarget = DistanceSort(inAttackRangeMinion);

            if (attackTarget == null) //ユニットから選択
                attackTarget = DistanceSort(inAttackRangeUnit);

            MyUnitAttack.SetAttackTarget(attackTarget);
        }

        /// <summary>
        /// 視線のオブジェクト判定
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private int CheckSightPass(GameObject target)
        {
            //草むらの個数
            int passBushCount = 0;

            //レイの設定
            Vector3 diff = target.transform.position - transform.position;
            Vector3 direction = diff.normalized; //方向
            float distance = diff.magnitude; //距離

            //レイを飛ばす
            RaycastHit[] hits;
            hits = Physics.RaycastAll(transform.position + new Vector3(0, unitHeight, 0), direction, distance);

            //オブジェクト情報
            foreach (var no in hits)
            {
                //マップ（壁・地形）
                if (no.transform.CompareTag("Stage"))
                    return -1;

                //マップ（草むら）
                if (no.transform.CompareTag("Bush"))
                    passBushCount++;
            }

            return passBushCount;
        }

        /// <summary>
        /// 草むらの特殊な条件判定
        /// </summary>
        /// <param name="target"></param>
        /// <param name="bushNum"></param>
        /// <returns></returns>
        private bool InBushSightPass(UnitStatus_offline TargetUnitStatus, int bushNum)
        {
            //bushNum == 2の場合は視線が通らない
            if (bushNum == 1)
            {
                if ((MyUnitStatus.inBush == true //自陣営が草むらの中
                    && TargetUnitStatus == false) //敵陣営が草むらの外
                    || TargetUnitStatus.iAttack) //相手が射撃中
                    return true;
            }
            else if (bushNum == 0)
                return true;

            return false;
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="value"></param>
        public void Visible(bool value)
        {
            //ユニットの描画処理
            if (value)
                gameObject.SetLayerToDown("Default");
            else
                gameObject.SetLayerToDown("Ignore UI");
        }

        /// <summary>
        /// 距離の計算（一番近いユニットを返す）
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private GameObject DistanceSort(List<GameObject> list)
        {
            GameObject ret = null;

            if (list.Count > 1)
            {
                ret = list[0];
                for (int i = 1; i < list.Count; i++)
                {
                    if (Vector3.Distance(transform.position, ret.transform.position)
                        < Vector3.Distance(transform.position, list[i].transform.position))
                        ret = list[i];
                }
            }
            else if (list.Count == 1)
            {
                ret = list[0];
            }
            else
            {
                ret = null;
            }

            return ret;
        }

        //草むらの判定
        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Bush"))
            {
                enterBushCount++;
                if (enterBushCount == 1)
                {
                    MyUnitStatus.SetInBush(true);
                }
                //Debug.Log(enterBushCount);
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Bush"))
            {
                enterBushCount--;
                if (enterBushCount == 0)
                {
                    MyUnitStatus.SetInBush(false);
                }
                //Debug.Log(enterBushCount);
            }
        }
    }
}
