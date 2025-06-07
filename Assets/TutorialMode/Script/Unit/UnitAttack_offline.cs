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
        private GameObject _attackTarget = null; //�U������
        private int _attackTrigger = 0; //�U������̃Z�b�g

        void Start()
        {
            //UpdateManager
            GameObject.Find("UpdateManager").
                GetComponent<UpdateManager_offline>().
                upd.Add(this);

            //�R���|�[�l���g�擾
            MyUnitStatus = GetComponent<UnitStatus_offline>();
            MyUnitBaseData = GetComponent<UnitBaseData_offline>();
            MyUnitMove = GetComponent<IUnitMove_offline>();

            StartCoroutine(Attack());
        }

        public bool UpdateRequest() { return MyUnitStatus.iActive; }
        public void Updated()
        {
            //�^�[�Q�b�g������
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
                //�U������̃Z�b�g
                yield return new WaitUntil(() => _attackTrigger == 2);

                //�R���|�[�l���g�擾
                if (_attackTarget == null) continue;
                if (_attackTarget.TryGetComponent<UnitBaseData_offline>(out var TargetUnitBaseData)
                    && _attackTarget.TryGetComponent<UnitStatus_offline>(out var TargetUnitStatus))
                {
                    //�^�[�Q�b�g�Z�b�g
                    MyUnitStatus.SetIAttack(true); //�U���J�n
                    _attackTrigger = 1;
                    if (MyUnitStatus.iMoveState == UnitState_offline.VigilanceMove)
                        MyUnitMove.isStop();

                    while (true)
                    {
                        //�e��ł�
                        for (int i = 0; i < 5; i++) //�e���i�������j
                        {
                            //�_���[�W����
                            if (MyUnitStatus.iMove) //�ړ�
                            {
                                MyUnitStatus.SetIAttackState(UnitState_offline.MoveAttack);
                                if (MyUnitBaseData.moveHitRate > Random.Range(0f, 100f))
                                {
                                    //Debug.Log("is moveHit to " + _attackTarget);
                                    TargetUnitStatus.AddDamage(MyUnitStatus.attackPower);
                                }
                            }
                            else //��~
                            {
                                MyUnitStatus.SetIAttackState(UnitState_offline.Attack);
                                if (MyUnitBaseData.staticHitRate > Random.Range(0f, 100f))
                                {
                                    //aDebug.Log("is staticHit to " + _attackTarget);
                                    TargetUnitStatus.AddDamage(MyUnitStatus.attackPower);
                                }
                            }

                            //���˃C���^�[�o��
                            yield return new WaitForSeconds(1 / MyUnitStatus.attackRate);

                            if (_attackTrigger != 1 || !TargetUnitStatus.iActive) break;
                        }

                        //�V���ȃ^�[�Q�b�g�̕⑫or�^�[�Q�b�g���j
                        if (_attackTrigger != 1 || !TargetUnitStatus.iActive) break;

                        //�����[�h����
                        MyUnitStatus.SetIAttackState(UnitState_offline.Reload);
                        yield return new WaitForSeconds(MyUnitStatus.reloadSpeed);

                        //�V���ȃ^�[�Q�b�g�̕⑫or�^�[�Q�b�g���j
                        if (_attackTrigger != 1 || !TargetUnitStatus.iActive) break;
                    }

                    //�U���I��
                    MyUnitStatus.SetIAttack(false);
                    MyUnitStatus.SetIAttackState(UnitState_offline.None);
                    MyUnitStatus.SetIState(MyUnitStatus.iMoveState);
                }
            }
        }

        /// <summary>
        /// �^�[�Q�b�g���i�[
        /// </summary>
        /// <param name="target"></param>
        public void SetAttackTarget(GameObject target)
        {
            //�^�[�Q�b�g�̕ύX
            if (target != _attackTarget)
            {
                _attackTarget = target;
                _attackTrigger = 0;

                //�V�����^�[�Q�b�g
                if (target != null)
                {
                    //Debug.Log("new target == " + target);
                    _attackTrigger = 2;
                }
            }
        }

        /// <summary>
        /// �^�[�Q�b�g������
        /// </summary>
        /// <param name="target"></param>
        private void LookTarget(GameObject target)
        {
            Vector3 diff = target.transform.position - this.transform.position; //�x�N�g��
            Quaternion rot = Quaternion.LookRotation(diff);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rot, 0.2f);
        }
    }
}