using Fusion;
using Online;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : NetworkBehaviour
{
    //コンポーネント
    [SerializeField] private Slider leftSlider;
    [SerializeField] private Slider rightSlider;

    public List<Flag> Flags { get; private set; }  //旗の配列
    [SerializeField] private GameObject flags;

    public Flag[] Flag = new Flag[3];


    [Networked, Capacity(2)]
    public NetworkDictionary<OwnerTypes, float> Scores { get; } = MakeInitializer(new Dictionary<OwnerTypes, float> { { OwnerTypes.Host, 0 }, { OwnerTypes.Client, 0 } });   // スコアのディクショナリ

    private OwnerTypes _myMode;  //自分のゲームモードを格納,敵味方の判定に使用 Host->OwnerTypes.Host  Client->OwnerTypes.Client
    private OwnerTypes _otherMode;   //相手のゲームモードを格納,敵味方の判定に使用 Host->OwnerTypes.Host  Client->OwnerTypes.Client

    private const float _addRate = 0.25f; //加算するスコア率
    public const float _goalScore = 100f; //終了スコア
    [Networked] private TickTimer addInterval { get; set; } //スコア加算の間隔

    public static ScoreManager Instance { get; private set; }


    //旗を多く持つプレイヤーにスコアの加算を行う
    //毎秒呼び出される
    private void addScore()
    {
        var hasNumClient = Flag.Count(e => e.Owner == OwnerTypes.Client);    //所有者がClientの旗数
        var hasNumHost = Flag.Count(e => e.Owner == OwnerTypes.Host);      //所有者がHostの旗数

        if (hasNumClient != hasNumHost)
        {
            OwnerTypes target = (hasNumClient > hasNumHost) ? OwnerTypes.Client : OwnerTypes.Host;  //スコアを加算する対象の決定
            float addScore = Mathf.Abs(hasNumClient - hasNumHost) * _addRate;  //加算するスコアは保持している旗の数の差×0.25

            Scores.Set(target, Scores.Get(target) + addScore); // スコアの加算

            //スコア上限値を超えた場合
            if (Scores.Get(target) >= _goalScore)
            {
                Scores.Set(target, _goalScore);
            }
        }
    }

    public override void FixedUpdateNetwork()
    {
        //1s毎
        if (Runner.IsServer && addInterval.ExpiredOrNotRunning(Runner))
        {
            addScore(); //スコアの加算
            addInterval = TickTimer.CreateFromSeconds(Runner, 1f);
        }

        leftSlider.value = (Scores.Get(_myMode) / _goalScore);
        rightSlider.value = Scores.Get(_otherMode) / _goalScore;
    }

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
        //敵味方の判断のためにホストかクライアントかを格納
        if (GameLauncher.Runner.GameMode == GameMode.Host)
        {
            _myMode = OwnerTypes.Host;
            _otherMode = OwnerTypes.Client;

        }
        else if (GameLauncher.Runner.GameMode == GameMode.Client)
        {
            _myMode = OwnerTypes.Client;
            _otherMode = OwnerTypes.Host;
        }
    }
}