using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioFade : MonoBehaviour
{
    private Coroutine fadeRoutine = null;
    private BackGroundControl backGround;

    private void Start()
    {
        backGround = GetComponent<BackGroundControl>();
    }

    public void SimpleFade(float waitTime, BackGroundType bgType, Action action, Action action2)
    {
        if (fadeRoutine == null)
        {
            backGround.FindBackGround();
            fadeRoutine = StartCoroutine(SimpleFadeRoutine(waitTime,bgType,action, action2));
        }
    }
    private IEnumerator SimpleFadeRoutine(float waitTime,BackGroundType bgType, Action action, Action action2)
    {
        GameManager.Instance.IsEffect = true;
        var fade = Fade.Instance;
        fade.SimpleFadeOut(waitTime);
        yield return new WaitForSeconds(waitTime * 3);
        backGround.SetBackGround(bgType);
        fade.SimpleFadeIn(waitTime);
        action2();
        action();
        yield return new WaitForSeconds(waitTime);
        GameManager.Instance.IsEffect = false;
        fadeRoutine = null;
    }
    
    public void LeftFade(float waitTime, BackGroundType bgType, Action action, Action action2)
    {
        if (fadeRoutine == null)
        {
            backGround.FindBackGround();
            fadeRoutine = StartCoroutine(LeftFadeRoutine(waitTime,bgType,action, action2));
        }
    }
    private IEnumerator LeftFadeRoutine(float waitTime,BackGroundType bgType, Action action, Action action2)
    {
        GameManager.Instance.IsEffect = true;
        var fade = Fade.Instance;
        fade.LeftFadeOut(waitTime);
        yield return new WaitForSeconds(waitTime * 3);
        backGround.SetBackGround(bgType);
        fade.LeftFadeIn(waitTime);
        action2();
        action();
        yield return new WaitForSeconds(waitTime);
        GameManager.Instance.IsEffect = false;
        fadeRoutine = null;
    }
    
    public void WhiteFade(float waitTime, BackGroundType bgType, Action action, Action action2)
    {
        if (fadeRoutine == null)
        {
            backGround.FindBackGround();
            fadeRoutine = StartCoroutine(WhiteFadeRoutine(waitTime,bgType,action, action2));
        }
    }
    private IEnumerator WhiteFadeRoutine(float waitTime,BackGroundType bgType, Action action, Action action2)
    {
        GameManager.Instance.IsEffect = true;
        var fade = Fade.Instance;
        fade.WhiteFadeOut(waitTime);
        yield return new WaitForSeconds(waitTime * 3);
        backGround.SetBackGround(bgType);
        fade.WhiteFadeIn(waitTime);
        action2();
        action();
        yield return new WaitForSeconds(waitTime);
        GameManager.Instance.IsEffect = false;
        fadeRoutine = null;
    }
}
