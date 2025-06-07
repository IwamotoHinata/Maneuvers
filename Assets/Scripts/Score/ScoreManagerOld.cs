using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;

public class ScoreManagerOld : NetworkBehaviour
{
    public static ScoreManagerOld Instance;

    private static NetworkSceneManagerDefault _networkSceneManagerDefault;
    [SerializeField] UIManager _uiManager;
    Dictionary<PlayerRef, ScoreObject> _scoreObjects = new Dictionary<PlayerRef, ScoreObject>();
    PlayerRef p1,p2;
    private void Awake()
    {
        _networkSceneManagerDefault = FindObjectOfType<NetworkSceneManagerDefault>();
        Instance = this;
    }

    public void Register(ScoreObject score)
    {
        _scoreObjects.Add(score.Object.InputAuthority, score);
        if (_scoreObjects.Count == 1)
        {    //��l�ڂ�A�`�[��
            _scoreObjects[score.Object.InputAuthority].camp = Camp.A;
            p1 = score.Object.InputAuthority;
        }
        else
        {�@//��l�ڂ�B�`�[��
            _scoreObjects[score.Object.InputAuthority].camp = Camp.B;
            p2 = score.Object.InputAuthority;
        }
        if (_uiManager != null)
        {
            _uiManager.OnPlayerJoined(score.Object.InputAuthority, score);
        }
    }

    public void Unregister(ScoreObject score)
    {
        if (score.Object == null || _scoreObjects.ContainsKey(score.Object.InputAuthority) == false)
        {
            return;
        }
        if (_uiManager != null)
        {
            _uiManager.OnPlayerLeft(score);
        }
        _scoreObjects.Remove(score.Object.InputAuthority);
    }

    public ScoreObject GetScoreObjectFor(PlayerRef player)
    {
        if (_scoreObjects.ContainsKey(player) == false)
        {
            return null;
        }
        return _scoreObjects[player];
    }

    public void OnScoreChanged(ScoreObject scoreObject, float score, Camp camp)
    {
        if (_uiManager != null)
        {
            _uiManager.OnScoreChanged(scoreObject, score, camp);
        }
    }

    public void FinishGame(Camp camp)
    {
        if (camp == Camp.A)
        {
            RoomPlayer.Local.HasWined = FlagOnlineManager._runner.GameMode == GameMode.Host;
        }
        else
        {
            RoomPlayer.Local.HasWined = FlagOnlineManager._runner.GameMode != GameMode.Host;
        }

        _networkSceneManagerDefault.Runner.SetActiveScene(2);
    }
}
