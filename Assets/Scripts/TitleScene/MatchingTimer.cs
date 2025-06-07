using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// �}�b�`���O�J�n�����Ƃ��̌o�ߎ��Ԃ��v�����邽�߂̃X�N���v�g
/// </summary>
public class MatchingTimer : MonoBehaviour
{
    /*�R���t���N�g
    [SerializeField]
    private MatchingTimerManager _manager;
    [SerializeField]
    private TextMeshProUGUI _timer;

*/
    public MatchingTimerManager _manager;
    public Text _timer;

    private int _time = 0;
    // Start is called before the first frame update
    void Start()
    {
        _manager = GameObject.Find("MatchingTimerManager").GetComponent<MatchingTimerManager>();
        _timer = GameObject.Find("MatchingTimer").GetComponent<Text>();
    }

    public void startTimer()
    {
        StartCoroutine(updateTimer());
    }

    public IEnumerator updateTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            _time++;
            _manager.SetTime(_time / 60, _time % 60);
            string managerMin = _manager._min.ToString("D2");
            string managerSec = _manager._sec.ToString("D2");
            _timer.text = $"�}�b�`���O��... {managerMin }:{managerSec}";
                
        }
    }
}
