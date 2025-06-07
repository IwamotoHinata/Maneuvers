using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Tutorial
{
    public class UnitStatus_offline : MonoBehaviour
    {
        private GameObject _spawnManager;
        private UnitSearch_offline MyUnitSearch;
        private UnitStateAction_offline MyUnitStateAction;
        private UnitBaseData_offline MyUnitBaseData;
        private IUnitMove_offline MyUnitMove;

        //ユニット状態
        public bool iSelected { get; private set; } = false; //ユニット選択状態
        public UnitState_offline iState { get; private set; } = UnitState_offline.Idle; //ユニット状態
        public UnitState_offline iMoveState { get; private set; } = UnitState_offline.Idle;
        public UnitState_offline iAttackState { get; private set; } = UnitState_offline.None;
        public bool iDiscovered { get; private set; } = false; //ユニットが発見されているか
        public bool inBush { get; private set; } = false; //草むらの中にいるか
        public bool iAttack { get; private set; } = false; //攻撃をする
        public bool iMove { get; private set; } = false; //移動状態
        public bool iActive { get; private set; } = true; //有効・無効

        //ユニットデータ
        private ReactiveProperty<float> hp = new ReactiveProperty<float>();
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

        public ReactiveProperty<float> HP => hp;

        void Start()
        {
            //コンポーネント設定
            _spawnManager = GameObject.Find("SpawnerManager");
            MyUnitSearch = GetComponent<UnitSearch_offline>();
            MyUnitStateAction = GetComponent<UnitStateAction_offline>();
            MyUnitBaseData = GetComponent<UnitBaseData_offline>();
            MyUnitMove = GetComponent<IUnitMove_offline>();
        }

        //ステータス初期化
        public void InitStatus(UnitData_offline unitData)
        {
            hp.Value = unitData.hp;
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
        }

        /// <summary>
        /// デスポーン
        /// </summary>
        /// <returns></returns>
        public IEnumerator DeSpawn()
        {
            //機能停止
            MyUnitMove.isStop();
            SetIActive(false);

            //死亡
            SetIState(UnitState_offline.Dead); //死亡
            yield return new WaitForSeconds(1.5f); //死亡アクション待機

            //再スポーン
            if (MyUnitBaseData.MySpawnState == SpawnState_offline.Auto)
                StartCoroutine(MyUnitBaseData.MySpawner.Spawn(0));

            if (MyUnitBaseData.isHasInputAuthority()
                && _spawnManager.TryGetComponent<SpawnerManager_offline>(out var spawnerManager))
                StartCoroutine(spawnerManager.ResetEnemyUnit());

            //削除
            yield return null; //制御待機
            Destroy(gameObject);
        }

        /// <summary>
        /// キャラにダメージを与える
        /// </summary>
        /// <param name="damage"></param>
        public void AddDamage(float damage)
        {
            //hpの変更
            //Debug.Log(damage - armor);
            if (damage - armor > 0)
                ChangeHP(hp.Value - (damage - armor));
        }

        //フラグセット
        public void SetISelected(bool value)
        {
            //Debug.Log(value);
            iSelected = value;
        }

        public void SetIState(UnitState_offline value)
        {
            //Debug.Log(value);
            iState = value;

            //アニメーション
            MyUnitStateAction.StateAction(value);
        }

        public void SetIMoveState(UnitState_offline value)
        {
            //Debug.Log(value);
            iMoveState = value;

            if (!iAttack) SetIState(value);
        }

        public void SetIAttackState(UnitState_offline value)
        {
            //Debug.Log(value);
            iAttackState = value;

            SetIState(value);
        }

        public void SetIDiscovered(bool value)
        {
            //Debug.Log(value);

            //可視状態の変更
            if (!MyUnitBaseData.isHasInputAuthority())
                MyUnitSearch.Visible(value);

            iDiscovered = value;
        }

        public void SetInBush(bool value)
        {
            //Debug.Log("inBush is " + value);
            inBush = value;
        }

        public void SetIAttack(bool value)
        {
            //Debug.Log(value);
            iAttack = value;
        }

        public void SetIMove(bool value)
        {
            //Debug.Log(value);
            iMove = value;
        }

        public void SetIActive(bool value)
        {
            //Debug.Log(value);
            iActive = value;
        }

        //ステータス変更
        public void ChangeHP(float value)
        {
            //Debug.Log(value);
            if (value > MyUnitBaseData.hp) value = MyUnitBaseData.hp;
            if (value < 0) value = 0;

            hp.Value = value;

            //死亡
            if (hp.Value == 0) StartCoroutine(DeSpawn());
        }
    }
}