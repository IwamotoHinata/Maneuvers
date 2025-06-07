using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace Online
{
    public class GameLauncher : MonoBehaviour, INetworkRunnerCallbacks
    {
        public static GameLauncher Instance { get; private set; }
        public static NetworkRunner Runner;
        //[SerializeField] private HostManager _hostManager;
        [SerializeField] private RoomPlayer roomPlayer;
        

        //private LevelManager _levelManager;
        private NetworkSceneManagerDefault _networkSceneManagerDefault;

        //プレイヤー用のディクショナリ
        public Dictionary<NetworkId, NetworkObject> _spawnedCharacters = new Dictionary<NetworkId, NetworkObject>();

        //room名の名前を格納するstring
        public string roomName = "";

        void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            //_levelManager = GetComponent<LevelManager>();
            _networkSceneManagerDefault = GetComponent<NetworkSceneManagerDefault>();
        }

        public void StartHostOrClient() => StartGame(GameMode.AutoHostOrClient);
        
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (!runner.IsServer) return;
            //if (_gameMode == GameMode.Host) runner.Spawn(_hostManager, Vector3.zero, Quaternion.identity);
            runner.Spawn(roomPlayer, Vector3.zero, Quaternion.identity, player);

            // プレイヤーが2人以上になったらシーンに飛ぶ
            if (RoomPlayer.Players.Count >= 2)
            {
                _networkSceneManagerDefault.Runner.SetActiveScene(1);
            }     
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            RoomPlayer.RemovePlayer(runner, player);
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            RoomPlayer.RemovePlayerOnShutdown(runner);
        }

        public void OnConnectedToServer(NetworkRunner runner) { }

        public void OnDisconnectedFromServer(NetworkRunner runner) { }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

        public void OnSceneLoadDone(NetworkRunner runner) { }

        public void OnSceneLoadStart(NetworkRunner runner) { }

        private async void StartGame(GameMode mode)
        {
            if (Runner != null) return;
            
            Runner = gameObject.AddComponent<NetworkRunner>();
            Runner.ProvideInput = true;

            await Runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                //SessionName = roomName,                             
                CustomLobbyName = roomName,                 // Lobby名。ここにstringを入れると、同じロビー名同士でマッチングします。
                SceneManager = _networkSceneManagerDefault,
                PlayerCount = 2,
            }) ;
        }
    }
}
