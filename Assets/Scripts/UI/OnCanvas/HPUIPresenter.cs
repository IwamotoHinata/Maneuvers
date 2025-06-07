using UnityEngine;
using Fusion;
using Online;
using UnityEngine.UI;
using Unit;
using UniRx;

public class HPUIPresenter : NetworkBehaviour
{
    [SerializeField] private Slider[] characterHPSliders;
    [SerializeField] private ObserveCharacters observeCharacters;
    private CharacterStatus myCharacterBaseData;
    
    private Subject<EnemyHPData> observeEnemyHP = new Subject<EnemyHPData>();

    public struct EnemyHPData : INetworkStruct
    {
        public float currentHp;
        public float maxHp;
        public int indexNum;
    }

    [Networked(OnChanged = nameof(OnEnemySliderChanged))]
    public EnemyHPData enemySliderData { get; set; }

    public static void OnEnemySliderChanged(Changed<HPUIPresenter> changed)
    {
        //void StartのobserveEnemyHp.Subscribeより、SetEnemySliderを発動
        changed.Behaviour.observeEnemyHP.OnNext(changed.Behaviour.enemySliderData);
    }

    private void Start()
    {
        observeEnemyHP.Subscribe(data => { SetEnemySlider(data.indexNum, data.currentHp,data.maxHp); }).AddTo(this);
    }

    #region Host

    //ObserveCharactersよりホストのキャラが生成された時にこの関数を使用する。
    public void GetMyHPData(int indexNum)
    {
        //キャラクターのHPが変動した際にSetMyHpDataを発動
        observeCharacters.myUiCharacters[indexNum].GetComponent<CharacterStatus>().OnCharacterHPChanged
            .Subscribe(currentHp => SetMyHPData(indexNum, currentHp)).AddTo(this);
    }

    //スライダーの値を変更
    void SetMyHPData(int index, float hp)
    {
        characterHPSliders[index].value =
            hp / observeCharacters.myUiCharacters[index].GetComponent<CharacterBaseData>().MaxHP;
    }

    #endregion

    #region Client

    //ObserveCharactersよりクライアントのキャラが生成された時にこの関数を使用する。
    public void GetEnemyHPData(int index)
    {
        //クライアントのHPが変更した際にsetEnemyHpDataを発動
        observeCharacters.enemyUiCharacters[index].GetComponent<CharacterStatus>().OnCharacterHPChanged
            .Subscribe(currentHp => SetEnemyHpData(index, currentHp)).AddTo(this);
    }
    
    //EnemyHpDataを作成し、クライアントにHpDataを送る。OnEnemySliderChangedを発動
    void SetEnemyHpData(int index, float currentHp)
    {
        EnemyHPData slider = new EnemyHPData
        {
            indexNum = index,
            currentHp = currentHp,
            maxHp = observeCharacters.enemyUiCharacters[index].GetComponent<CharacterBaseData>().MyHp,
        };
        enemySliderData = slider;
    }

    void SetEnemySlider(int index, float currenHp ,float maxHP)
    {
        if (GameLauncher.Runner.GameMode.Equals(GameMode.Client))
        {
            characterHPSliders[index].value = currenHp / maxHP;
        }
    }

    #endregion
}