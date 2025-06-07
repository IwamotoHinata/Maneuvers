using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class SpawnerManager_offline : MonoBehaviour
    {
        [SerializeField] private MapDataList_offline _playerMap; //マップ情報
        [SerializeField] private MapDataList_offline _enemyMap;
        [SerializeField] private UnitPrefabList_offline _playerPrefab; //ユニット
        [SerializeField] private UnitPrefabList_offline _enemyPrefab;
        [SerializeField] private UnitDataList_offline _playerData; //ユニット情報
        [SerializeField] private UnitDataList_offline _enemyData;
        [SerializeField] private GameObject _spawner; //スポナー
        [SerializeField] private int _gameScene = 0; //ゲームシーン

        private const int _portNum = 100; //ポート数
        private GameObject[] _spawnerData = new GameObject[_portNum]; //スポナーポート配列

        /// <summary>
        /// スポナーを配置
        /// </summary>
        /// <param name="ownerType"></param>
        /// <param name="posNo"></param>
        /// <param name="prefabNo"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public int SetSpawner(OwnerType_offline ownerType,
            SpawnState_offline spawnState,
            int posNo, int prefabNo, int unitId)
        {
            //ポート番号設定
            int port = CreatePort();
            if (port == -1)
            {
                Debug.Log("err : setting port");
                return -1;
            }

            //スポナー生成
            _spawnerData[port] = Instantiate(_spawner);
            _spawnerData[port].transform.parent = this.gameObject.transform;

            //スポーン地点
            if (ownerType == OwnerType_offline.Player)
                _spawnerData[port].transform.position = _playerMap.list[_gameScene - 1].pos[posNo];
            else if (ownerType == OwnerType_offline.Enemy)
                _spawnerData[port].transform.position = _enemyMap.list[_gameScene - 1].pos[posNo];

            //スポナー初期化
            if (_spawnerData[port].TryGetComponent<UnitSpawner_offline>(out var unitSpawner))
            {
                if (ownerType == OwnerType_offline.Player)
                {
                    unitSpawner.InitSpawner(
                        OwnerType_offline.Player,
                        spawnState,
                        _playerPrefab.list[prefabNo],
                        _playerData.list[unitId]);
                }
                else if (ownerType == OwnerType_offline.Enemy)
                {
                    unitSpawner.InitSpawner(
                        OwnerType_offline.Enemy,
                        spawnState,
                        _enemyPrefab.list[prefabNo],
                        _enemyData.list[unitId]);
                }
            }

            return port;
        }

        /// <summary>
        /// スポナーの移動
        /// </summary>
        /// <param name="port"></param>
        /// <param name="to"></param>
        public void MoveSpawner(int port, int to)
        {
            if (_spawnerData[port] != null)
            {
                if (_spawnerData[port].TryGetComponent<UnitSpawner_offline>(out var TargetUnitSpawner))
                {
                    if (TargetUnitSpawner.MyOwnerType == OwnerType_offline.Player)
                        _spawnerData[port].transform.position = _playerMap.list[_gameScene - 1].pos[to];
                    else if (TargetUnitSpawner.MyOwnerType == OwnerType_offline.Enemy)
                        _spawnerData[port].transform.position = _enemyMap.list[_gameScene - 1].pos[to];
                }
            }
        }

        /// <summary>
        /// スポナー削除
        /// </summary>
        /// <param name="port"></param>
        public void DeleteSpawner(int port)
        {
            //Debug.Log("delete " + port + " port");
            Destroy(_spawnerData[port]);
        }

        /// <summary>
        /// ポート番号の設定
        /// </summary>
        /// <returns></returns>
        private int CreatePort()
        {
            int port = 0;

            while (port < _portNum)
            {
                if (_spawnerData[port] == null) return port;
                else port++;
            }

            return -1;
        }

        /// <summary>
        /// 敵ユニットのリセット
        /// </summary>
        /// <returns></returns>
        public IEnumerator ResetEnemyUnit()
        {
            //削除
            var allUnit = GameObject.FindGameObjectsWithTag("Unit"); //すべてのユニットを取得

            foreach (var no in allUnit)
            {
                //コンポーネント取得
                if (no.TryGetComponent<UnitBaseData_offline>(out var TargetUnitBaseData)
                    && no.TryGetComponent<UnitStatus_offline>(out var TargetStatus))
                {
                    if (!TargetUnitBaseData.isHasInputAuthority()) //敵
                    {
                        //ユニット無効
                        TargetStatus.SetIActive(false);

                        //削除
                        yield return null; //制御待機
                        Destroy(no);
                    }
                }
            }

            //スポーン
            var allSpawner = GameObject.FindGameObjectsWithTag("Spawner"); //すべてのスポナーを取得

            foreach (var no in allSpawner)
            {
                //コンポーネント取得
                if (no.TryGetComponent<UnitSpawner_offline>(out var unitSpawner))
                {
                    if (unitSpawner.MyOwnerType == OwnerType_offline.Enemy) //敵
                    {
                        StartCoroutine(unitSpawner.Spawn(0)); //スポーン
                    }
                }
            }
        }
    }
}