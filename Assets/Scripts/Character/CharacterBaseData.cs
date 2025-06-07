using UnityEngine;
using UnityEngine.AI;
using UniRx;
using System;
using System.Collections;
using Fusion;
using TMPro;
using UnityEngine.UI;

namespace Unit
{
    public class CharacterBaseData : NetworkBehaviour
    {
       NavMeshAgent agent;
        private ReactiveProperty<bool> initialSetting = new ReactiveProperty<bool>(false);//初期値の設定が行われているかどうか
        [SerializeField]
        private OwnerType CharacterOwnerType;
        //[Networked] 

        private float MaxHp;
        private float attackPower;
        private int recoverySpeed;
        private float moveSpeed;
        private float attackRange;
        private float armor;
        private float staticHitRate;
        private float moveHitRate;
        private float reloadSpeed;
        private float searchRange;
        private float skillTime;
        private UnitType unitType;
        private int characterID;

       // [Networked] public Vector3 TestPosition { get; set; }

        [Networked]
        public PlayerRef local { get;set; }

        [SerializeField] UnitType showType;


        public IObservable<bool> OninitialSetting//数値の初期設定を行ったかどうかを通知する
        {
            get { return initialSetting; }
        }

        public float MyAttackPower { get => attackPower; }
        public int MyRecoverySpeed { get => recoverySpeed; }
        public float MyMoveSpeed { get => moveSpeed; }
        public float MyattackRange { get => attackRange; }
        public float MyArmor { get => armor; }
        public float MyStaticHitRate { get => staticHitRate; }
        public float MyMoveHitRate { get => moveHitRate; }
        public float MyReloadSpeed { get => reloadSpeed; }
        public float MysearchRange { get => searchRange; }
        public float MyHp { get => MaxHp; }
        public UnitType MyUnitType { get => unitType; }
        public float MySkillTime { get => skillTime; }
        public int MyCharacterID { get => characterID; }


        public float MaxHP;//ヒットポイントの最大値(回復の判定用)
        public float MyMaxHp { get => MaxHP; }

        private HeroSkill heroSkillCs;

        /// <summary>
        /// キャラクターの所有者を取得
        /// </summary>
        public OwnerType GetCharacterOwnerType()
        {
            return CharacterOwnerType;
        }

        public void SetCharacterOwnerType(OwnerType ownerType)
        {
            CharacterOwnerType = ownerType;
        }

        public bool isHasInputAuthority()
        {
            bool value = false;
            if (HasInputAuthority) value = true;
            return value;
        }
        public bool isHasStateAuthority()
        {
            bool value = false;
            if (HasStateAuthority) value = true;
            return value;
        }
        /// <summary>
        /// キャラクターの初期値を設定
        /// </summary>
        public void Init(CharacterData Data)
        {
            //Debug.Log($"Init:searchRange = {searchRange = Data.searchRange}:attackRange = {Data.attackRange}:MaxHp = {Data.hp}");
            attackPower = Data.attackPower;
            recoverySpeed = Data.recoverySpeed;
            moveSpeed = Data.moveSpeed;
            attackRange = Data.attackRange;
            armor = Data.armor;
            staticHitRate = Data.staticHitRate;
            moveHitRate = Data.moveHitRate;
            reloadSpeed = Data.reloadSpeed;
            searchRange = Data.searchRange;
            MaxHp = Data.hp;
            unitType = Data.type;
            skillTime = Data.skillTime;

            MaxHP = Data.hp;
            characterID = Data.characterID;

            agent = GetComponent<NavMeshAgent>();
            agent.enabled = true;
            agent.speed = Data.moveSpeed;

            initialSetting.Value = true;
            showType = unitType;//デバッグ用
        }
    }
}