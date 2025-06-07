using System;
using System.Collections;
using Fusion;
using Unit;
using UnityEngine;

namespace Online
{
    public enum BattlePhase
    {
        SetUp,
        First,
        Second,
        Third,
        End
    }
    
    public class SpawnDelay : NetworkBehaviour
    {
        [SerializeField] private GameObject camaraObject;
        [SerializeField] private Vector3 camera1;
        [SerializeField] private Vector3 camera2;
        [SerializeField] private Vector3[] spawnPoints;

        [SerializeField] private CharacterSpawner characterSpawnerPrefab;
        [SerializeField] private ScoreManager point;
        [SerializeField] private Timer timer;
        private float _firstScore;
        private float _secondScore;
        private const int FIRST_TIME = 360;
        private const int SECOND_TIME = 120;
        private PlayerDeck _deck;

        [Header("Unitを生成する．")]
        [SerializeField] private bool spawnUnits = true;   // デバッグ用bool値
        [Header("Minionを生成する．")]
        [SerializeField] private bool spawnMinions = false; // デバッグ用bool値

        [Networked] public BattlePhase NowBattleFaze { get; set; } 
        [Networked] private TickTimer Timer { get; set; }

        private byte _minionNum = 6;
        private byte _firstHeroNum = 3;

        public static SpawnDelay Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _firstScore = ScoreManager._goalScore / 4;
            _secondScore = ScoreManager._goalScore / 2;

            StartCoroutine(GameCoroutine());
        }

        IEnumerator GameCoroutine()
        {
            Debug.Log("GameCoroutine開始");
            //yield return null;
            Debug.Log("GameCoroutineテスト");
            if (Object.StateAuthority) NowBattleFaze = BattlePhase.SetUp;
            yield return StartCoroutine(SetUpGame());

            yield return new WaitUntil(() => NowBattleFaze == BattlePhase.Second);
            var units = _deck.MyPlayerCharacterDeck.getSelectedDeck().Unit;
            if (Object.StateAuthority)
            {
                var data = Resources.Load<CharacterDataList>("CharacterDataList");
                foreach (var player in RoomPlayer.Players)
                {
                    var index = RoomPlayer.Players.IndexOf(player);
                    var defaultPoint = spawnPoints[index];
                    if (spawnUnits)
                    {
                        var heroNum = _firstHeroNum;
                        var point = defaultPoint + new Vector3(0, 0, 5f) * heroNum;
                        SpawnCharacterSpawner(point, player, data.characterDataList[units[heroNum]]);
                    }
                }
            }
            
            yield return new WaitUntil(() => NowBattleFaze == BattlePhase.Third);
            if (Object.StateAuthority)
            {
                var data = Resources.Load<CharacterDataList>("CharacterDataList");
                foreach (var player in RoomPlayer.Players)
                {
                    var index = RoomPlayer.Players.IndexOf(player);
                    var defaultPoint = spawnPoints[index];
                    if (spawnUnits)
                    {
                        var heroNum = _firstHeroNum + 1;
                        var point = defaultPoint + new Vector3(0, 0, 5f) * heroNum;
                        SpawnCharacterSpawner(point, player, data.characterDataList[units[heroNum]]);
                    }
                }
            }

            yield return new WaitUntil(() => NowBattleFaze == BattlePhase.End);
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.StateAuthority && NowBattleFaze == BattlePhase.First && 
                (point.Scores.Get(OwnerTypes.Host) >=  _firstScore ||
                 point.Scores.Get(OwnerTypes.Client) >= _firstScore || 
                 timer.Time.Value <= FIRST_TIME))
            {
                NowBattleFaze = BattlePhase.Second;
            }
            if (Object.StateAuthority && NowBattleFaze == BattlePhase.First && 
                (point.Scores.Get(OwnerTypes.Host) >=  _secondScore ||
                 point.Scores.Get(OwnerTypes.Client) >= _secondScore || 
                 timer.Time.Value <= SECOND_TIME))
            {
                NowBattleFaze = BattlePhase.Third;
            }

            if (Object.StateAuthority &&
                (point.Scores.Get(OwnerTypes.Host) >= ScoreManager._goalScore ||
                 point.Scores.Get(OwnerTypes.Client) >= ScoreManager._goalScore ||
                 timer.Time.Value < 0))
            {
                NowBattleFaze = BattlePhase.End;
            }
        }

        IEnumerator SetUpGame()
        {
            //yield return null;
            Timer = TickTimer.CreateFromSeconds(Runner, 5f);
            Debug.Log("今のフェーズ："+ NowBattleFaze);
            SetCamera();
                        
            _deck = GetComponent<PlayerDeck>();
            if (Object.StateAuthority)
            {
                var data = Resources.Load<CharacterDataList>("CharacterDataList");
                var units = _deck.MyPlayerCharacterDeck.getSelectedDeck().Unit;
                foreach (var player in RoomPlayer.Players)
                {
                    var index = RoomPlayer.Players.IndexOf(player);
                    var defaultPoint = spawnPoints[index];
                    if (spawnUnits)
                    {
                        for (var i = 0; i < _firstHeroNum; i++)
                        {
                            var point = defaultPoint + new Vector3(0, 0, 5f) * i;
                            SpawnCharacterSpawner(point, player, data.characterDataList[units[i]]);
                        }
                    }

                    if (spawnMinions)
                    {
                        var point = defaultPoint - new Vector3(Math.Sign(defaultPoint.x) * 10f , 0, 0);
                        for (var i = 0; i < _minionNum; i++)
                        {
                            SpawnCharacterSpawner(point, player, data.characterDataList[6]);
                            point += new Vector3(0, 0, 5f);
                        }
                    }
                }
            }

            yield return new WaitUntil(() => Timer.Expired(Runner));
            if (Object.StateAuthority) NowBattleFaze = BattlePhase.First;
        }

        private void SpawnCharacterSpawner(Vector3 spawn, RoomPlayer player, CharacterData data)
        {
            GameLauncher.Runner.Spawn(
                characterSpawnerPrefab,
                spawn,
                Quaternion.identity,
                player.Object.InputAuthority,
                (runner, obj) =>
                {
                    var characterSpawner = obj.GetComponent<CharacterSpawner>();
                    characterSpawner.Init(data, data.type != UnitType.Minion ? OwnerType.Player : OwnerType.Minion);
                });
        }

        private void SetCamera()
        {
            switch (GameLauncher.Runner.GameMode)
            {
                case GameMode.Host:
                    camaraObject.transform.position = camera1;
                    camaraObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    return;
                case GameMode.Client:
                    camaraObject.transform.position = camera2;
                    camaraObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    return;
                case GameMode.Single:
                case GameMode.Shared:
                case GameMode.Server:
                case GameMode.AutoHostOrClient:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
