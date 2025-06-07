using Fusion;
using Online;
using Unit;
using UnityEngine;

/*
 * CharacterSpawnerにプレイヤー情報を与えるためにSpawnさせる為のクラスです。
 * 恐らく、α2以降は別のクラスでこの機能を実装することになると思います。
 * 
 * ノカミ
 */

public class SpawnSpawner : NetworkBehaviour
{
    [SerializeField] private Vector3[] spawnPoints;

    [SerializeField] private CharacterSpawner characterSpawnerPrefab;

    private PlayerDeck _deck;

    [Header("Unitを生成する．")]
    [SerializeField] private bool spawnUnits = true;   // デバッグ用bool値
    [Header("Minionを生成する．")]
    [SerializeField] private bool spawnMinions = false; // デバッグ用bool値

    private byte _minionNum = 5;
    
    void Start()
    {
        if (GameLauncher.Runner.GameMode != GameMode.Host) return;
        
        _deck=GetComponent<PlayerDeck>();
        var data = Resources.Load<CharacterDataList>("CharacterDataList");
        var units = _deck.MyPlayerCharacterDeck.getSelectedDeck().Unit;
        foreach (var player in RoomPlayer.Players)
        {
            var index = RoomPlayer.Players.IndexOf(player);
            var point = spawnPoints[index];

            if (spawnUnits)
            {
                Debug.Log("プレイヤー用スポーン処理");
                foreach (var t in units)
                {
                    GameLauncher.Runner.Spawn(
                        characterSpawnerPrefab,
                        point,
                        Quaternion.identity,
                        player.Object.InputAuthority,
                        (runner, obj) =>
                        {
                            var characterSpawner = obj.GetComponent<CharacterSpawner>();
                            characterSpawner.Init(data.characterDataList[t], OwnerType.Player);
                        });
                    
                    point += new Vector3(Random.Range(-10f, 10f), 0,Random.Range(-10f, 10f));
                }
            }

            if (spawnMinions)
            {
                Debug.Log("ミニオン用スポーン処理");
                for (var i = 0; i < _minionNum; i++)
                {
                    GameLauncher.Runner.Spawn(
                        characterSpawnerPrefab,
                        spawnPoints[index] + new Vector3(Random.Range(-10f, 10f), 0,Random.Range(-10f, 10f)) * i,
                        Quaternion.identity,
                        player.Object.InputAuthority,
                        (runner, obj) =>
                        {
                            var characterSpawner = obj.GetComponent<CharacterSpawner>();
                            characterSpawner.Init(data.characterDataList[6], OwnerType.Minion);
                        } 
                    );
                }
            }
        }
    }
}
