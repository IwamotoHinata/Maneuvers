using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

namespace Tutorial
{
    public class SceneManager1_offline : MonoBehaviour, IUpdated
    {
        [SerializeField] private SpawnerManager_offline _spawnerManager;
        [SerializeField] private UnitManager_offline _unitManager;
        [SerializeField] GameObject _timerUI;
        [SerializeField] TMP_Text _text;

        private int _gameEvent = 0;
        private int[] _port = new int[10];



        void Start()
        {
            //UpdateManager
            GameObject.Find("UpdateManager").
                GetComponent<UpdateManager_offline>().
                upd.Add(this);
        }

        public bool UpdateRequest() { return true; }
        public void Updated()
        {
            switch (_gameEvent)
            {
                case 0: //初期化
                    //プレイヤースポナーセット
                    _port[0] = _spawnerManager.SetSpawner(OwnerType_offline.Player,
                        SpawnState_offline.Auto,
                        0, 0, 0);

                    _gameEvent = 1000;
                    break;

                case 1:
                    //プレイヤースポナー移動
                    _spawnerManager.MoveSpawner(_port[0], 1);

                    _gameEvent = 1000;
                    break;

                case 2:
                    _spawnerManager.MoveSpawner(_port[0], 2);

                    //敵スポナーセット
                    _port[1] = _spawnerManager.SetSpawner(OwnerType_offline.Enemy,
                        SpawnState_offline.Manual,
                        0, 0, 0);

                    _gameEvent = 1000;
                    break;

                case 3:
                    _spawnerManager.MoveSpawner(_port[0], 3);

                    //スポナー削除
                    _spawnerManager.DeleteSpawner(_port[1]);

                    _port[1] = _spawnerManager.SetSpawner(OwnerType_offline.Enemy,
                        SpawnState_offline.Manual,
                        1, 0, 0);
                    _port[2] = _spawnerManager.SetSpawner(OwnerType_offline.Enemy,
                        SpawnState_offline.Manual,
                        2, 0, 0);
                    _port[3] = _spawnerManager.SetSpawner(OwnerType_offline.Enemy,
                        SpawnState_offline.Manual,
                        3, 0, 0);
                    _port[4] = _spawnerManager.SetSpawner(OwnerType_offline.Enemy,
                        SpawnState_offline.Manual,
                        4, 0, 0);
                    _port[5] = _spawnerManager.SetSpawner(OwnerType_offline.Enemy,
                        SpawnState_offline.Manual,
                        5, 0, 0);

                    _gameEvent = 1000;
                    break;

                case 4:
                    _spawnerManager.MoveSpawner(_port[0], 4);

                    _spawnerManager.DeleteSpawner(_port[1]);
                    _spawnerManager.DeleteSpawner(_port[2]);
                    _spawnerManager.DeleteSpawner(_port[3]);
                    _spawnerManager.DeleteSpawner(_port[4]);
                    _spawnerManager.DeleteSpawner(_port[5]);

                    _port[1] = _spawnerManager.SetSpawner(OwnerType_offline.Enemy,
                        SpawnState_offline.Manual,
                        6, 0, 0);
                    _port[2] = _spawnerManager.SetSpawner(OwnerType_offline.Enemy,
                        SpawnState_offline.Manual,
                        7, 0, 0);

                    _gameEvent = 1000;
                    break;

                case 5:
                    _spawnerManager.MoveSpawner(_port[0], 5);

                    _spawnerManager.DeleteSpawner(_port[1]);
                    _spawnerManager.DeleteSpawner(_port[2]);

                    _port[1] = _spawnerManager.SetSpawner(OwnerType_offline.Enemy,
                        SpawnState_offline.Manual,
                        8, 0, 0);
                    _port[2] = _spawnerManager.SetSpawner(OwnerType_offline.Enemy,
                        SpawnState_offline.Manual,
                        9, 0, 0);
                    _port[3] = _spawnerManager.SetSpawner(OwnerType_offline.Enemy,
                        SpawnState_offline.Manual,
                        10, 0, 0);
                    _port[4] = _spawnerManager.SetSpawner(OwnerType_offline.Enemy,
                        SpawnState_offline.Manual,
                        11, 0, 0);

                    _gameEvent = 1000;
                    break;

                case 6: //ゴール
                    Debug.Log("ゴール");

                    _spawnerManager.DeleteSpawner(_port[1]);
                    _spawnerManager.DeleteSpawner(_port[2]);
                    _spawnerManager.DeleteSpawner(_port[3]);
                    _spawnerManager.DeleteSpawner(_port[4]);

                    _gameEvent = 1000;
                    StartCoroutine(quitTutorial());
                    break;

                case 1000: //イベントなし

                    break;

                case 1001: //プレイヤー死亡
                    StartCoroutine(_spawnerManager.ResetEnemyUnit());

                    _gameEvent = 1000;
                    break;
            }
        }

        public void SetGameEvent(int value)
        {
            //Debug.Log("gameEvent = value");
            _gameEvent = value;
        }

        IEnumerator quitTutorial()
        {
            float time = 10.0f;
            _timerUI.SetActive(true);
            _text.text = "メインメニューに戻るまで..." + time.ToString();
            while (time > 0)
            { 
                yield return new WaitForSeconds(0.1f);
                time -= 0.1f;
                _text.text = "メインメニューに戻るまで..." + time.ToString("F1");
            }

            GridTransition.Instance.nextTransition = "MainMenu";
            GameObject.Find("GridTransition").GetComponent<Animator>().SetTrigger("isTransition");
        }
    }
}

