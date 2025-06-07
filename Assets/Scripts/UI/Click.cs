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
    //�ΐ�{�^�����������Ƃ�
    public void SelectA()
    {
        //room���̕ύX�𖳌������}�b�`���O���ɂ̂ݕK�v��UI��\��
        _inputField.interactable = false;
        cancelButton.SetActive(true);
        //selectText.text = "Matching";
        /*�R���t���N�g�C��
    GameLauncher.Instance.isMatching = true;
    */

            //�^�C�}�[�̗L��
        timer.SetActive(true);
        Debug.Log("TimerActive");
        var matchingTimerCs = timer.GetComponent<MatchingTimer>();
        matchingTimerCs.startTimer();

    }

    //�}�b�`���O�̃L�����Z���{�^�����������Ƃ�
    public void SelectB()
    {
        //Runner�֌W�̏��������V���b�g�_�E��
        GameLauncher.Runner.Shutdown();


        MatchingTimerManager.instance.resetTimer();
        SceneManager.LoadScene(0);
    }
}
