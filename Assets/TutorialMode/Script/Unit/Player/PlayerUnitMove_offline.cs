using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Tutorial
{
    public class PlayerUnitMove_offline : MonoBehaviour, IUpdated, IUnitMove_offline
    {
        private Camera _mainCamera;
        private NavMeshAgent _agent;
        private UnitStatus_offline MyUnitStatus;
        private List<Vector3> _bookPos = new List<Vector3>(); //予約した座標
        private List<UnitState_offline> _bookState = new List<UnitState_offline>(); //予約した状態

        void Start()
        {
            //UpdateManager
            GameObject.Find("UpdateManager").
                GetComponent<UpdateManager_offline>().
                upd.Add(this);

            //コンポーネント設定
            _mainCamera = Camera.main;
            MyUnitStatus = GetComponent<UnitStatus_offline>();
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = MyUnitStatus.moveSpeed;
        }

        public bool UpdateRequest() { return MyUnitStatus.iActive; }
        public void Updated()
        {
            SetBookPos(); //座標予約
            isArrive(); //到達判定
        }

        /// <summary>
        /// 座標の予約
        /// </summary>
        private void SetBookPos()
        {
            //右マウスボタン&ユニット選択状態
            if (Input.GetMouseButtonDown(1) && MyUnitStatus.iSelected)
            {
                //レイを飛ばしてマウス座標を確保
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (_bookPos.Count > 0) //予約座標が1つ以上あるとき
                    {
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            AddBook(hit.point);
                        }
                        else
                        {
                            //予約リストをリセットし、新たな座標を登録
                            RemoveBook("all");
                            AddBook(hit.point);
                            ToBook();
                        }
                    }
                    else if (_bookPos.Count == 0) //予約座標がないとき
                    {
                        //動き出す
                        AddBook(hit.point);
                        ToBook();
                        MyUnitStatus.SetIMove(true);
                    }
                }
            }
        }

        /// <summary>
        /// 座標への到着を判定
        /// </summary>
        private void isArrive()
        {
            if (_bookPos.Count >= 1) //予約座標が一つ以上ある時
            {
                //平面上の距離計算
                Vector2 myPos = new Vector2(transform.position.x, transform.position.z);
                Vector2 targetPos = new Vector2(_bookPos[0].x, _bookPos[0].z);
                float distancePlane = Vector2.Distance(myPos, targetPos);

                //目的地に到着
                if (distancePlane < 1f)
                {
                    if (_bookPos.Count > 1) //予約座標が二つ以上ある時
                    {
                        RemoveBook("zero");
                        ToBook();
                    }
                    else //予約座標が一つ
                    {
                        RemoveBook("zero");
                        MyUnitStatus.SetIMove(false);
                        MyUnitStatus.SetIMoveState(UnitState_offline.Idle);
                    }
                }
            }
        }

        /// <summary>
        /// 予約の追加
        /// </summary>
        /// <param name="pos"></param>
        private void AddBook(Vector3 pos)
        {
            _bookPos.Add(pos);
            if (Input.GetKey(KeyCode.Q)) _bookState.Add(UnitState_offline.VigilanceMove);
            else _bookState.Add(UnitState_offline.Move);
        }

        /// <summary>
        /// 予約の削除
        /// </summary>
        /// <param name="setting"></param>
        private void RemoveBook(string setting)
        {
            if (setting == "zero")
            {
                _bookPos.RemoveAt(0);
                _bookState.RemoveAt(0);
            }
            else if (setting == "all")
            {
                _bookPos.Clear();
                _bookState.Clear();
            }
        }

        /// <summary>
        /// 目的地への移動
        /// </summary>
        private void ToBook()
        {
            _agent.SetDestination(_bookPos[0]);
            MyUnitStatus.SetIMoveState(_bookState[0]);
        }

        /// <summary>
        /// 移動停止
        /// </summary>
        public void isStop()
        {
            _bookPos.Clear();
            _bookState.Clear();
            _agent.SetDestination(transform.position);
            MyUnitStatus.SetIMove(false);
            MyUnitStatus.SetIMoveState(UnitState_offline.Idle);
        }
    }
}
