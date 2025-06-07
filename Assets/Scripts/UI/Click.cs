using UnityEngine;
using TMPro;
using Online;
using UnityEngine.SceneManagement;
using UniRx;
using UnityEngine.UI;

public class Click : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI selectText;
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] GameObject cancelButton;
    [SerializeField] GameObject timer;
    //[SerializeField] private GameObject TitlePanel;


    private void OnEnable()
    {
        if (gameObject.name != "MatchingCancelButton")
        {
            this.GetComponent<Button>().onClick.AddListener(GameLauncher.Instance.StartHostOrClient);
        }
    }

    private void OnDisable()
    {
        if (gameObject.name != "MatchingCancelButton")
        {
            this.GetComponent<Button>().onClick.RemoveListener(GameLauncher.Instance.StartHostOrClient);
        }
    }
    //対戦ボタンを押したとき
    public void SelectA()
    {
        //room名の変更を無効化しつつマッチング中にのみ必要なUIを表示
        _inputField.interactable = false;
        cancelButton.SetActive(true);
        //selectText.text = "Matching";
        /*コンフリクト修正
    GameLauncher.Instance.isMatching = true;
    */

            //タイマーの有効
        timer.SetActive(true);
        Debug.Log("TimerActive");
        var matchingTimerCs = timer.GetComponent<MatchingTimer>();
        matchingTimerCs.startTimer();

    }

    //マッチングのキャンセルボタンを押したとき
    public void SelectB()
    {
        //Runner関係の初期化＆シャットダウン
        GameLauncher.Runner.Shutdown();


        MatchingTimerManager.instance.resetTimer();
        SceneManager.LoadScene(0);
    }
}
