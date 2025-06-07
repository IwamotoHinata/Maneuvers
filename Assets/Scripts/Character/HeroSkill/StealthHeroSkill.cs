using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public class StealthHeroSkill : HeroSkill
    {
        //�t�B�[���h
        CharacterBaseData MyCharacterBaseData; //�L�����f�[�^�Q��
        CharacterStatus MyCharacterStatus;
        private GameObject obj_detectionRange; //�X�e���X�����͈�
        private StealthDetectionRange cs_detectionRange;
        private bool stealth = false; //�X�e���X���

        void Start()
        {
            //�G���m�͈͐���
            obj_detectionRange = new GameObject("detectionRange");
            obj_detectionRange.transform.parent = gameObject.transform;
            obj_detectionRange.AddComponent<StealthDetectionRange>();
            cs_detectionRange = obj_detectionRange.GetComponent<StealthDetectionRange>();

            //�R���|�[�l���g�擾
            MyCharacterStatus = GetComponent<CharacterStatus>();
            MyCharacterBaseData = GetComponent<CharacterBaseData>();
        }

        void Update()
        {
            //�ȉ����u��
            PassiveSkill();
            if (stealth) ActiveSkillManager();
        }

        /// <summary>
        /// �p�b�V�u�X�L��
        /// </summary>
        public override void PassiveSkill()
        {
            //Debug.Log("�X�e���X-passive-");

            //�U���͕ύX
            //Debug.Log("attackPower : " + attackPower);
            float attackPower = MyCharacterStatus.CurrentHp / MyCharacterBaseData.MaxHP * MyCharacterStatus.CurrentAttackPower;
            MyCharacterStatus.ChangeAttackPower(attackPower);
        }

        /// <summary>
        /// �A�N�e�B�u�X�L��
        /// </summary>
        public override void ActiveSkill()
        {
            //Debug.Log("�X�e���X-active-");

            //�s����ԁi���g�j
            if (MyCharacterBaseData.MaxHP - MyCharacterStatus.CurrentHp == 0)
            {
                //Debug.Log("�X�e���X��HP���ő�->�X�e���X���");
                stealth = true; //�X�e���X���
                MyCharacterStatus.ChangeMoveSpeed(6.0f); //�ړ����x�ύX
            }
        }

        /// <summary>
        /// �A�N�e�B�u�X�L���̊Ď��i�I������j
        /// stealth = true�@�̎��ɔ��s
        /// </summary>
        private void ActiveSkillManager()
        {
            //�I������
            if (cs_detectionRange.getUnitNum() != 0)
            {
                //Debug.Log("�G���P�O���ȓ�");
                stealth = false;
            }
            else if (MyCharacterStatus.CurrentIVisibleChanged)
            {
                //Debug.Log("���G���ꂽ");
                stealth = false;
            }
            else if (MyCharacterBaseData.MaxHP - MyCharacterStatus.CurrentHp != 0)
            {
                //Debug.Log("�_���[�W���󂯂��iHP�����j");
                stealth = false;
            }

            //�X�e���X����
            if (!stealth)
            {
                //Debug.Log("�X�e���X����");
                MyCharacterStatus.ChangeMoveSpeed(10.0f); //�ړ����x�ύX
            }
        }

        //�ϐ��A�N�Z�X
        public bool GetStealthState()
        {
            return stealth;
        }
    }
}
