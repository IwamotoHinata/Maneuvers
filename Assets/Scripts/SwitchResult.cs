using UnityEngine;
using UnityEngine.Serialization;

public enum ResultIdentifier
{
    WIN,
    LOSE,
    DRAW,
}

public class SwitchResult : MonoBehaviour
{
    [SerializeField] private GameObject[] switchUI;
    
    void Start()
    {
        var bgm = BGMType.Title;
        switch (SceneChange.ResultIdentifier) {
            case ResultIdentifier.WIN:
                switchUI[0].SetActive(true);
                switchUI[1].SetActive(false);
                switchUI[2].SetActive(false);
                bgm = BGMType.Win;
                break;
            case ResultIdentifier.LOSE:
                switchUI[0].SetActive(false);
                switchUI[1].SetActive(true);
                switchUI[2].SetActive(false);
                bgm = BGMType.Lose;
                break;
            case ResultIdentifier.DRAW:
                switchUI[0].SetActive(false);
                switchUI[1].SetActive(false);
                switchUI[2].SetActive(true);
                bgm = BGMType.Draw;
                break;
        }
        BGMPlayer.Instance.playBGM(bgm);
    }
}

