using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UniRx.Triggers;

namespace DeckMake
{
    public class DeckUIManager : MonoBehaviour
    {
        [Header("===== デバッグ用フラグ =====")]
        [SerializeField] bool _allowDuplication;    //デッキに被りはありかどうか
        [SerializeField] bool _allowChainbuff;      //チェインバフを実装するかどうか
        [SerializeField] bool _allowFunnel;         //ファンネルを実装するかどうか
        [SerializeField] bool _allowStealth;        //ステルスを実装するかどうか

        [Header("===== ここからゲーム内情報 =====")]
        [Header("選択パネルに関する項目")]
        [SerializeField] GameObject _leaderSelectPanel;
        [SerializeField] GameObject _heroSelectPanel;

        [Header("スキルパネルに関する項目")]
        [SerializeField] Image _leaderSkillExplanationImage;
        [SerializeField] Image _heroSkillExplanationImage;
        [SerializeField] GameObject _heroSkillSelectPanel;
        

        [Header("デッキパネルを変更する項目")]
        [SerializeField] Button _leftButton;
        [SerializeField] Button _rightButton;

        [Header("選択中のリーダー・ヒーローの設定項目")]
        [SerializeField] GameObject _currentLeaderIcon;
        [SerializeField] Button _currentLeaderSkill;
        [SerializeField] GameObject[] _currentHeroIcons;
        [SerializeField] Button[] _currentHeroSkills;

        [Header("選択パネルのリーダー・ヒーローの設定項目")]
        [SerializeField] Transform _leaderChoices;
        [SerializeField] Transform _heroChoices;

        [Header("各パラメータの最大値")]
        [SerializeField] float[] _maxValues;


        [Header("その他の項目")]
        [SerializeField] TextMeshProUGUI deckNamTextMeshPro;
        [SerializeField] Button[] _closePanelButtons;  //クリックするとパネルが閉じるボタン
        [SerializeField] Button _startButton;
        [SerializeField] Button _backMainMenuButton;//メインメニューに戻るためのボタン。startButtonと違い、いつでもメインメニューに戻れる
        [SerializeField] MyDeck _myDeck;
        [SerializeField] Unit.CharacterDataList _characterDataList;
        [SerializeField] DeckDataManager _deckDataManager;

        private int _currentDeckId = 0;  //現在編集中のデッキ番号
        private int _heroEditIndex = 0;  //選択パネルを開いた時の編集中のヒーローの番号
        private int _currentSkillId = 0; //選択中のスキル番号
        ReactiveProperty<int> _leaderPoint = new ReactiveProperty<int>(0);
        ReactiveProperty<int> _sumCost = new ReactiveProperty<int>(0);


        void Start()
        {

            uiInit();

            _leaderPoint.Value = _myDeck.getLeaderPoint(_currentDeckId);
            _sumCost.Value = _myDeck.getSumCost(_currentDeckId);

            updateDeckPanel();

            Debug.Log("Start;");
        }

        /*  UIの初期化処理    */
        private void uiInit()
        {
            //デッキ変更のボタン設定
            _leftButton.onClick.AsObservable()
                              .Subscribe(_ => CarouselChange(-1))
                              .AddTo(this);
            _rightButton.onClick.AsObservable()
                               .Subscribe(_ => CarouselChange(1))
                               .AddTo(this);

            _startButton.onClick.AsObservable()
                              .Subscribe(_ => returnMainMenu())
                              .AddTo(this);

            _backMainMenuButton.onClick.AsObservable()
                              .Subscribe(_ => returnMainMenu())
                              .AddTo(this);

            //選択パネル外をクリックした場合、パネルを閉じるように
            foreach (var b in _closePanelButtons)
            {
                b.onClick.AsObservable().Subscribe(_ => closeOpendPanel());
            }

            //選択中のデッキのアイコンボタン設定（選択パネルとスキルパネル）
            var leaderBtn = _currentLeaderIcon.GetComponent<Button>();
            leaderBtn.onClick.AsObservable()
                             .Subscribe(_ => openLeaderSelectPanel())
                             .AddTo(this);

            //リーダーのスキルパネルのマウスオーバーイベント
            var leaderEventTrigger = _currentLeaderSkill.gameObject.AddComponent<ObservableEventTrigger>();
            leaderEventTrigger.OnPointerEnterAsObservable()
                        .Subscribe(_ => StartCoroutine(openLeaderSkillPanel()))
                        .AddTo(this);
            leaderEventTrigger.OnPointerExitAsObservable()
                        .Subscribe(_ => closeOpendPanel())
                        .AddTo(this);


            for (int i = 0; i < _currentHeroIcons.Length; i++)
            {
                int bId = i;
                var btn = _currentHeroIcons[bId].GetComponent<Button>();
                btn.onClick.AsObservable()
                           .Subscribe(_ => openHeroSelectPanel(bId))
                           .AddTo(this);
                //ヒーロースキルアイコンのクリックイベント（スキル選択のあるユニットのみ）
                _currentHeroSkills[bId].onClick.AsObservable()
                                            .Subscribe(_ => StartCoroutine(openHeroSkillSelectPanel(bId)))
                                            .AddTo(this);
                //ヒーローのスキルパネルのマウスオーバーイベント
                var heroEventTrigger = _currentHeroSkills[bId].gameObject.AddComponent<ObservableEventTrigger>();
                heroEventTrigger.OnPointerEnterAsObservable()
                            .Subscribe(_ => StartCoroutine(openHeroSkillPanel(bId)))
                            .AddTo(this);
                heroEventTrigger.OnPointerExitAsObservable()
                            .Subscribe(_ => closeHeroSkillPanel())
                            .AddTo(this);
            }

            //選択パネルのキャラボタンの設定
            for (int i = 0; i < _leaderChoices.childCount; i++)
            {
                int btnIndex = i;   //イテレータを一度変数に入れないとうまく動作しない
                var btn = _leaderChoices.GetChild(btnIndex).GetComponent<Button>();
                btn.onClick.AsObservable()
                           .Subscribe(_ => changeLeader(btnIndex))
                           .AddTo(this);
            }
            for (int i = 0; i < _heroChoices.childCount; i++)
            {
                int btnIndex = i;   //イテレータを一度変数に入れないとうまく動作しない
                var btn = _heroChoices.GetChild(btnIndex).GetComponent<Button>();
                btn.onClick.AsObservable()
                           .Subscribe(_ => changeHero(btnIndex))
                           .AddTo(this);
            }
        }

        /// <summary>
        /// デッキ編成画面からメインメニューに戻るための関数
        /// </summary>
        private void returnMainMenu()
        {
            GameObject.Find("DeckMaker").SetActive(false);
            GameObject.Find("MatchingCanvas").transform.Find("MainMenuPanel").gameObject.SetActive(true);//メインメニュ用のパネルを表示
        }



        /*  表示デッキの切り替え  */
        private void CarouselChange(int mode)
        {
            if (mode > 0)
            {
                _currentDeckId = (_currentDeckId + 1) % 10;
            }
            else
            {
                _currentDeckId = (_currentDeckId + 9) % 10;
            }
            updateDeckPanel();
        }


        /*  リーダーの選択パネルを開く   */
        private void openLeaderSelectPanel()
        {

            _leaderSelectPanel.SetActive(true);
        }


        /*  ヒーローの選択パネルを開く buttonId:クリックしたボタンの番号 */
        private void openHeroSelectPanel(int buttonId)
        {
            _heroSelectPanel.SetActive(true);
            
            _heroEditIndex = buttonId;
        }


        /*  開いている選択パネルを閉じる  */
        private void closeOpendPanel()
        {
            
            _leaderSelectPanel.SetActive(false);
            _heroSelectPanel.SetActive(false);

            _heroSkillSelectPanel.GetComponent<CanvasGroup>().alpha = 0;
            _leaderSkillExplanationImage.enabled = false;
            _heroSkillExplanationImage.enabled = false;
            _heroSkillSelectPanel.SetActive(false);


            _deckDataManager.Save();
        }

        /* ヒーローのスキルの選択パネルを閉じる */
        private void closeHeroSkillPanel()
        {
            _heroSkillExplanationImage.enabled = false;
        }

        /*  リーダーのスキルパネルを開く */
        private IEnumerator openLeaderSkillPanel()
        {
            //説明画像の取得
            var skillId = _myDeck.getDeckOf(_currentDeckId).Leader;
            _leaderSkillExplanationImage.sprite = _myDeck.leaderSkillExplanationSprite[skillId];


            _leaderSkillExplanationImage.enabled = true;
            var alpha = 0.0f;
            while (alpha < 1.0f){
                alpha += Time.deltaTime*3;
                _leaderSkillExplanationImage.color = new Color(1, 1, 1, alpha);
                yield return new WaitForEndOfFrame();
            }
            yield break; 
        }

        /*  ヒーローのスキルパネルを開く　buttonId:クリックしたボタンの番号 */
        private IEnumerator openHeroSkillPanel(int buttonId)
        {
            //説明画像の取得
            var skillId = _myDeck.getDeckOf(_currentDeckId).Unit[buttonId];
            _heroSkillExplanationImage.sprite = _myDeck.heroSkillExplanationSprite[skillId]; 


            _heroSkillExplanationImage.enabled = true;
            var alpha = 0.0f;
            while (alpha < 1.0f)
            {
                alpha += Time.deltaTime * 3;
                _heroSkillExplanationImage.color = new Color(1, 1, 1, alpha);
                yield return new WaitForEndOfFrame();
            }
            yield break;
        }


        /*  ヒーローのスキル選択パネルを開く buttonId:クリックしたボタンの番号    */
        private IEnumerator openHeroSkillSelectPanel(int buttonId)
        {
            _heroEditIndex = buttonId;
            var heroId = _myDeck.getDeckOf(_currentDeckId).Unit[buttonId];
            var isOpen = true;  //選択ウィンドウを表示するかどうか

            //スキル選択の必要なユニットなら
            var skillButton_0 = _heroSkillSelectPanel.transform.GetChild(2);
            var skillButton_1 = _heroSkillSelectPanel.transform.GetChild(3);
            if (_allowFunnel && ( heroId == (int)Unit.UnitType.Recon || heroId == (int)Unit.UnitType.Funnnel))     //リコン
            {
                int recon = (int)Unit.UnitType.Recon;
                int funnel = (int)Unit.UnitType.Funnnel;

                skillButton_0.GetChild(0).gameObject.GetComponent<Image>().sprite = _myDeck.heroSkillIconSprite[recon];
                skillButton_0.GetChild(1).gameObject.GetComponent<Text>().text = _myDeck.heroSkillName[recon];

                skillButton_1.GetChild(0).gameObject.GetComponent<Image>().sprite = _myDeck.heroSkillIconSprite[funnel];
                skillButton_1.GetChild(1).gameObject.GetComponent<Text>().text = _myDeck.heroSkillName[funnel];
            }
            else if(_allowChainbuff && (heroId == (int)Unit.UnitType.Tank || heroId == (int)Unit.UnitType.Chainbuff))    //タンク
            {
                int tank = (int)Unit.UnitType.Tank;
                int chainbuff = (int)Unit.UnitType.Chainbuff;

                skillButton_0.GetChild(0).gameObject.GetComponent<Image>().sprite = _myDeck.heroSkillIconSprite[tank];
                skillButton_0.GetChild(1).gameObject.GetComponent<Text>().text = _myDeck.heroSkillName[tank];

                skillButton_1.GetChild(0).gameObject.GetComponent<Image>().sprite = _myDeck.heroSkillIconSprite[chainbuff];
                skillButton_1.GetChild(1).gameObject.GetComponent<Text>().text = _myDeck.heroSkillName[chainbuff];
            }
            else if (_allowStealth && (heroId == (int)Unit.UnitType.Assault || heroId == (int)Unit.UnitType.Stealth))    //突撃兵
            {
                int assault = (int)Unit.UnitType.Assault;
                int stealth = (int)Unit.UnitType.Stealth;

                skillButton_0.GetChild(0).gameObject.GetComponent<Image>().sprite = _myDeck.heroSkillIconSprite[assault];
                skillButton_0.GetChild(1).gameObject.GetComponent<Text>().text = _myDeck.heroSkillName[assault];

                skillButton_1.GetChild(0).gameObject.GetComponent<Image>().sprite = _myDeck.heroSkillIconSprite[stealth];
                skillButton_1.GetChild(1).gameObject.GetComponent<Text>().text = _myDeck.heroSkillName[stealth];
            }
            else
            {
                isOpen = false;
            }

            if (isOpen)
            {
                _heroSkillSelectPanel.SetActive(true);
                var cg = _heroSkillSelectPanel.GetComponent<CanvasGroup>();
                while (cg.alpha < 1.0f)
                {
                    cg.alpha += Time.deltaTime * 3;
                    yield return new WaitForEndOfFrame();
                }
            }
            
            yield break;
        }

        /*  リーダーの変更を行う    */
        private void changeLeader(int newLeaderIndex = 0)
        {
            _myDeck.getDeckOf(_currentDeckId).Leader = newLeaderIndex;
            _leaderPoint.Value = _myDeck.getLeaderPoint(_currentDeckId);
            updateDeckPanel();
            closeOpendPanel();
        }

        /*  
         *  ヒーローの変更を行う 
         *  switchで無理やりスキルを2つもつユニットに対応させてます
         */
        private void changeHero(int newHeroIndex)
        {
            int heroId = convertToSkillFromHero(newHeroIndex);   //ヒーロー番号からスキル番号に変換

            _myDeck.getDeckOf(_currentDeckId).Unit[_heroEditIndex] = heroId;
            _sumCost.Value = _myDeck.getSumCost(_currentDeckId);
            updateDeckPanel();
            closeOpendPanel();
        }

        /*  デッキパネルの更新を行う    */
        private void updateDeckPanel()
        {
            var list = _myDeck.getDeckOf(_currentDeckId);

            //デッキ名の変更
            deckNamTextMeshPro.SetText(list.DeckName); 

            //コスト
            _sumCost.Value = _myDeck.getSumCost(_currentDeckId);
            _leaderPoint.Value = _myDeck.getLeaderPoint(_currentDeckId);

            //リーダー
            var leader = list.Leader;
            if(leader >= 0)  //空でないなら
            {
                _currentLeaderIcon.GetComponent<Image>().sprite = _myDeck.leaderIconSprite[leader]; //cardIcon
                _currentLeaderSkill.transform.GetChild(0).GetComponent<Image>().sprite = _myDeck.leaderSkillIconSprite[leader];    //Icon
                _currentLeaderSkill.transform.GetChild(1).GetComponent<Text>().text = _myDeck.leaderSkillName[leader];   //Text
                int fontSize = _myDeck.leaderSkillName[leader].Length >= 6 ? 24 : 32;
                _currentLeaderSkill.transform.GetChild(1).GetComponent<Text>().fontSize = fontSize;
            }
            else
            {
                _currentLeaderIcon.GetComponent<Image>().sprite = null;
                _currentLeaderSkill.transform.GetChild(0).GetComponent<Image>().sprite = null;    //Icon
                _currentLeaderSkill.transform.GetChild(1).GetComponent<Text>().text = "";   //Text
            }

            //ヒーロー
            for (int i = 0; i < list.Unit.Length; i++)
            {
                var hero = list.Unit[i];
                if(hero >= 0)    //空でないなら
                {
                    _currentHeroIcons[i].GetComponent<Image>().sprite = _myDeck.heroIconSprites[hero];  //cardIcon
                    _currentHeroSkills[i].transform.GetChild(0).GetComponent<Image>().sprite = _myDeck.heroSkillIconSprite[hero];    //Icon
                    _currentHeroSkills[i].transform.GetChild(1).GetComponent<Text>().text = _myDeck.heroSkillName[hero];   //Text
                    int fontSize = (_myDeck.heroSkillName[hero].Length >= 6) ? 15 : 25;
                    _currentHeroSkills[i].transform.GetChild(1).GetComponent<Text>().fontSize = fontSize;

                    var statusPanel = _currentHeroIcons[i].transform.GetChild(0);  //ステータスパネルを取得
                    //各パラメータの数値を取得
                    float[] values = new float[5];
                    var characterDataList = getCharaDataList(hero);
                    values[0] = characterDataList.hp;             //HP
                    values[1] = characterDataList.attackPower;    //Power
                    values[2] = characterDataList.moveSpeed;      //Speed
                    values[3] = characterDataList.attackRange;    //Attack Range
                    values[4] = characterDataList.searchRange;    //Search Range
                    //各パラメータのゲージを更新
                    for (int gageItr = 0; gageItr < 5; gageItr++)
                    {
                        var fillImage = statusPanel.GetChild(gageItr).GetChild(1).gameObject.GetComponent<Image>();
                        fillImage.fillAmount = Mathf.Clamp(values[gageItr], 0, _maxValues[gageItr]) / _maxValues[gageItr];
                    }
 
                }
                else
                {
                    _currentHeroIcons[i].GetComponent<Image>().sprite = null;
                    _currentHeroSkills[i].transform.GetChild(0).GetComponent<Image>().sprite = null;    //Icon
                    _currentHeroSkills[i].transform.GetChild(1).GetComponent<Text>().text = "";   //Text

                    var heroCostText = _currentHeroIcons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();   //コストのテキストを取得
                    heroCostText.text = "0";   //コストのテキストを変更

                    var statusPanel = _currentHeroIcons[i].transform.GetChild(1);  //ステータスパネルを取得

                    //各パラメータのゲージを更新
                    for (int gageItr = 0; gageItr < 5; gageItr++)
                    {
                        var fillImage = statusPanel.GetChild(gageItr).GetChild(1).GetComponent<Image>();
                        fillImage.fillAmount = 0;
                    }
                }

            }

            //コストを判定とスロットが空でないか確認をしボタンの状態を反映
            var checkDuplication = _allowDuplication ? true : !_myDeck.isDuplicationDeckOf(_currentDeckId); //重複を許すならtrue, 重複を許さないなら重複がないか確認
            if (/*_leaderPoint.Value < _sumCost.Value ||*/ !_myDeck.isAllSetDeckOf(_currentDeckId) || !checkDuplication)
            {
                _startButton.interactable = false;
            }
            else
            {
                _startButton.interactable = true;
            }
        }

        //スキル選択ボタン
        public void OnCloseHeroSkillSelectPanel(int id)
        {
            _currentSkillId = id;

            var skillId = _myDeck.getDeckOf(_currentDeckId).Unit[_heroEditIndex];
            var heroId = _myDeck.convertToHeroFromSkill(skillId);
            changeHero(heroId);
            
            closeOpendPanel();

            _currentSkillId = 0;    //0に戻す
        }



        //スキル選択番号（_currentSkillId）とユニット番号(0～5）からスキル番号（0～8）を求める
        private int convertToSkillFromHero(int heroId)
        {
            int skillId = 0;
            switch (heroId)
            {
                case 0: //基本兵
                    skillId = (int)Unit.UnitType.Basic;
                    break;
                case 1: //スナイパー
                    skillId = (int)Unit.UnitType.Sniper;
                    break;
                case 2: //リコン
                    if (_currentSkillId == 0) skillId = (int)Unit.UnitType.Recon;
                    else skillId = (int)Unit.UnitType.Funnnel;
                    break;
                case 3: //タンク
                    if (_currentSkillId == 0) skillId = (int)Unit.UnitType.Tank;
                    else skillId = (int)Unit.UnitType.Chainbuff;
                    break;
                case 4: //デバッファー
                    skillId = (int)Unit.UnitType.Debuffer;
                    break;
                case 5: //突撃兵
                    if (_currentSkillId == 0) skillId = (int)Unit.UnitType.Assault;
                    else skillId = (int)Unit.UnitType.Stealth;
                    break;
                default:
                    skillId = -1;
                    break;
            }
            return skillId;
        }
        
        //スキル番号からキャラのステータスを取得する
        private Unit.CharacterData getCharaDataList(int skillId)
        {
            int heroId = 0; //CharacterDataListのインデックスにあわせる
            switch (skillId)
            {
                case (int)Unit.UnitType.Basic: //基本兵
                    heroId = 0;
                    break;
                case (int)Unit.UnitType.Sniper: //スナイパー
                    heroId = 4;
                    break;
                case (int)Unit.UnitType.Recon:  //リコン
                    heroId = 2;
                    break;
                case (int)Unit.UnitType.Funnnel: //ファンネル
                    heroId = 7;
                    break;
                case (int)Unit.UnitType.Tank:   //タンク
                    heroId = 5;
                    break;
                case (int)Unit.UnitType.Chainbuff: //チェインバフ（ない！！！！）
                    heroId = 5;
                    Debug.Log("取得失敗");
                    break;
                case (int)Unit.UnitType.Debuffer: //デバッファー
                    heroId = 3;
                    break;
                case (int)Unit.UnitType.Assault:    //アサルト
                    heroId = 1;
                    break;
                case (int)Unit.UnitType.Stealth: //ステルス(ない！！！)
                    heroId = 1;
                    Debug.Log("取得失敗");
                    break;
                default:
                    break;
            }
            
            return _characterDataList.characterDataList[heroId];
        }
    }

}