using System;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameState initState;
    public static GameManager Instance;
    [SerializeField]
    private ReactiveGameStateProperty currentState;
    public ReactiveProperty<GameState> CurrentState
    {
        get => currentState;
        set => currentState.Value = value.Value;
    }
    [SerializeField]
    private BoolReactiveProperty isStartScenario = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> IsStartScenario => isStartScenario;

    private bool isEffect = false;
    public bool IsEffect
    {
        get => isEffect;
        set => isEffect = value;
    }

    private int damageCount;
    public int DamageCount
    {
        get => damageCount;
        set => damageCount = value;
    }

    private Coroutine routine = null;
    private BackGroundControl backGround;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            currentState = new ReactiveGameStateProperty(initState);
            backGround = GetComponent<BackGroundControl>();
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("切り捨て");
            Destroy(gameObject);
        }
        
        currentState
            .Where(state => state == GameState.Title)
            .Subscribe(_ =>
            {
                damageCount = 0;
            }).AddTo(gameObject);
    }

    // このメソッドで次のシーンへ移行した扱いになる
    private void ChangeState(GameState state)
    {
        currentState.Value = state;
    }

    // シーン読み込み処理（ボタンがクリックされたら呼び出す）
    public void GameStart()
    {
        ChangeState(GameState.Sec1); // ゲームの状態をタイトルからSec1の状態へ変更
        SceneManager.LoadScene("Sec1", LoadSceneMode.Additive);
    }

    public void ScenarioStart()
    {
        isStartScenario.Value = true;
    }
    
    public void ScenarioStop()
    {
        isStartScenario.Value = false;
    }

    public void ScenarioToExplore(GameState state)
    {
        switch (state)
        {
            case GameState.Sec1:
                if (routine == null)
                {
                    routine = StartCoroutine(ScenarioToExploreRoutine(1.5f,state,GameState.Exp1,FadeType.Simple));
                }
                break;
            case GameState.Exp1:
                if (routine == null)
                {
                    routine = StartCoroutine(ExploreToScenarioRoutine(1.5f,state,GameState.Sec2,FadeType.Simple));
                }
                break;
            case GameState.Sec2:
                if (routine == null)
                {
                    routine = StartCoroutine(ScenarioToExploreRoutine(1.5f,state,GameState.Exp2,FadeType.Lefts));
                }
                break;
            case GameState.Exp2:
                if (routine == null)
                {
                    routine = StartCoroutine(ExploreToScenarioRoutine(1.5f,state,GameState.Sec3,FadeType.Lefts));
                }
                break;
            case GameState.Sec3:
                if (routine == null)
                {
                    routine = StartCoroutine(ScenarioToExploreRoutine(1.5f,state,GameState.Exp3,FadeType.Lefts));
                }
                break;
            case GameState.Exp3:
                if (routine == null)
                {
                    routine = StartCoroutine(ExploreToScenarioRoutine(1.5f,state,GameState.Sec4,FadeType.Lefts));
                }
                break;
            case GameState.Sec4:
                if (routine == null)
                {
                    routine = StartCoroutine(ScenarioToExploreRoutine(1.5f,state,GameState.Exp4,FadeType.Lefts));
                }
                break;
            case GameState.Exp4:
                if (routine == null)
                {
                    // ダメージにより分岐
                    if (damageCount < 10)
                    {
                        routine = StartCoroutine(ExploreToScenarioRoutine(1.5f,state,GameState.Sec5,FadeType.Lefts));
                    }
                    // バッドエンド1
                    else
                    {
                        routine = StartCoroutine(ExploreToScenarioRoutine(1.5f,state,GameState.Sec6,FadeType.Lefts));
                    }
                }
                break;
            case GameState.Sec5:
                if (routine == null)
                {
                    routine = StartCoroutine(ScenarioToExploreRoutine(1.5f,state,GameState.Exp5,FadeType.Lefts));
                }
                break;
            case GameState.Sec6:
                if (routine == null)
                {
                    backGround.FindBackGroundBad();
                    routine = StartCoroutine(MoveTitle(2.0f,state,FadeType.Simple,BackGroundType.Bad1));
                }
                break;
            case GameState.Exp5:
                if (routine == null)
                {
                    routine = StartCoroutine(ExploreToScenarioRoutine(1.5f,state,GameState.Sec7,FadeType.Lefts));
                }
                break;
            case GameState.Sec8:
                if (routine == null)
                {
                    backGround.FindBackGroundBad();
                    routine = StartCoroutine(MoveTitle(2.0f,GameState.Sec7,FadeType.Simple,BackGroundType.Bad2));
                }
                break;
            case GameState.Sec9:
                if (routine == null)
                {
                   routine = StartCoroutine(ScenarioToExploreRoutine(1.5f,GameState.Sec7,GameState.Exp6,FadeType.Lefts));
                }
                break;
            case GameState.Exp6:
                if (routine == null)
                {
                    routine = StartCoroutine(ExploreToScenarioRoutine(1.5f,state,GameState.Sec10,FadeType.Lefts));
                }
                break;
            case GameState.Sec11:
                if (routine == null)
                {
                    backGround.FindBackGroundBad();
                    routine = StartCoroutine(MoveTitle(2.0f,GameState.Sec10,FadeType.Simple,BackGroundType.Bad3));
                }
                break;
            case GameState.Sec12:
                if (routine == null)
                {
                    backGround.FindBackGroundBad();
                    routine = StartCoroutine(MoveTitle(2.0f,GameState.Sec10,FadeType.Simple,BackGroundType.Bad4));
                }
                break;
        }
    }
    
    // シナリオから探索の遷移
    private IEnumerator ScenarioToExploreRoutine(float waitTime, GameState state, GameState nextState, FadeType fadeType)
    {
        var fade = Fade.Instance;
        switch (fadeType)
        {
           case FadeType.Simple:
               fade.SimpleFadeOut(waitTime);
               yield return new WaitForSeconds(waitTime);
               fade.SimpleFadeIn(waitTime);
                //SceneManager.UnloadSceneAsync(Enum.GetName(typeof(GameState), state));
                //SceneManager.LoadScene(Enum.GetName(typeof(GameState), nextState),LoadSceneMode.Additive);
                GridTransition.Instance.nextTransition = "MainMenu";
                GameObject.Find("GridTransition").GetComponent<Animator>().SetTrigger("isTransition");
                yield return new WaitForSeconds(waitTime);
               break;
           case FadeType.Lefts:
               fade.LeftFadeOut(waitTime);
               yield return new WaitForSeconds(waitTime);
               fade.LeftFadeIn(waitTime);
               SceneManager.UnloadSceneAsync(Enum.GetName(typeof(GameState), state));
               SceneManager.LoadScene(Enum.GetName(typeof(GameState), nextState),LoadSceneMode.Additive);
               yield return new WaitForSeconds(waitTime);
               break;
        }
        ChangeState(nextState);
        ScenarioStop();
        routine = null;
    }
    
    // 探索からシナリオの遷移
    private IEnumerator ExploreToScenarioRoutine(float waitTime, GameState state, GameState nextState,FadeType fadeType)
    {
        var fade = Fade.Instance;
        switch (fadeType)
        {
            case FadeType.Simple:
                fade.SimpleFadeOut(waitTime);
                yield return new WaitForSeconds(waitTime);
                fade.SimpleFadeIn(waitTime);
                SceneManager.UnloadSceneAsync(Enum.GetName(typeof(GameState), state));
                SceneManager.LoadScene(Enum.GetName(typeof(GameState), nextState),LoadSceneMode.Additive);
                yield return new WaitForSeconds(waitTime);
                break;
            case FadeType.Lefts:
                fade.LeftFadeOut(waitTime);
                yield return new WaitForSeconds(waitTime);
                fade.LeftFadeIn(waitTime);
                SceneManager.UnloadSceneAsync(Enum.GetName(typeof(GameState), state));
                SceneManager.LoadScene(Enum.GetName(typeof(GameState), nextState),LoadSceneMode.Additive);
                yield return new WaitForSeconds(waitTime);
                break;
        }
        ChangeState(nextState);
        ScenarioStart();
        routine = null;
    }
    
    // タイトルへの遷移
    private IEnumerator MoveTitle(float waitTime, GameState state,FadeType fadeType,BackGroundType bType)
    {
        var fade = Fade.Instance;
        switch (fadeType)
        {
            case FadeType.Simple:
                fade.SimpleFadeOut(waitTime);
                yield return new WaitForSeconds(waitTime);
                fade.SimpleFadeIn(waitTime);
                backGround.SetBackGroundBad(bType);
                yield return new WaitForSeconds(waitTime*3);
                fade.SimpleFadeOut(waitTime);
                yield return new WaitForSeconds(waitTime*3);
                SceneManager.UnloadSceneAsync(Enum.GetName(typeof(GameState), state));
                backGround.SetTitleBackGround(bType);
                ScenarioStop();
                fade.SimpleFadeIn(waitTime/3);
                ChangeState(GameState.Title);
                break;
            case FadeType.Lefts:
                fade.LeftFadeOut(waitTime);
                yield return new WaitForSeconds(waitTime);
                fade.LeftFadeIn(waitTime);
                SceneManager.UnloadSceneAsync(Enum.GetName(typeof(GameState), state));
                yield return new WaitForSeconds(waitTime);
                break;
        }
        routine = null;
    }
}