using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class UnitAttack_offline : MonoBehaviour, IUpdated
    {
        private UnitStatus_offline MyUnitStatus;
        private UnitBaseData_offline MyUnitBaseData;
        private IUnitMove_offline MyUnitMove;
        private GameObject _attackTarget = null; //攻撃相手
        private int _attackTrigger = 0; //攻撃相手のセット

        void Start()
        {
            //UpdateManager
            GameObject.Find("UpdateManager").
                GetComponent<UpdateManager_offline>().
                upd.Add(this);

            //コンポーネント取得
            MyUnitStatus = GetComponent<UnitStatus_offline>();
            MyUnitBaseData = GetComponent<UnitBaseData_offline>();
            MyUnitMove = GetComponent<IUnitMove_offline>();

            StartCoroutine(Attack());
        }

        public bool UpdateRequest() { return MyUnitStatus.iActive; }
        public void Updated()
        {
            //ターゲットを向く
            if (MyUnitStatus.iAttack
                && _attackTarget != null)
            {
                LookTarget(_attackTarget);
            }
        }

        private IEnumerator Attack()
        {
            while (true)
            {
                //攻撃相手のセット
                yield return new WaitUntil(() => _attackTrigger == 2);

                //コンポーネント取得
                if (_attackTarget == null) continue;
                if (_attackTarget.TryGetComponent<UnitBaseData_offline>(out var TargetUnitBaseData)
                    && _attackTarget.TryGetComponent<UnitStatus_offline>(out var TargetUnitStatus))
                {
                    //ターゲットセット
                    MyUnitStatus.SetIAttack(true); //攻撃開始
                    _attackTrigger = 1;
                    if (MyUnitStatus.iMoveState == UnitState_offline.VigilanceMove)
                        MyUnitMove.isStop();

                    while (true)
                    {
                        //弾を打つ
                        for (int i = 0; i < 5; i++) //弾数（未検討）
                        {
                            //ダメージ処理
                            if (MyUnitStatus.iMove) //移動
                            {
                                MyUnitStatus.SetIAttackState(UnitState_offline.MoveAttack);
                                if (MyUnitBaseData.moveHitRate > Random.Range(0f, 100f))
                                {
                                    //Debug.Log("is moveHit to " + _attackTarget);
                                    TargetUnitStatus.AddDamage(MyUnitStatus.attackPower);
                                }
                            }
                            else //停止
                            {
                                MyUnitStatus.SetIAttackState(UnitState_offline.Attack);
                                if (MyUnitBaseData.staticHitRate > Random.Range(0f, 100f))
                                {
                                    //aDebug.Log("is staticHit to " + _attackTarget);
                                    TargetUnitStatus.AddDamage(MyUnitStatus.attackPower);
                                }
                            }

                            //発射インターバル
                            yield return new WaitForSeconds(1 / MyUnitStatus.attackRate);

                            if (_attackTrigger != 1 || !TargetUnitStatus.iActive) break;
                        }

                        //新たなターゲットの補足orターゲット撃破
                        if (_attackTrigger != 1 || !TargetUnitStatus.iActive) break;

                        //リロード時間
                        MyUnitStatus.SetIAttackState(UnitState_offline.Reload);
                        yield return new WaitForSeconds(MyUnitStatus.reloadSpeed);

                        //新たなターゲットの補足orターゲット撃破
                        if (_attackTrigger != 1 || !TargetUnitStatus.iActive) break;
                    }

                    //攻撃終了
                    MyUnitStatus.SetIAttack(false);
                    MyUnitStatus.SetIAttackState(UnitState_offline.None);
                    MyUnitStatus.SetIState(MyUnitStatus.iMoveState);
                }
            }
        }

        /// <summary>
        /// ターゲットを格納
        /// </summary>
        /// <param name="target"></param>
        public void SetAttackTarget(GameObject target)
        {
            //ターゲットの変更
            if (target != _attackTarget)
            {
                _attackTarget = target;
                _attackTrigger = 0;

                //新しいターゲット
                if (target != null)
                {
                    //Debug.Log("new target == " + target);
                    _attackTrigger = 2;
                }
            }
        }

        /// <summary>
        /// ターゲットを向く
        /// </summary>
        /// <param name="target"></param>
        private void LookTarget(GameObject target)
        {
            Vector3 diff = target.transform.position - this.transform.position; //ベクトル
            Quaternion rot = Quaternion.LookRotation(diff);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rot, 0.2f);
        }
    }
}