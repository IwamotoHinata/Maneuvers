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

    [Header("その他")]
    [SerializeField] private GridTransition _gridTransition;
    [SerializeField] private Animator _transitionAnimation;//遷移の時に流す。GridTransition.cs参照

    private void Start()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            var rect = _buttons[i].GetComponent<RectTransform>();
            _buttonPositions[i] = rect.localPosition;
        }
    }

    /// <summary>
    /// メインメニューのキャンバスが有効化になる度に画面左に出る画像を変化させる
    /// </summary>
    private void OnEnable()
    {
        int random = Random.Range(0, _characters.Length);
        var characterImage = GameObject.Find("CharacterImage").GetComponent<Image>();
        characterImage.sprite = _characters[random];
    }

    /// <summary>
    /// ボタンが押されたときに呼び出される関数
    /// <param name="num">各ボタンに割り振っている番号。</param>
    /// </summary>
    public void onClick(int num)
    {

        switch (num)
        {
            case 0://ストーリーボタン
                pushStoryButton();
                break;
            case 1://デッキ編成ボタン
                pushDeckButton();
                break;
            case 2://戦闘ボタン
                pushBattleButton();
                _backButton.SetActive(true);
                break;
            case 3://チュートリアルボタン
                pushTutorialButton();
                break;
        }

        //ボタンを所定の場所まで移動
        if (num == 2)
        {

            //押したボタン以外のボタンは消す
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
                    case 0://ストーリーボタン
                           //pushStoryButton();
                           //_backButton.SetActive(true);
                        break;
                    case 1://デッキ編成ボタン
                        pushDeckButton();
                        break;
                    case 2://戦闘ボタン
                        pushBattleButton();
                        break;
                    case 3://チュートリアルボタン
                           //pushTutorialButton();
                        break;
                }
            });
        }              
    }

    /// <summary>
    /// バックボタンを押したときに処理を行う。
    /// </summary>
    public void OnClickBackButton()
    {
        _story1.SetActive(false);
        _randomMatch.SetActive(false);
        _costomMatch.SetActive(false);
        _lesson1.SetActive(false);
        _lesson2.SetActive(false);
        _lesson3.SetActive(false);

        //ボタンの位置をもとに戻す
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
        //デッキ編成画面をタイトルシーン上に出現させる
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

    //ストーリーのボタン用関数
    public void pushStory1Button()
    {
        _gridTransition = GameObject.Find("GridTransition").GetComponent<GridTransition>();
        _transitionAnimation = GameObject.Find("GridTransition").GetComponent<Animator>();

        _gridTransition.nextTransition = "Sec1";
        _transitionAnimation.SetTrigger("isTransition");
    }


    //チュートリアルのボタン用関数
    public void pushLesson1Button()
    {
        _gridTransition = GameObject.Find("GridTransition").GetComponent<GridTransition>();
        _transitionAnimation = GameObject.Find("GridTransition").GetComponent<Animator>();

        _gridTransition.nextTransition = "Tutorial1";
        _transitionAnimation.SetTrigger("isTransition");
    }
}
