using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    [CreateAssetMenu(menuName = "For Tutorial/Create UnitData")]
    public class UnitData_offline : ScriptableObject
    {
        public int unitID; //ユニットの識別番号
        public string unitName; //ユニット名
        public UnitType_offline type; //ユニットタイプ
        public float hp; //hp
        public float recoverySpeed; //回復速度
        public float moveSpeed; //移動速度
        public float attackRange; //射程
        public float attackRate; //レート
        public float attackPower; //攻撃力
        public float armor; //防御力
        public float staticHitRate; //静止命中率
        public float moveHitRate; //移動命中率
        public float reloadSpeed; //照準時間
        public float searchRange; //視界範囲
        public float skillTime; //アクティブクールタイム
    }
}
