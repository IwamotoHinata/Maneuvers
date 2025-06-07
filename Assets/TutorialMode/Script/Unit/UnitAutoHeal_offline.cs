using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class UnitAutoHeal_offline : MonoBehaviour
    {
        private static UnitStatus_offline MyUnitStatus;
        private static UnitBaseData_offline UnitBaseData;
        private static float Nowtime_heal = 0f;  //現在の時間（ヒール）
        private static float Healtime = 1f;  //ヒールするまでの時間
        private static float Nowtime_damage = 0f; //現在の時間（攻撃）
        private static float Damagetime = 10f;  //攻撃をくらってない必要な時間
        private static float Nowtime_count = 0f; //現在の時間（カウント）
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

                float hp = MyUnitStatus.HP.Value; //ｈｐ
                float maxhp = UnitBaseData.hp;
                float heal = UnitBaseData.recoverySpeed;

                if (Nowtime_heal >= Healtime && hp <= maxhp && Nowtime_damage >= Damagetime)
                {
                    hp += heal; //オートヒールの値？
                    MyUnitStatus.ChangeHP(hp); //オートヒール処理
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