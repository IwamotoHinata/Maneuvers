using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class UnitAutoHeal_offline : MonoBehaviour
    {
        private static UnitStatus_offline MyUnitStatus;
        private static UnitBaseData_offline UnitBaseData;
        private static float Nowtime_heal = 0f;  //���݂̎��ԁi�q�[���j
        private static float Healtime = 1f;  //�q�[������܂ł̎���
        private static float Nowtime_damage = 0f; //���݂̎��ԁi�U���j
        private static float Damagetime = 10f;  //�U����������ĂȂ��K�v�Ȏ���
        private static float Nowtime_count = 0f; //���݂̎��ԁi�J�E���g�j
        private static float Counttime = 1f;
        private static float Pasthp = 0f;


        // Start is called before the first frame update
        void Start()
        {
            MyUnitStatus = GetComponent<UnitStatus_offline>();
            UnitBaseData = GetComponent<UnitBaseData_offline>();
            Pasthp = MyUnitStatus.HP.Value;
        }

        interface IFixedUpdate_offline
        {
            public void FixedUpdated()
            {
                Nowtime_heal += Time.deltaTime;
                Nowtime_count += Time.deltaTime;

                float hp = MyUnitStatus.HP.Value; //����
                float maxhp = UnitBaseData.hp;
                float heal = UnitBaseData.recoverySpeed;

                if (Nowtime_heal >= Healtime && hp <= maxhp && Nowtime_damage >= Damagetime)
                {
                    hp += heal; //�I�[�g�q�[���̒l�H
                    MyUnitStatus.ChangeHP(hp); //�I�[�g�q�[������
                    Nowtime_heal = 0f;
                }
                if (Pasthp == hp)
                {
                    if (Nowtime_count >= Counttime)
                    {
                        Nowtime_damage += 1f;
                    }
                }
                else if (Pasthp != hp)
                {
                    Nowtime_damage = 0f;
                    Pasthp = MyUnitStatus.HP.Value;
                }
            }
        }

    }
}