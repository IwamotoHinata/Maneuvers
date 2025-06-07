using UnityEngine;
using UniRx;
using TMPro;
using UnityEngine.UI;
using Unit;
using Online;
using System.Linq;
using System.Collections.Generic;
using Fusion;

    public class CharacterStateImageChange : NetworkBehaviour
    {
        [SerializeField] private Image[] _myStateImage;
        CharacterStatus MyCharacterStatus;
        [SerializeField] private ObserveCharacters observeCharacters;
        [SerializeField] private GameObject[] flame = new GameObject[5];
    // Start is called before the first frame update

    private Subject<EnemyStateData> observeEnemyState = new Subject<EnemyStateData>();

        public struct EnemyStateData : INetworkStruct
        {
            public CharacterState enemyState;
            public int indexNum;
        }

        [Networked(OnChanged = nameof(OnEnemyStateImageChanged))]
        public EnemyStateData enemyStateData { get; set; }

        public static void OnEnemyStateImageChanged(Changed<CharacterStateImageChange> changed)
        {
            changed.Behaviour.observeEnemyState.OnNext(changed.Behaviour.enemyStateData);
        }
        private void Start()
        {
            observeEnemyState.Subscribe(data =>
            {
                SetEnemyState(data.indexNum, data.enemyState);
            }
            ).AddTo(this);
        }


        #region Host
        public void GetMyStateData(int indexNum)
        {
            observeCharacters.myUiCharacters[indexNum].GetComponent<CharacterStatus>().OnCharacterStateChanged
                .Subscribe(CharacterState =>
                {
                    ImageChange(_myStateImage[indexNum], CharacterState);
                }
                ).AddTo(this);

            observeCharacters.myUiCharacters[indexNum].GetComponent<CharacterMove>().isSelect
                .Subscribe(isSelect =>
                {
                    FlameChange(indexNum, isSelect);
                }
                ).AddTo(this);
    }
        private void ImageChange(Image _myStateImage, CharacterState _characterState)
        {
            Color _statecolor;
            switch (_characterState)
            {
                case CharacterState.Idle: ColorUtility.TryParseHtmlString("#00BA20", out _statecolor); break;
                case CharacterState.VigilanceMove:
                case CharacterState.Move: ColorUtility.TryParseHtmlString("#0000ff", out _statecolor); break;
                case CharacterState.MoveAttack:
                case CharacterState.Attack: ColorUtility.TryParseHtmlString("#ff0000", out _statecolor); break;
                case CharacterState.Dead: ColorUtility.TryParseHtmlString("#000000", out _statecolor); break;
                default: ColorUtility.TryParseHtmlString("#ffffff", out _statecolor); break;
            }
            _myStateImage.color = _statecolor;
        }

        private void FlameChange(int indexNum, bool isSelect)
        {
            if (isSelect)
            {
                flame[indexNum].SetActive(true);
            }
            else
            {
                flame[indexNum].SetActive(false);
            }
        }
    #endregion

    #region Client
    public void GetEnemyStateData(int indexNum)
        {
            observeCharacters.enemyUiCharacters[indexNum].GetComponent<CharacterStatus>().OnCharacterStateChanged
                .Subscribe(CharacterState =>
                {
                    SetEnemyStateData(indexNum, CharacterState);
                }
                ).AddTo(this);
        }
        void SetEnemyStateData(int index, CharacterState _characterState)
        {
            EnemyStateData state = new EnemyStateData
            {
                indexNum = index,
                enemyState = _characterState,
            };
            enemyStateData = state;
        }
        void SetEnemyState(int indexNum, CharacterState enemyState)
        {
            if (GameLauncher.Runner.GameMode.Equals(GameMode.Client))
            {
                ImageChange(_myStateImage[indexNum], enemyState);
            }
        }
        #endregion
    }

