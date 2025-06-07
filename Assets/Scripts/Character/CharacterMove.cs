using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using System;
using Fusion;

namespace Unit
{
    public class CharacterMove : CharacterStateActionBase

    {
        private ReactiveProperty<bool> moveCompleted = new ReactiveProperty<bool>(false);

        //[Networked]
        private ReactiveProperty<Vector3> moveTargetPosition { get; set; } = new ReactiveProperty<Vector3>();
        private NavMeshAgent Agent;
        CharacterStatus MyCharacterStatus;
        CharacterBaseData MyCharacterBaseData;
        private Camera mainCamera;
        private ReactiveProperty<bool> IsSelect = new ReactiveProperty<bool>(false);
        //private LeaderManager myLeaderManager;


        [Networked(OnChanged = nameof(ChangeMovepoint))]
        private NetworkBool IsDecideMovePoint { get; set; } = false;

        [Networked] private Vector3 NetworkPos { get; set; }

        public List<Vector3> bookPos;
        public List<CharacterState> bookState;

        private BasicLeader _basicLeader;

        [Networked] private TickTimer _stackCheckTimer { get; set; }
        [Networked] private TickTimer multiMoveStackTimer { get; set; }
        private Vector3 _stackCheckCurrentPosition;
        private Vector3 _stackCheckPastPosition;
        private float _stackRange;

        public bool isUseSkill = false;//スキルを使用している最中か否か

        //unitのアウトラインを表示するオブジェクト
        [Header("アウトラインを表示するオブジェクト")]
        [SerializeField] private GameObject[] visuals;

        [SerializeField] private TargetPin _targetPin;

        public static void ChangeMovepoint(Changed<CharacterMove> changed)
        {
            changed.Behaviour.SendMovePosition();
        }

        private void SendMovePosition()
        {
            if (IsDecideMovePoint == true)
            {
                MoveTarget();
                IsDecideMovePoint = false;
            }
        }

        public IObservable<Vector3> OnMoveTargetPositionChanged
        {
            get { return moveTargetPosition; }
        }

        public IObservable<bool> OnMoveCompleted //moveCompletedが変更されたときに送られるイベント
        {
            get { return moveCompleted; }
        }

        public IObservable<bool> isSelect //IsSelectが変更されたときに送られるイベント
        {
            get { return IsSelect; }
        }

        public void iSelect(bool value)
        {
            try
            {
                /*
                if (IsSelect.Value)
                {
                    //myLeaderManager.SetCharacter(this.Object.InputAuthority, this.gameObject);
                    foreach (GameObject child in visuals)
                    {
                        child.layer = 10;//layerの名前はUnitOutline
                    }
                }
                else
                {
                    //myLeaderManager.RemoveCharacter(this.Object.InputAuthority);
                    foreach (GameObject child in visuals)
                    {
                        child.layer = 6;//layerの名前はUnit
                    }
                }*/
            }
            finally
            {
                StateAction();
                SoundManager.Instance.shotSe(SeType.SelectChara);
                IsSelect.Value = value;
            }
        }

        public bool isArrival()
        {
            return moveCompleted.Value;
        }

        void Start()
        {
            //myLeaderManager = GameObject.Find("LeaderManager").GetComponent<LeaderManager>();
            _stackCheckCurrentPosition = transform.position;
            _stackCheckTimer = TickTimer.CreateFromSeconds(Runner, 3.0f);
        }

        public override void Spawned()
        {
            base.Spawned();
            mainCamera = Camera.main;
            MyCharacterStatus = GetComponent<CharacterStatus>();
            MyCharacterBaseData = GetComponent<CharacterBaseData>();
            Agent = GetComponent<NavMeshAgent>();
            Agent.enabled = true;
            bookPos = new List<Vector3>();
            bookState = new List<CharacterState>();
        }

        public void MoveTarget()
        {
            Agent.destination = moveTargetPosition.Value;
            moveCompleted.Value = false;
        }


        public void RandomTargetPosition()
        {
            moveTargetPosition.Value = transform.position +
                                       new Vector3(UnityEngine.Random.Range(-10f, 10), 0,
                                           UnityEngine.Random.Range(-10f, 10));
        }

        /*
        IEnumerator CompletedMove()
        {
            moveCompleted.Value = false;
            yield return new WaitUntil(() => CharacterToTragetDistance() < 1);

            if(bookPos.Count > 0)bookPos.RemoveAt(0);                                       //一時的にフリーズ予防で変更。しかし根源的なミスが何かはわかってない...orz修正必須
            if (bookState.Count > 0)bookState.RemoveAt(0);

            if (bookPos.Count != 0 && bookState.Count != 0)
            {
                RPC_MoveChara(bookPos[0], bookState[0]);
            }
            else
            {
                moveCompleted.Value = true;
            }
        }
        */

        private bool IslastBook;

        private void FixedUpdate()
        {
            if (Object.HasStateAuthority)
            {
                if (bookPos.Count > 1 && CharacterToTragetDistance() <= 1.0f)
                {
                    IslastBook = false;
                    RPC_RemoveBook(IslastBook);
                }
                else if (bookPos.Count == 1 && CharacterToTragetDistance() <= 30.0f)
                {
                    StartCoroutine(canMoveComplete());
                }
            }
        }

        IEnumerator canMoveComplete()
        {
            yield return new WaitForSeconds(4.5f * 6 / Agent.speed);
            if (bookPos.Count == 1)
            {
                IslastBook = true;
                RPC_RemoveBook(IslastBook);
                RPC_MoveChara(this.transform.position, CharacterState.Idle);
            }
        }

        private float CharacterToTragetDistance() //キャラクターと目標地点までの距離
        {
            //var Dinstance = Vector3.Distance(transform.position, moveTargetPosition.Value);
            float xRange = Mathf.Abs(transform.position.x - moveTargetPosition.Value.x);
            float zRange = Mathf.Abs(transform.position.z - moveTargetPosition.Value.z);
            var Distance = xRange + zRange;
            return Distance;
        }

        public void StopMove(bool value) //移動停止
        {
            if (Agent.enabled)
            {
                Agent.isStopped = value;
            }
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(1) && IsSelect.Value && Object.HasInputAuthority &&
                MyCharacterBaseData.MyUnitType != UnitType.Funnnel && !isUseSkill)
            {
                bool IsShift; //シフトを押してるか否か
                CharacterState state;
                IsShift = Input.GetKey(KeyCode.LeftShift);

                if (Input.GetKey(KeyCode.Q)) state = CharacterState.VigilanceMove;
                else state = CharacterState.Move;

                Vector3 mousePosition = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);

                RPC_SetMovePosition(hit.point, state, IsShift);
                SoundManager.Instance.shotSe(SeType.MoveToPoint);
            }
        }

        public override void StartStateAction()
        {
        }

        public override void StateAction()
        {
        }

        public override void EndStateAction()
        {
        }

        public bool ReturnIsSelect()
        {
            return IsSelect.Value;
        }

        public override void FixedUpdateNetwork()
        {
            if (_stackCheckTimer.Expired(Runner))
            {
                _stackCheckTimer = TickTimer.CreateFromSeconds(Runner, 3.0f);
                _stackCheckPastPosition = _stackCheckCurrentPosition;
                _stackCheckCurrentPosition = transform.position;
                if (_stackRange >= Vector3.Distance(_stackCheckPastPosition, _stackCheckCurrentPosition) &&
                    _stackRange >= Vector3.Distance(moveTargetPosition.Value, _stackCheckCurrentPosition) &&
                    _stackRange >= Vector3.Distance(_stackCheckPastPosition, moveTargetPosition.Value) &&
                    (MyCharacterStatus.GetCharacterState() == CharacterState.Move ||
                     MyCharacterStatus.GetCharacterState() == CharacterState.VigilanceMove))
                {
                    Debug.LogWarning("characterが移動を停止しているのに状態が移動状態のままです。");
                }
            }
        }

        public List<Vector3> ReturnBookPos()
        {
            return bookPos;
        }

        //bookPosとbookStateの情報をホスト・クライアント側で同じにする
        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        public void RPC_SetMovePosition(Vector3 setPosition, CharacterState state, bool IsShift, RpcInfo info = default)
        {
            if (IsShift == false)
            {
                bookPos.Clear();
                bookState.Clear();
            }

            if (state == CharacterState.VigilanceMove) bookState.Add(CharacterState.VigilanceMove);
            else bookState.Add(CharacterState.Move);

            if (_targetPin) _targetPin.setPoint(setPosition);  //ピンを立てる

            bookPos.Add(setPosition);
            if (bookPos.Count == 1 && MyCharacterStatus.HasStateAuthority)
                RPC_MoveChara(bookPos[0], bookState[0]); //座標決定後速やかに適応

        }

        /*この関数使わなくてもネットワーク上で動きますが、今後使うかもしれないので一応残しておきます*/
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_MoveChara(Vector3 movePosition, CharacterState state, RpcInfo info = default)
        {
            moveTargetPosition.Value = new Vector3(movePosition.x, transform.position.y, movePosition.z);
            MyCharacterStatus.ChangeCharacterState(state);
            MoveTarget();
        }

        //移動地点付近に到着したらbookPosとbookStateを1個ずつ消す
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_RemoveBook(bool lastBook, RpcInfo info = default)
        {
            Debug.Log("enter RPC_RemoveBook");

            if (lastBook == false)
            {
                bookPos.RemoveAt(0);
                bookState.RemoveAt(0);
                RPC_MoveChara(bookPos[0], bookState[0]);
                Debug.Log("RPC_RemoveBook:false");
            }
            else
            {

                bookPos.RemoveAt(0);
                bookState.RemoveAt(0);
                Debug.Log("RPC_RemoveBook:true");
                moveCompleted.Value = true;
                if (_targetPin) _targetPin.reset(); //ピンを消す
            }
        }
    }
}