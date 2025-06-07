using System.Collections;
using System.Collections.Generic;
using Fusion;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using Online;
using System;
using System.Linq;
using DeckMake;
using Unit;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks;

public class ObserveCharacters : NetworkBehaviour
{
    [HideInInspector] public GameObject[] myUiCharacters = new GameObject[5];

    [HideInInspector] public GameObject[] enemyUiCharacters = new GameObject[5];

    //ホストのキャラが死亡、生成した際に_voidStartにあるObserveRemove,ObserveAddを発動
    [CanBeNull] private readonly ReactiveCollection<GameObject> _myCharacters = new ReactiveCollection<GameObject>();
    public IReactiveCollection<GameObject> MyCharacters => _myCharacters;

    [CanBeNull] private readonly ReactiveCollection<GameObject> _enemyCharacters = new ReactiveCollection<GameObject>();
    public IReactiveCollection<GameObject> EnemyCharacters => _enemyCharacters;

    [SerializeField] private HPUIPresenter hpUiPresenter;
    [SerializeField] private SkillUIPresenter skillUiPresenter;

    private int myCount;
    [SerializeField] private CharacterStateImageChange ImagePresenter;

    private PlayerDeck _playerDeck;

    private void Start()
    {
        _playerDeck = FindObjectOfType<PlayerDeck>();
        //ホストのキャラが生成した際にmyUiCharacterに並び替える。Hp,Skillのデータと配列番号を渡す。
        _myCharacters.ObserveAdd()
            .Subscribe(data =>
            {
                var _baseData = data.Value.GetComponent<CharacterBaseData>();
                _baseData.OninitialSetting
                    .Where(x => x)
                    .Subscribe(_ =>
                    {
                        int index = 0;
                        var units = _playerDeck.MyPlayerCharacterDeck.getSelectedDeck().Unit;
                        index = Array.IndexOf(units, (int) _baseData.MyUnitType);
                        myUiCharacters[index] = data.Value;

                        hpUiPresenter.GetMyHPData(index);
                        skillUiPresenter.GetMySkillData(index);
                        ImagePresenter.GetMyStateData(index);
                    });
            })
            .AddTo(this);

        //ホストのキャラが死亡した際にmyUiCharacterに並び替える。Hp,Skillのデータと配列番号を渡す。
        _myCharacters.ObserveRemove()
            .Subscribe(data =>
            {
                var _baseData = data.Value.GetComponent<CharacterBaseData>();
                int index = 0;
                var units = _playerDeck.MyPlayerCharacterDeck.getSelectedDeck().Unit;
                index = Array.IndexOf(units, (int) _baseData.MyUnitType);

                myUiCharacters[index] = null;
            })
            .AddTo(this);

        //クライアントのキャラが生成した際にmyUiCharacterに並び替える。Hp,Skillのデータと配列番号を渡す。
        _enemyCharacters.ObserveAdd()
            .Subscribe(data =>
            {
                var _baseData = data.Value.GetComponent<CharacterBaseData>();
                _baseData.OninitialSetting
                    .Where(x => x)
                    .Subscribe(async _ =>
                    {
                        int index = 0;
                       
                        var units = await RoomPlayerData();
                        index = Array.IndexOf(units, (int) _baseData.MyUnitType);
                        Debug.Log($"units {data.Value} MyUnitType {index},size {enemyUiCharacters.Length}");
                        //indexに負の値が入るときがあるため応急処置
                        if(index < 0)
                        {
                            index = 0;
                        }
                        enemyUiCharacters[index] = data.Value;
                        hpUiPresenter.GetEnemyHPData(index);
                        skillUiPresenter.GetEnemySkillData(index);
                        ImagePresenter.GetEnemyStateData(index);
                    });
            })
            .AddTo(this);

        //クライアントのキャラが死亡した際にmyUiCharacterに並び替える。Hp,Skillのデータと配列番号を渡す。
        _enemyCharacters.ObserveRemove()
            .Subscribe(data =>
            {
                var _baseData = data.Value.GetComponent<CharacterBaseData>();
                int index = 0;
                var units = RoomPlayer.Players[1].MyUnits.ToArray();
                index = Array.IndexOf(units, (int) _baseData.MyUnitType);
                enemyUiCharacters[index] = null;
            }).AddTo(this);
    }

    /// <summary>
    /// クライアントサイドのRoomPlayerの情報を取得する
    /// </summary>
    private async UniTask<int[]> RoomPlayerData()
    {
        await UniTask.WaitUntil(() => RoomPlayer.Players.Count >= 2);
        return RoomPlayer.Players[1].MyUnits.ToArray();
    }
}