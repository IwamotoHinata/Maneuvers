using System.Collections;
using Fusion;
using Online;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Unit;


public class SkillUIPresenter : NetworkBehaviour
{
    [SerializeField] private Slider[] characterSPSliders;
    [SerializeField] private Image[] skillIcons;
    [SerializeField] private ObserveCharacters observeCharacters;
    private CharacterStatus myCharacterBaseData;

    private Subject<EnemySkillIconData> observeEnemySkills = new Subject<EnemySkillIconData>();

    public struct EnemySkillIconData : INetworkStruct
    {
        public Color iconColor;
        public int indexNum;
    }

    // EnemySkillIcondataが変更した時にホスト、クライアントでOnEnemySkillDataChangedを発動
    [Networked(OnChanged = nameof(OnEnemySkillDataChanged))]
    public EnemySkillIconData enemyIconDataChanged { get; set; }

    public static void OnEnemySkillDataChanged(Changed<SkillUIPresenter> changed)
    {
        //void StartのobserveEnemySkillより、SetEnemyIconを発動
        changed.Behaviour.observeEnemySkills.OnNext(changed.Behaviour.enemyIconDataChanged);
    }

    private Subject<EnemySliderData> observeEnemySlider = new Subject<EnemySliderData>();

    public struct EnemySliderData : INetworkStruct
    {
        public float coolTime;
        public int indexNum;
    }

    [Networked(OnChanged = nameof(OnEnemySliderChanged))]
    public EnemySliderData enemySliderData { get; set; }

    public static void OnEnemySliderChanged(Changed<SkillUIPresenter> changed)
    {
        //void StartのobserveEnemySliderより、SetEnemySliderを発動
        changed.Behaviour.observeEnemySlider.OnNext(changed.Behaviour.enemySliderData);
    }

    private void Start()
    {
        observeEnemySkills.Subscribe(data => { SetEnemyIcon(data.indexNum, data.iconColor); }).AddTo(this);
        observeEnemySlider.Subscribe(data => { SetEnemySlider(data.indexNum, data.coolTime); }).AddTo(this);
    }

    #region Host

    //ObserveCharactersよりホストのキャラが生成された時にこの関数を使用する。
    public void GetMySkillData(int indexNum)
    {
        characterSPSliders[indexNum].value = 1;
        skillIcons[indexNum].color = new Color(1, 1, 1);
        observeCharacters.myUiCharacters[indexNum].GetComponent<HeroSkill>().skillObservable
            .Subscribe(skillCoolTime => OnMyUseSkill(indexNum, skillCoolTime)).AddTo(this);
    }

    private void OnMyUseSkill(int indexNum, float skillCoolTime)
    {
        characterSPSliders[indexNum].value = 0;
        skillIcons[indexNum].color = new Color(0.4f, 0.4f, 0.4f);
        StartCoroutine(MySkillCoolDown(indexNum, skillCoolTime));
    }

    private IEnumerator MySkillCoolDown(int indexNum, float skillCoolTime)
    {
        float nowCoolDownTime = 0;
        while (skillCoolTime >= nowCoolDownTime)
        {
            nowCoolDownTime += 1;
            characterSPSliders[indexNum].value = nowCoolDownTime / skillCoolTime;
            yield return new WaitForSeconds(1);
        }

        MySkillAvailable(indexNum);
    }

    private void MySkillAvailable(int indexNum)
    {
        skillIcons[indexNum].color = new Color(1, 1, 1);
    }

    #endregion

    #region Client

    //ObserveCharactersよりクライアントのキャラが生成された時にこの関数を使用する。
    public void GetEnemySkillData(int index)
    {
        EnemySkillIconData a = new EnemySkillIconData
        {
            iconColor = new Color(1.0f, 1.0f, 1.0f),
            indexNum = index
        };
        enemyIconDataChanged = a;

        //クライアントがスキルを発動した際にOnEnemyUseSkillを発動
        observeCharacters.enemyUiCharacters[index].GetComponent<HeroSkill>().skillObservable
            .Subscribe(skillCoolTime => OnEnemyUseSkill(index, skillCoolTime)).AddTo(this);
    }

    //構造体を作成し、OnEnemySliderChanged,OnEnemySkillDataChangedを発動
    void OnEnemyUseSkill(int index, float coolTime)
    {
        EnemySkillIconData icon = new EnemySkillIconData
        {
            iconColor = new Color(0.4f, 0.4f, 0.4f),
            indexNum = index,
        };
        enemyIconDataChanged = icon;
        EnemySliderData slider = new EnemySliderData
        {
            indexNum = index,
            coolTime = coolTime,
        };
        enemySliderData = slider;
    }

    void SetEnemyIcon(int index, Color iconColor)
    {
        if (GameLauncher.Runner.GameMode.Equals(GameMode.Client))
        {
            skillIcons[index].color = iconColor;
        }
    }

    //Sliderを0にした後にEnemySkillCoolDownを発動。クライアントならEnemySkillCoolDownを発動
    void SetEnemySlider(int index, float coolTime)
    {
        if (GameLauncher.Runner.GameMode.Equals(GameMode.Client))
        {
            characterSPSliders[index].value = 0;
            StartCoroutine(EnemySkillCoolDown(index, coolTime));
        }
    }

    IEnumerator EnemySkillCoolDown(int indexNum, float skillCoolTime)
    {
        float nowCoolDownTime = 0;
        while (skillCoolTime >= nowCoolDownTime)
        {
            nowCoolDownTime += 1;
            characterSPSliders[indexNum].value = nowCoolDownTime / skillCoolTime;
            yield return new WaitForSeconds(1);
        }

        RPC_EnemySkillAvailable(indexNum);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void RPC_EnemySkillAvailable(int index)
    {
        EnemySkillAvailable(index);
    }

    //スキルが発動できるようになったら、構造体を作成し、OnEnemySliderChanged,OnEnemySkillDataChangedを発動
    private void EnemySkillAvailable(int index)
    {
        EnemySkillIconData icon = new EnemySkillIconData
        {
            iconColor = new Color(1.0f, 1.0f, 1.0f),
            indexNum = index,
        };
        enemyIconDataChanged = icon;
        EnemySliderData slider = new EnemySliderData
        {
            indexNum = index,
            coolTime = 1,
        };
        enemySliderData = slider;
    }

    #endregion
}