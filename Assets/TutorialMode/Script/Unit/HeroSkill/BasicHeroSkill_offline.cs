using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Tutorial
{
    public class BasicHeroSkill_offline : HeroSkill_offline,IDisposable
    {
        private const float healMe = 50; //�񕜗�
        private const float healAlly = 25;
        private const float healRange = 50; //�񕜔͈�

        private UnitBaseData_offline MyUnitBaseData;
        private UnitStatus_offline MyUnitStatus;
        
        public IObservable<float> skillObservable_offline => skillSubject_offline;
        private Subject<float> skillSubject_offline = new Subject<float>();

        void Start()
        {
            //�R���|�[�l���g�擾
            MyUnitBaseData = GetComponent<UnitBaseData_offline>();
            MyUnitStatus = GetComponent<UnitStatus_offline>();

            //�R���[�`��
            StartCoroutine(CheckUseSkill(MyUnitBaseData.skillTime));
        }

        /// <summary>
        /// �p�b�V�u�X�L��
        /// </summary>
        public override void PassiveSkill()
        {
            Debug.Log("PassiveSkill() : ��{��");
        }

        /// <summary>
        /// �A�N�e�B�u�X�L��
        /// </summary>
        public override void ActiveSkill()
        {
            Debug.Log("ActiveSkill() : ��{��");
            //���g����
            MyUnitStatus.ChangeHP(MyUnitStatus.HP.Value + healMe);

            //���a50m�ȓ��̃v���C���[�q�[���[���j�b�g����
            var allUnit = GameObject.FindGameObjectsWithTag("Unit"); //�S�Ẵ��j�b�g���擾

            foreach (var no in allUnit)
            {
                //�R���|�[�l���g�擾
                if (no.TryGetComponent<UnitBaseData_offline>(out var TargetUnitBaseData)
                    && no.TryGetComponent<UnitStatus_offline>(out var TargetUnitStatus))
                {
                    //��������
                    if (TargetUnitBaseData.isHasInputAuthority() //�v���C���[
                        && TargetUnitBaseData != gameObject //���̃��j�b�g�ł͂Ȃ�
                        && TargetUnitBaseData.type != UnitType_offline.Minion //�~�j�I���ł͂Ȃ�
                        && Vector3.Distance(transform.position, no.transform.position) < healRange) //50m�ȓ�
                    {
                        //��������
                        TargetUnitStatus.ChangeHP(TargetUnitStatus.HP.Value + healAlly);
                    }
                }
            }
        }

        /// <summary>
        /// �A�N�e�B�u�X�L���Ď�
        /// </summary>
        /// <param name="skillTime"></param>
        /// <returns></returns>
        public override IEnumerator CheckUseSkill(float skillTime)
        {
            while (true)
            {
                //�����{�^��
                yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.E) //E�{�^��
                    || (MyUnitStatus.HP.Value / MyUnitBaseData.hp < 0.20f))); //hp��20%�ȉ�

                ActiveSkill();

                skillSubject_offline.OnNext(skillTime);
                //�N�[���^�C��
                yield return new WaitForSeconds(skillTime);
            }
        }

        public void Dispose()
        {
            skillSubject_offline.Dispose();
        }
    }
}
