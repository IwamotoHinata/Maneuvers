using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DeckMake;
using Fusion;
using Online;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomPlayer : NetworkBehaviour
{
    public enum EGameState
    {
        Lobby,
        Preparing,
        GameNow
    }

    [SerializeField] private MyDeck myDeck;
    public static readonly List<RoomPlayer> Players = new List<RoomPlayer>();

    public static RoomPlayer Local;
    private PlayerRef _ref;

    [Networked, Capacity(5)] public NetworkArray<int> MyUnits { get; }
    
    [Networked] public int MyLeader { get; set; }
    
    [Networked] public NetworkBool HasWined { get; set; }

    public override void Spawned()
    {
        base.Spawned();

        if (Object.HasInputAuthority)
        {
            Local = this;
            Debug.Log(GameLauncher.Runner.GameMode);
            var deck = myDeck.getSelectedDeck();
            RPC_SetUnitIDs(deck.Unit);
            RPC_SetLeaderID(deck.Leader);
        }
        
        Players.Add(this);
        Debug.Log(Object.InputAuthority);
        
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += despawnAllPlayer;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SetUnitIDs(int[] units)
    {
        for (var i = 0; i < MyUnits.Length; i++)
        {
            MyUnits.Set(i, units[i]);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SetLeaderID(int leader)
    {
        MyLeader = leader;
    }

    public static void RemovePlayer(NetworkRunner runner, PlayerRef player)
    {
        var roomPlayer = Players.FirstOrDefault(x => x.Object.InputAuthority == player);
        if (roomPlayer != null)
        {
            Players.Remove(roomPlayer);
            runner.Despawn(roomPlayer.Object);
        }
    }

    public static void RemovePlayerOnShutdown(NetworkRunner runner)
    {
        var roomPlayer = Players.FirstOrDefault(x => x.Object.InputAuthority == true);
        if (roomPlayer != null)
        {
            Players.Remove(roomPlayer);
            runner.Despawn(roomPlayer.Object);
        }
    }

    public void despawnAllPlayer(Scene nextScene, LoadSceneMode mode)
    {
        if (nextScene.name == "ResultScene" && Object.HasStateAuthority)
        {
            for (int i = 0; i < Players.Count; i++)
            { 
                var roomPlayer = Players[i];
                Players.Remove(Players[i]);
                GameLauncher.Runner.Despawn(roomPlayer.Object);
            }
        }
    }
}
