using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class UnitSpawner_offline : MonoBehaviour
    {
        private GameObject _unitManagerObj; //ユニットマネージャーオブジェクト
        public OwnerType_offline MyOwnerType { get; private set; } //権限
        private SpawnState_offline _spawnState; //スポーン方式
        private GameObject _unitPrefab; //ユニットプレハブ
        private UnitData_offline _unitData; //ユニットデータ
        private UnitPresenter_Offline _unitPresenter;

        void Start()
        {
            //UnitManager
            _unitManagerObj = GameObject.Find("UnitManager");
        }

        /// <summary>
        /// スポナーの初期化
        /// </summary>
        /// <param name="ownerType"></param>
        /// <param name="spawnState"></param>
        /// <param name="unitPrefab"></param>
        /// <param name="unitData"></param>
        public void InitSpawner(OwnerType_offline ownerType,
            SpawnState_offline spawnState,
            GameObject unitPrefab,
            UnitData_offline unitData)
        {
            //初期化
            MyOwnerType = ownerType;
            _spawnState = spawnState;
            _unitPrefab = unitPrefab;
            _unitData = unitData;

            //１回目のスポーン
            StartCoroutine(Spawn(0));
        }

        /// <summary>
        /// スポーン
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public IEnumerator Spawn(float time)
        {
            //スポーン時間
            yield return new WaitForSeconds(time);

            //ユニット生成
            GameObject clone = Instantiate(_unitPrefab,
                this.gameObject.transform.position,
                Quaternion.identity);
            clone.transform.parent = _unitManagerObj.transform;
            

            //ユニット設定
            if (clone.TryGetComponent<UnitBaseData_offline>(out var unitBaseData))
            {
                unitBaseData.InitBaseData(this,
                    MyOwnerType,
                    _spawnState,
                    _unitData);
            }

            if (unitBaseData.isHasInputAuthority())
            {
                FindObjectOfType<UnitPresenter_Offline>().OnCreateUnit(clone);
            }
        }
    }
}