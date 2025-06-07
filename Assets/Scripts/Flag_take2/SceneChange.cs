using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using System;
using UniRx;


public class SceneChange : NetworkBehaviour
{
    //Scene WINSCENE;
    //Scene LOSESCENE;
    [SerializeField] private ScoreManager point;
    private static NetworkSceneManagerDefault _networkSceneManagerDefault;
    public static ResultIdentifier ResultIdentifier = ResultIdentifier.DRAW;
    [SerializeField] private Timer timer;

    private void Awake()
    {
        _networkSceneManagerDefault = FindObjectOfType<NetworkSceneManagerDefault>();
        timer
            .Time
            .Where(x => x == 0)
            .Subscribe(_ => sceneManage())
            .AddTo(this);
    }

    private void FixedUpdate()
    {
        if (timer.Time.Value < 600)
        {
            //ホストかクライアントのスコアが目標値を超えたら
            if (point.Scores.Get(OwnerTypes.Host) >= ScoreManager._goalScore ||
                point.Scores.Get(OwnerTypes.Client) >= ScoreManager._goalScore)
            {
                sceneManage();
            }
        }
    }

    //スコアの判定とシーンの変更
    private void sceneManage()
    {
        float hostScore = point.Scores.Get(OwnerTypes.Host);
        float clientScore = point.Scores.Get(OwnerTypes.Client);

        if (hostScore == clientScore) //同点
        {
            ResultIdentifier = ResultIdentifier.DRAW;
        }
        else if (hostScore > clientScore) //ホスト側の勝利
        {
            ResultIdentifier = Runner.GameMode == GameMode.Host ? ResultIdentifier.WIN : ResultIdentifier.LOSE;
        }
        else //クライアント側の勝利
        {
            ResultIdentifier = Runner.GameMode == GameMode.Host ? ResultIdentifier.LOSE : ResultIdentifier.WIN;
        }

        _networkSceneManagerDefault.Runner.SetActiveScene(2);
    }
}