using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextSectionView : MonoBehaviour
{
    private bool isShowTitle = true;
    [SerializeField]
    private Image[] titleImages;

    private Image gameStartImage;
    private GameManager gameManager;

    public bool isDebug = false;
    public GameState state;
    
    private void Start()
    {
        gameManager = GameManager.Instance;
        gameStartImage = GetComponent<Image>();
        var bgm = gameManager.GetComponent<BGMControl>();
        var se = gameManager.GetComponent<ScenarioAudio>();
        
        this.UpdateAsObservable()
            .Where(_ => (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && isShowTitle)
            .ThrottleFirst(TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ =>
            {
                isShowTitle = false;
                StartCoroutine(GameStartRoutine(isDebug));
                bgm.StopBGM();
                se.ShotSE("start");
            }).AddTo(gameObject);

        gameManager.CurrentState
            .Where(state => state == GameState.Title)
            .Subscribe(_ =>
            {
                ImageShow();
                isShowTitle = true;
                bgm.PlayBGM("title");
            }).AddTo(gameObject);
    }

    private IEnumerator GameStartRoutine(bool isDebug)
    {
        if (!isDebug)
        {
            Fade.Instance.SimpleFadeOut(1.0f);
            yield return new WaitForSeconds(3.0f);

            ImageHide();
        
            gameManager.GameStart();
            Fade.Instance.SimpleFadeIn(1.0f);
            yield return new WaitForSeconds(1.0f);
            gameManager.ScenarioStart();
        }
        else
        {
            Fade.Instance.SimpleFadeOut(1.0f);
            yield return new WaitForSeconds(3.0f);

            ImageHide();

            gameManager.CurrentState.Value = state;
            SceneManager.LoadScene(Enum.GetName(typeof(GameState),state), LoadSceneMode.Additive);
            Fade.Instance.SimpleFadeIn(1.0f);
            yield return new WaitForSeconds(1.0f);
            gameManager.ScenarioStart();
        }
    }

    private void ImageHide()
    {
        gameStartImage.enabled = false;
        foreach (var i in titleImages)
        {
            i.enabled = false;
        }
    }
    
    private void ImageShow()
    {
        gameStartImage.enabled = true;
        foreach (var i in titleImages)
        {
            i.enabled = true;
        }
    }
}
