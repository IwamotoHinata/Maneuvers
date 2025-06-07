
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Fusion;

namespace Unit
{
    public class Taisoukou : HeroSkill
    {
        private CharacterBaseData MyCharacterBaseData;
        private CharacterStatus MyCharacterStatus;
        private Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        private RaycastHit hit;
        private bool TaisouSkillActive = false; //�p�b�V�u�̃I���I�t�̃t���O

        void Start()
        {
            MyCharacterStatus = GetComponent<CharacterStatus>();
            MyCharacterBaseData = GetComponent<CharacterBaseData>();
            StartCoroutine(CheckUseSkill(MyCharacterBaseData.MySkillTime));
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        ///�p�b�V�u�X�L���̏������L�q�i�g�p����ꏊ�͔C���܂��j
        /// </summary>
        public override void PassiveSkill()
        {
            TaisouSkillActive = true;
            //MyCharacterStatus.ArmorPiercing(TaisouSkillActive);    //�X�e�[�^�X��ύX
            //�p�b�V�u�N���X�̓��e���L�q
            Debug.Log("�ϑ��b");
        }

        /// <summary>
        /// �A�N�e�B�u�X�L���̏������L�q
        /// </summary>
        public override void ActiveSkill()
        {
           
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.distance < 50)
                {
                    //�G����
                    bool enemyHit = false;
                    if (hit.collider.gameObject.GetComponent<CharacterBaseData>())
                    {
                        enemyHit = hit.collider.gameObject.GetComponent<CharacterBaseData>().isHasInputAuthority();
                    }

                    //�ʒu����
                    if (enemyHit == false && hit.collider.gameObject.GetComponent<CharacterStatus>())
                    {
                        //5�b��ɃI�u�W�F�N�g��HP�������l�ɂ���
                        Invoke(nameof(SwapObj), 5);
                    }

                }
            }

            //�X�L���̓��e���L�q
            Debug.Log("�ϑ��b-active-");

        }

        //Invoke�Ŏg��
        void SwapObj()
        {
            Vector3 tmp = GameObject.Find("Taisoukou").transform.position;

            hit.collider.gameObject.transform.position = tmp;

            tmp = hit.point;

            GameObject.Find("Taisoukou").transform.position = tmp;
        }
    }
}