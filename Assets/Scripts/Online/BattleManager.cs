using System;
using Fusion;
using Unit;
using UnityEngine;

namespace Online
{
    public class BattleManager : NetworkBehaviour
    {
        [SerializeField] private GameObject camaraObject;
        [SerializeField] private Vector3 camera1;
        [SerializeField] private Vector3 camera2;
        [SerializeField] private Vector3[] spawnPoints;

        [SerializeField] private CharacterSpawner characterSpawnerPrefab;
        [SerializeField] private LeaderObserver leaderObserver;
        
        [Header("Unitを生成する．")]
        [SerializeField] private bool spawnUnits = true;   // デバッグ用bool値
        [Header("Minionを生成する．")]
        [SerializeField] private bool spawnMinions = false; // デバッグ用bool値
        [Header("Leaderを生成する．")]
        [SerializeField] private bool spawnLeader = false; // デバッグ用bool値

        private byte _minionNum = 6;
        private byte _firstHeroNum = 4;

        public static BattleManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            setCamera();
            BGMPlayer.Instance.playBGM(BGMType.Battle);
            //RPC_SendUnitsData(RoomPlayer.Local.Object.InputAuthority, _deck.MyPlayerCharacterDeck.getSelectedDeck().Unit);

            if (GameLauncher.Runner.GameMode == GameMode.Host)
            {
                var data = Resources.Load<CharacterDataList>("CharacterDataList");
                foreach (var player in RoomPlayer.Players)
                {
                    var index = RoomPlayer.Players.IndexOf(player);
                    var defaultPoint = spawnPoints[index];
                    if (spawnUnits)
                    {
                        var point = defaultPoint;
                        foreach (var unitID in player.MyUnits)
                        {
                            spawnCharacterSpawner(point, player, data.characterDataList[unitID]);
                            point += new Vector3(0, 0, 25f);
                        }
                    }

                    if (spawnMinions)
                    {
                        var point = defaultPoint - new Vector3(Math.Sign(defaultPoint.x) * 10f , 0, 0);
                        for (var i = 0; i < _minionNum; i++)
                        {
                            spawnCharacterSpawner(point, player, data.characterDataList[^1]);
                            point += new Vector3(0, 0, 5f);
                        }
                    }
                    
                    if (spawnLeader)
                    {
                        GameLauncher.Runner.Spawn(
                            leaderObserver,
                            null,
                            null,
                            player.Object.InputAuthority);
                    }
                }
            }
        }

        private void spawnCharacterSpawner(Vector3 spawn, RoomPlayer player, CharacterData data)
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

        private void setCamera()
        {
            switch (GameLauncher.Runner.GameMode)
            {
                case GameMode.Host:
                    camaraObject.transform.position = camera1;
                    camaraObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                    return;
                case GameMode.Client:
                    camaraObject.transform.position = camera2;
                    camaraObject.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
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
