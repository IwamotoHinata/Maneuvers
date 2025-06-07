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
        [Header("===== �f�o�b�O�p�t���O =====")]
        [SerializeField] bool _allowDuplication;    //�f�b�L�ɔ��͂��肩�ǂ���
        [SerializeField] bool _allowChainbuff;      //�`�F�C���o�t���������邩�ǂ���
        [SerializeField] bool _allowFunnel;         //�t�@���l�����������邩�ǂ���
        [SerializeField] bool _allowStealth;        //�X�e���X���������邩�ǂ���

        [Header("===== ��������Q�[������� =====")]
        [Header("�I���p�l���Ɋւ��鍀��")]
        [SerializeField] GameObject _leaderSelectPanel;
        [SerializeField] GameObject _heroSelectPanel;

        [Header("�X�L���p�l���Ɋւ��鍀��")]
        [SerializeField] Image _leaderSkillExplanationImage;
        [SerializeField] Image _heroSkillExplanationImage;
        [SerializeField] GameObject _heroSkillSelectPanel;
        

        [Header("�f�b�L�p�l����ύX���鍀��")]
        [SerializeField] Button _leftButton;
        [SerializeField] Button _rightButton;

        [Header("�I�𒆂̃��[�_�[�E�q�[���[�̐ݒ荀��")]
        [SerializeField] GameObject _currentLeaderIcon;
        [SerializeField] Button _currentLeaderSkill;
        [SerializeField] GameObject[] _currentHeroIcons;
        [SerializeField] Button[] _currentHeroSkills;

        [Header("�I���p�l���̃��[�_�[�E�q�[���[�̐ݒ荀��")]
        [SerializeField] Transform _leaderChoices;
        [SerializeField] Transform _heroChoices;

        [Header("�e�p�����[�^�̍ő�l")]
        [SerializeField] float[] _maxValues;


        [Header("���̑��̍���")]
        [SerializeField] TextMeshProUGUI deckNamTextMeshPro;
        [SerializeField] Button[] _closePanelButtons;  //�N���b�N����ƃp�l��������{�^��
        [SerializeField] Button _startButton;
        [SerializeField] Button _backMainMenuButton;//���C�����j���[�ɖ߂邽�߂̃{�^���BstartButton�ƈႢ�A���ł����C�����j���[�ɖ߂��
        [SerializeField] MyDeck _myDeck;
        [SerializeField] Unit.CharacterDataList _characterDataList;
        [SerializeField] DeckDataManager _deckDataManager;

        private int _currentDeckId = 0;  //���ݕҏW���̃f�b�L�ԍ�
        private int _heroEditIndex = 0;  //�I���p�l�����J�������̕ҏW���̃q�[���[�̔ԍ�
        private int _currentSkillId = 0; //�I�𒆂̃X�L���ԍ�
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

        /*  UI�̏���������    */
        private void uiInit()
        {
            //�f�b�L�ύX�̃{�^���ݒ�
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

            //�I���p�l���O���N���b�N�����ꍇ�A�p�l�������悤��
            foreach (var b in _closePanelButtons)
            {
                b.onClick.AsObservable().Subscribe(_ => closeOpendPanel());
            }

            //�I�𒆂̃f�b�L�̃A�C�R���{�^���ݒ�i�I���p�l���ƃX�L���p�l���j
            var leaderBtn = _currentLeaderIcon.GetComponent<Button>();
            leaderBtn.onClick.AsObservable()
                             .Subscribe(_ => openLeaderSelectPanel())
                             .AddTo(this);

            //���[�_�[�̃X�L���p�l���̃}�E�X�I�[�o�[�C�x���g
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
                //�q�[���[�X�L���A�C�R���̃N���b�N�C�x���g�i�X�L���I���̂��郆�j�b�g�̂݁j
                _currentHeroSkills[bId].onClick.AsObservable()
                                            .Subscribe(_ => StartCoroutine(openHeroSkillSelectPanel(bId)))
                                            .AddTo(this);
                //�q�[���[�̃X�L���p�l���̃}�E�X�I�[�o�[�C�x���g
                var heroEventTrigger = _currentHeroSkills[bId].gameObject.AddComponent<ObservableEventTrigger>();
                heroEventTrigger.OnPointerEnterAsObservable()
                            .Subscribe(_ => StartCoroutine(openHeroSkillPanel(bId)))
                            .AddTo(this);
                heroEventTrigger.OnPointerExitAsObservable()
                            .Subscribe(_ => closeHeroSkillPanel())
                            .AddTo(this);
            }

            //�I���p�l���̃L�����{�^���̐ݒ�
            for (int i = 0; i < _leaderChoices.childCount; i++)
            {
                int btnIndex = i;   //�C�e���[�^����x�ϐ��ɓ���Ȃ��Ƃ��܂����삵�Ȃ�
                var btn = _leaderChoices.GetChild(btnIndex).GetComponent<Button>();
                btn.onClick.AsObservable()
                           .Subscribe(_ => changeLeader(btnIndex))
                           .AddTo(this);
            }
            for (int i = 0; i < _heroChoices.childCount; i++)
            {
                int btnIndex = i;   //�C�e���[�^����x�ϐ��ɓ���Ȃ��Ƃ��܂����삵�Ȃ�
                var btn = _heroChoices.GetChild(btnIndex).GetComponent<Button>();
                btn.onClick.AsObservable()
                           .Subscribe(_ => changeHero(btnIndex))
                           .AddTo(this);
            }
        }

        /// <summary>
        /// �f�b�L�Ґ���ʂ��烁�C�����j���[�ɖ߂邽�߂̊֐�
        /// </summary>
        private void returnMainMenu()
        {
            GameObject.Find("DeckMaker").SetActive(false);
            GameObject.Find("MatchingCanvas").transform.Find("MainMenuPanel").gameObject.SetActive(true);//���C�����j���p�̃p�l����\��
        }



        /*  �\���f�b�L�̐؂�ւ�  */
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


        /*  ���[�_�[�̑I���p�l�����J��   */
        private void openLeaderSelectPanel()
        {

            _leaderSelectPanel.SetActive(true);
        }


        /*  �q�[���[�̑I���p�l�����J�� buttonId:�N���b�N�����{�^���̔ԍ� */
        private void openHeroSelectPanel(int buttonId)
        {
            _heroSelectPanel.SetActive(true);
            
            _heroEditIndex = buttonId;
        }


        /*  �J���Ă���I���p�l�������  */
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

        /* �q�[���[�̃X�L���̑I���p�l������� */
        private void closeHeroSkillPanel()
        {
            _heroSkillExplanationImage.enabled = false;
        }

        /*  ���[�_�[�̃X�L���p�l�����J�� */
        private IEnumerator openLeaderSkillPanel()
        {
            //�����摜�̎擾
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

        /*  �q�[���[�̃X�L���p�l�����J���@buttonId:�N���b�N�����{�^���̔ԍ� */
        private IEnumerator openHeroSkillPanel(int buttonId)
        {
            //�����摜�̎擾
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


        /*  �q�[���[�̃X�L���I���p�l�����J�� buttonId:�N���b�N�����{�^���̔ԍ�    */
        private IEnumerator openHeroSkillSelectPanel(int buttonId)
        {
            _heroEditIndex = buttonId;
            var heroId = _myDeck.getDeckOf(_currentDeckId).Unit[buttonId];
            var isOpen = true;  //�I���E�B���h�E��\�����邩�ǂ���

            //�X�L���I���̕K�v�ȃ��j�b�g�Ȃ�
            var skillButton_0 = _heroSkillSelectPanel.transform.GetChild(2);
            var skillButton_1 = _heroSkillSelectPanel.transform.GetChild(3);
            if (_allowFunnel && ( heroId == (int)Unit.UnitType.Recon || heroId == (int)Unit.UnitType.Funnnel))     //���R��
            {
                int recon = (int)Unit.UnitType.Recon;
                int funnel = (int)Unit.UnitType.Funnnel;

                skillButton_0.GetChild(0).gameObject.GetComponent<Image>().sprite = _myDeck.heroSkillIconSprite[recon];
                skillButton_0.GetChild(1).gameObject.GetComponent<Text>().text = _myDeck.heroSkillName[recon];

                skillButton_1.GetChild(0).gameObject.GetComponent<Image>().sprite = _myDeck.heroSkillIconSprite[funnel];
                skillButton_1.GetChild(1).gameObject.GetComponent<Text>().text = _myDeck.heroSkillName[funnel];
            }
            else if(_allowChainbuff && (heroId == (int)Unit.UnitType.Tank || heroId == (int)Unit.UnitType.Chainbuff))    //�^���N
            {
                int tank = (int)Unit.UnitType.Tank;
                int chainbuff = (int)Unit.UnitType.Chainbuff;

                skillButton_0.GetChild(0).gameObject.GetComponent<Image>().sprite = _myDeck.heroSkillIconSprite[tank];
                skillButton_0.GetChild(1).gameObject.GetComponent<Text>().text = _myDeck.heroSkillName[tank];

                skillButton_1.GetChild(0).gameObject.GetComponent<Image>().sprite = _myDeck.heroSkillIconSprite[chainbuff];
                skillButton_1.GetChild(1).gameObject.GetComponent<Text>().text = _myDeck.heroSkillName[chainbuff];
            }
            else if (_allowStealth && (heroId == (int)Unit.UnitType.Assault || heroId == (int)Unit.UnitType.Stealth))    //�ˌ���
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

        /*  ���[�_�[�̕ύX���s��    */
        private void changeLeader(int newLeaderIndex = 0)
        {
            _myDeck.getDeckOf(_currentDeckId).Leader = newLeaderIndex;
            _leaderPoint.Value = _myDeck.getLeaderPoint(_currentDeckId);
            updateDeckPanel();
            closeOpendPanel();
        }

        /*  
         *  �q�[���[�̕ύX���s�� 
         *  switch�Ŗ������X�L����2�����j�b�g�ɑΉ������Ă܂�
         */
        private void changeHero(int newHeroIndex)
        {
            int heroId = convertToSkillFromHero(newHeroIndex);   //�q�[���[�ԍ�����X�L���ԍ��ɕϊ�

            _myDeck.getDeckOf(_currentDeckId).Unit[_heroEditIndex] = heroId;
            _sumCost.Value = _myDeck.getSumCost(_currentDeckId);
            updateDeckPanel();
            closeOpendPanel();
        }

        /*  �f�b�L�p�l���̍X�V���s��    */
        private void updateDeckPanel()
        {
            var list = _myDeck.getDeckOf(_currentDeckId);

            //�f�b�L���̕ύX
            deckNamTextMeshPro.SetText(list.DeckName); 

            //�R�X�g
            _sumCost.Value = _myDeck.getSumCost(_currentDeckId);
            _leaderPoint.Value = _myDeck.getLeaderPoint(_currentDeckId);

            //���[�_�[
            var leader = list.Leader;
            if(leader >= 0)  //��łȂ��Ȃ�
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

            //�q�[���[
            for (int i = 0; i < list.Unit.Length; i++)
            {
                var hero = list.Unit[i];
                if(hero >= 0)    //��łȂ��Ȃ�
                {
                    _currentHeroIcons[i].GetComponent<Image>().sprite = _myDeck.heroIconSprites[hero];  //cardIcon
                    _currentHeroSkills[i].transform.GetChild(0).GetComponent<Image>().sprite = _myDeck.heroSkillIconSprite[hero];    //Icon
                    _currentHeroSkills[i].transform.GetChild(1).GetComponent<Text>().text = _myDeck.heroSkillName[hero];   //Text
                    int fontSize = (_myDeck.heroSkillName[hero].Length >= 6) ? 15 : 25;
                    _currentHeroSkills[i].transform.GetChild(1).GetComponent<Text>().fontSize = fontSize;

                    var statusPanel = _currentHeroIcons[i].transform.GetChild(0);  //�X�e�[�^�X�p�l�����擾
                    //�e�p�����[�^�̐��l���擾
                    float[] values = new float[5];
                    var characterDataList = getCharaDataList(hero);
                    values[0] = characterDataList.hp;             //HP
                    values[1] = characterDataList.attackPower;    //Power
                    values[2] = characterDataList.moveSpeed;      //Speed
                    values[3] = characterDataList.attackRange;    //Attack Range
                    values[4] = characterDataList.searchRange;    //Search Range
                    //�e�p�����[�^�̃Q�[�W���X�V
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

                    var heroCostText = _currentHeroIcons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();   //�R�X�g�̃e�L�X�g���擾
                    heroCostText.text = "0";   //�R�X�g�̃e�L�X�g��ύX

                    var statusPanel = _currentHeroIcons[i].transform.GetChild(1);  //�X�e�[�^�X�p�l�����擾

                    //�e�p�����[�^�̃Q�[�W���X�V
                    for (int gageItr = 0; gageItr < 5; gageItr++)
                    {
                        var fillImage = statusPanel.GetChild(gageItr).GetChild(1).GetComponent<Image>();
                        fillImage.fillAmount = 0;
                    }
                }

            }

            //�R�X�g�𔻒�ƃX���b�g����łȂ����m�F�����{�^���̏�Ԃ𔽉f
            var checkDuplication = _allowDuplication ? true : !_myDeck.isDuplicationDeckOf(_currentDeckId); //�d���������Ȃ�true, �d���������Ȃ��Ȃ�d�����Ȃ����m�F
            if (/*_leaderPoint.Value < _sumCost.Value ||*/ !_myDeck.isAllSetDeckOf(_currentDeckId) || !checkDuplication)
            {
                _startButton.interactable = false;
            }
            else
            {
                _startButton.interactable = true;
            }
        }

        //�X�L���I���{�^��
        public void OnCloseHeroSkillSelectPanel(int id)
        {
            _currentSkillId = id;

            var skillId = _myDeck.getDeckOf(_currentDeckId).Unit[_heroEditIndex];
            var heroId = _myDeck.convertToHeroFromSkill(skillId);
            changeHero(heroId);
            
            closeOpendPanel();

            _currentSkillId = 0;    //0�ɖ߂�
        }



        //�X�L���I��ԍ��i_currentSkillId�j�ƃ��j�b�g�ԍ�(0�`5�j����X�L���ԍ��i0�`8�j�����߂�
        private int convertToSkillFromHero(int heroId)
        {
            int skillId = 0;
            switch (heroId)
            {
                case 0: //��{��
                    skillId = (int)Unit.UnitType.Basic;
                    break;
                case 1: //�X�i�C�p�[
                    skillId = (int)Unit.UnitType.Sniper;
                    break;
                case 2: //���R��
                    if (_currentSkillId == 0) skillId = (int)Unit.UnitType.Recon;
                    else skillId = (int)Unit.UnitType.Funnnel;
                    break;
                case 3: //�^���N
                    if (_currentSkillId == 0) skillId = (int)Unit.UnitType.Tank;
                    else skillId = (int)Unit.UnitType.Chainbuff;
                    break;
                case 4: //�f�o�b�t�@�[
                    skillId = (int)Unit.UnitType.Debuffer;
                    break;
                case 5: //�ˌ���
                    if (_currentSkillId == 0) skillId = (int)Unit.UnitType.Assault;
                    else skillId = (int)Unit.UnitType.Stealth;
                    break;
                default:
                    skillId = -1;
                    break;
            }
            return skillId;
        }
        
        //�X�L���ԍ�����L�����̃X�e�[�^�X���擾����
        private Unit.CharacterData getCharaDataList(int skillId)
        {
            int heroId = 0; //CharacterDataList�̃C���f�b�N�X�ɂ��킹��
            switch (skillId)
            {
                case (int)Unit.UnitType.Basic: //��{��
                    heroId = 0;
                    break;
                case (int)Unit.UnitType.Sniper: //�X�i�C�p�[
                    heroId = 4;
                    break;
                case (int)Unit.UnitType.Recon:  //���R��
                    heroId = 2;
                    break;
                case (int)Unit.UnitType.Funnnel: //�t�@���l��
                    heroId = 7;
                    break;
                case (int)Unit.UnitType.Tank:   //�^���N
                    heroId = 5;
                    break;
                case (int)Unit.UnitType.Chainbuff: //�`�F�C���o�t�i�Ȃ��I�I�I�I�j
                    heroId = 5;
                    Debug.Log("�擾���s");
                    break;
                case (int)Unit.UnitType.Debuffer: //�f�o�b�t�@�[
                    heroId = 3;
                    break;
                case (int)Unit.UnitType.Assault:    //�A�T���g
                    heroId = 1;
                    break;
                case (int)Unit.UnitType.Stealth: //�X�e���X(�Ȃ��I�I�I)
                    heroId = 1;
                    Debug.Log("�擾���s");
                    break;
                default:
                    break;
            }
            
            return _characterDataList.characterDataList[heroId];
        }
    }

}