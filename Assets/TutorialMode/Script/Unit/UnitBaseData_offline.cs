using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class UnitBaseData_offline : MonoBehaviour
    {
        //ユニットデータ
        public OwnerType_offline MyOwnerType { get; private set; } //権限
        public SpawnState_offline MySpawnState { get; private set; } //スポーン方式
        public UnitSpawner_offline MySpawner { get; private set; } //自身のスポナー

        public int unitID { get; private set; } //ユニットの識別番号
        public string unitName { get; private set; } //ユニット名
        public UnitType_offline type { get; private set; } //ユニットタイプ
        public float hp { get; private set; } //HP
        public float recoverySpeed { get; private set; } //回復速度
        public float moveSpeed { get; private set; } //移動速度
        public float attackRange { get; private set; } //射程
        public float attackRate { get; private set; } //レート
        public float attackPower { get; private set; } //攻撃力
        public float armor { get; private set; } //防御力
        public float staticHitRate { get; private set; } //静止命中率
        public float moveHitRate { get; private set; } //移動命中率
        public float reloadSpeed { get; private set; } //標準時間
        public float searchRange { get; private set; } //索敵範囲
        public float skillTime { get; private set; } //アクティブクールタイム

        public void InitBaseData(UnitSpawner_offline mySpawner,
            OwnerType_offline ownerType,
            SpawnState_offline spawnState,
            UnitData_offline unitData)
        {
            //初期化
            MySpawner = mySpawner;
            MyOwnerType = ownerType;
            MySpawnState = spawnState;
            
            unitID = unitData.unitID;
            unitName = unitData.unitName;
            type = unitData.type;
            hp = unitData.hp;
            recoverySpeed = unitData.recoverySpeed;
            moveSpeed = unitData.moveSpeed;
            attackRange = unitData.attackRange;
            attackRate = unitData.attackRate;
            attackPower = unitData.attackPower;
            armor = unitData.armor;
            staticHitRate = unitData.staticHitRate;
            moveHitRate = unitData.moveHitRate;
            reloadSpeed = unitData.reloadSpeed;
            searchRange = unitData.searchRange;
            skillTime = unitData.skillTime;

            //ユニットデータ受け渡し
            GetComponent<UnitStatus_offline>().InitStatus(unitData);
        }

        //このユニットが操作可能か
        public bool isHasInputAuthority()
        {
            if (MyOwnerType == OwnerType_offline.Player)
                return true;
            else
                return false;
        }

        //あるユニットが相手陣営か否か
        public bool isHasAuthority(UnitBaseData_offline TargetBaseData)
        {
            if (MyOwnerType == TargetBaseData.MyOwnerType)
                return true;
            else
                return false;
        }
    }
}