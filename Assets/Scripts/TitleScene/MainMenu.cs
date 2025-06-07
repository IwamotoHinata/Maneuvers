using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.UIElements.Experimental;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] _buttons;
    [SerializeField] private GameObject _backButton;
    [SerializeField] private Vector3[] _buttonPositions;

    [Header("CharacterSprite")]
    [SerializeField] Sprite[] _characters;

    [Header("Story")] 
    [SerializeField] private GameObject _story1;

    [Header("DeckMaking")]
    [SerializeField] private GameObject _deckMaker;

    [Header("Battle")]
    [SerializeField] private GameObject _randomMatch;
    [SerializeField] private GameObject _costomMatch;

    [Header("Tutorial")]
    [SerializeField] private GameObject _lesson1;
    [SerializeField] private GameObject _lesson2;
    [SerializeField] private GameObject _lesson3;

    [Header("���̑�")]
    [SerializeField] private GridTransition _gridTransition;
    [SerializeField] private Animator _transitionAnimation;//�J�ڂ̎��ɗ����BGridTransition.cs�Q��

    private void Start()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            var rect = _buttons[i].GetComponent<RectTransform>();
            _buttonPositions[i] = rect.localPosition;
        }
    }

    /// <summary>
    /// ���C�����j���[�̃L�����o�X���L�����ɂȂ�x�ɉ�ʍ��ɏo��摜��ω�������
    /// </summary>
    private void OnEnable()
    {
        int random = Random.Range(0, _characters.Length);
        var characterImage = GameObject.Find("CharacterImage").GetComponent<Image>();
        characterImage.sprite = _characters[random];
    }

    /// <summary>
    /// �{�^���������ꂽ�Ƃ��ɌĂяo�����֐�
    /// <param name="num">�e�{�^���Ɋ���U���Ă���ԍ��B</param>
    /// </summary>
    public void onClick(int num)
    {

        switch (num)
        {
            case 0://�X�g�[���[�{�^��
                pushStoryButton();
                break;
            case 1://�f�b�L�Ґ��{�^��
                pushDeckButton();
                break;
            case 2://�퓬�{�^��
                pushBattleButton();
                _backButton.SetActive(true);
                break;
            case 3://�`���[�g���A���{�^��
                pushTutorialButton();
                break;
        }

        //�{�^��������̏ꏊ�܂ňړ�
        if (num == 2)
        {

            //�������{�^���ȊO�̃{�^���͏���
            for (int i = 0; i < _buttons.Length; i++)
            {
                if (i == num)
                    continue;
                else
                    _buttons[i].SetActive(false);
            }

            var rect = _buttons[num].GetComponent<RectTransform>();
            rect.DOLocalMove(new Vector3(200, -20, 0), 0.2f).OnStart(() =>
            {
                switch (num)
                {
                    case 0://�X�g�[���[�{�^��
                           //pushStoryButton();
                           //_backButton.SetActive(true);
                        break;
                    case 1://�f�b�L�Ґ��{�^��
                        pushDeckButton();
                        break;
                    case 2://�퓬�{�^��
                        pushBattleButton();
                        break;
                    case 3://�`���[�g���A���{�^��
                           //pushTutorialButton();
                        break;
                }
            });
        }              
    }

    /// <summary>
    /// �o�b�N�{�^�����������Ƃ��ɏ������s���B
    /// </summary>
    public void OnClickBackButton()
    {
        _story1.SetActive(false);
        _randomMatch.SetActive(false);
        _costomMatch.SetActive(false);
        _lesson1.SetActive(false);
        _lesson2.SetActive(false);
        _lesson3.SetActive(false);

        //�{�^���̈ʒu�����Ƃɖ߂�
        for (int i = 0; i < _buttons.Length; i++)
        {
            if (_buttons[i].activeSelf == true)
            {
                var rect = _buttons[i].GetComponent<RectTransform>();
                rect.DOLocalMove(_buttonPositions[i], 0.2f);
            }
            else
            {
                _buttons[i].SetActive(true);
            }
        }

        _backButton.SetActive(false);
    }

    private void pushStoryButton()
    {
        //_story1.SetActive(true);
        pushStory1Button();
    }

    private void pushDeckButton()
    {
        //�f�b�L�Ґ���ʂ��^�C�g���V�[����ɏo��������
        _gridTransition = GameObject.Find("GridTransition").GetComponent<GridTransition>();
        _transitionAnimation = GameObject.Find("GridTransition").GetComponent<Animator>();

        _gridTransition.nextTransition = "Deck";
        _transitionAnimation.SetTrigger("isTransition");
    }

    private void pushBattleButton()
    {
        _randomMatch.SetActive(true);
        _costomMatch.SetActive(true);
    }

    private void pushTutorialButton()
    {
        pushLesson1Button();
    }

    //�X�g�[���[�̃{�^���p�֐�
    public void pushStory1Button()
    {
        _gridTransition = GameObject.Find("GridTransition").GetComponent<GridTransition>();
        _transitionAnimation = GameObject.Find("GridTransition").GetComponent<Animator>();

        _gridTransition.nextTransition = "Sec1";
        _transitionAnimation.SetTrigger("isTransition");
    }


    //�`���[�g���A���̃{�^���p�֐�
    public void pushLesson1Button()
    {
        _gridTransition = GameObject.Find("GridTransition").GetComponent<GridTransition>();
        _transitionAnimation = GameObject.Find("GridTransition").GetComponent<Animator>();

        _gridTransition.nextTransition = "Tutorial1";
        _transitionAnimation.SetTrigger("isTransition");
    }
}
