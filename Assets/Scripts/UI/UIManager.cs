using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    UIScoreElement element;
    Dictionary<ScoreObject, List<UIScoreElement>> _scoreUi = new Dictionary<ScoreObject, List<UIScoreElement>>();
    [SerializeField] UIScoreElement _prefab;

    // �R���|�[�l���g
    [SerializeField]
    private Text[] statusPanelTexts;

    // �I�u�W�F�N�g
    [SerializeField]
    private GameObject statusPanel;

    /*private void Start()
    {
        scoreManager.scoreArray[(int)Camp.A].Subscribe(e => UpdateScoreSlider(e, Camp.A));  //�E��(A)�̃X�R�A���Ď���UI���X�V����
        scoreManager.scoreArray[(int)Camp.B].Subscribe(e => UpdateScoreSlider(e, Camp.B));   //�E��(B)�̃X�R�A���Ď���UI���X�V����

        timer.second.Subscribe(e => UpdateTimerText());  //�E��(B)�̃X�R�A���Ď���UI���X�V����
    }*/
    private void Awake()
    {
        element = Instantiate(_prefab, transform);  //�X�R�A�o�[�̐���
    }

    public void OnScoreChanged(ScoreObject scoreObject, float score, Camp camp)    //�X�R�A���ς������
    {
        
        if (_scoreUi.ContainsKey(scoreObject) == false)  //scoreObject�Ƃ����L�[�����邩�ǂ���
        {
            return; //������Δ�����
        }
        var list = _scoreUi[scoreObject];
        
        foreach (var item in list)
        {
            item.UpdateScoreSlider(score, camp);
        }
    }


    public void OnPlayerJoined(Fusion.PlayerRef player, ScoreObject scoreObject)
    {
        if (_scoreUi.ContainsKey(scoreObject) == false)
        {
            _scoreUi.Add(scoreObject, new List<UIScoreElement>());
        }
        _scoreUi[scoreObject].Add(element); //�e�v���C���[�ƃX�R�A�o�[��R�Â���
    }

    public void OnPlayerLeft(ScoreObject scoreObject)
    {
        if (_scoreUi.ContainsKey(scoreObject))
        {
            foreach (var item in _scoreUi[scoreObject])
            {
                if (item != null)
                {
                    Destroy(item.gameObject);
                }
            }
            _scoreUi[scoreObject].Clear();
        }
    }

    // �󂯎����Player�̃X�e�[�^�X��\������
    public void showStatusPanel(FlagOnPlayer player)
    {
        if (player == null)
        {
            statusPanel.SetActive(false);
        }
        else
        {
            statusPanel.SetActive(true);
            statusPanelTexts[0].text = player.hp.ToString("000");
            statusPanelTexts[1].text = player.max_hp.ToString("000");
            statusPanelTexts[2].text = player.speed_heal.ToString("000");
            statusPanelTexts[3].text = player.speed_move.ToString("000");
            statusPanelTexts[4].text = player.speed_attack.ToString("000");
            statusPanelTexts[5].text = player.range_hit.ToString("000");
            statusPanelTexts[6].text = player.attack.ToString("000");
            statusPanelTexts[7].text = player.defense.ToString("000");
            statusPanelTexts[8].text = player.dynamic_hitrate.ToString("000");
            statusPanelTexts[9].text = player.static_hitrate.ToString("000");
            statusPanelTexts[10].text = player.range_search.ToString("000");
        }

    }

    private void Update()
    {
       element.UpdateTimerText();
    }
}
