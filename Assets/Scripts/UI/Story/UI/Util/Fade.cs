using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class Fade : MonoBehaviour
{
    [SerializeField] private Image simpleFade;
    [SerializeField] private Image leftFadeImage;
    [SerializeField] private Image whiteFadeImage;
    
    private Material leftFade;
    private Material whiteFade;
    private static Fade instance;
    public static Fade Instance => instance;

    private void Start()
    {
        if (instance == null) {
            instance = this;
            leftFade = leftFadeImage.material;
            whiteFade = whiteFadeImage.material;
            //DontDestroyOnLoad (gameObject);
        }
        else {
            Destroy (gameObject);
        }
    }

    public void SimpleFadeOut(float time)
    {
        StartCoroutine(SimpleFadeOutRoutine(time));
    }

    private IEnumerator SimpleFadeOutRoutine(float time)
    {
        var c = simpleFade.color;
        var elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            simpleFade.color = new Color(c.r,c.g,c.b,elapsedTime / time);
            yield return null;
        }
        simpleFade.color = new Color(c.r,c.g,c.b,1.0f);
    }
    
    public void SimpleFadeIn(float time)
    {
        StartCoroutine(SimpleFadeInRoutine(time));
    }

    private IEnumerator SimpleFadeInRoutine(float time)
    {
        var c = simpleFade.color;
        var elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            simpleFade.color = new Color(c.r,c.g,c.b,1.0f - (elapsedTime / time));
            yield return null;
        }
        simpleFade.color = new Color(c.r,c.g,c.b,0.0f);
    }

    public void LeftFadeOut(float time)
    {
        StartCoroutine(LeftFadeOutRoutine(time));
    }
    private IEnumerator LeftFadeOutRoutine(float time)
    {
        var elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            leftFade.SetFloat("_Clip",1.0f - (elapsedTime / time));
            yield return null;
        }

        leftFade.SetFloat("_Clip",0.0f);
    }
    
    public void LeftFadeIn(float time)
    {
        StartCoroutine(LeftFadeInRoutine(time));
    }
    private IEnumerator LeftFadeInRoutine(float time)
    {
        var elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            leftFade.SetFloat("_Clip",(elapsedTime / time));
            yield return null;
        }

        leftFade.SetFloat("_Clip",1.0f);
    }
    
    public void WhiteFadeOut(float time)
    {
        StartCoroutine(WhiteFadeOutRoutine(time));
    }
    private IEnumerator WhiteFadeOutRoutine(float time)
    {
        var elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            whiteFade.SetFloat("_Clip",1.0f - (elapsedTime / time));
            yield return null;
        }

        whiteFade.SetFloat("_Clip",0.0f);
    }
    
    public void WhiteFadeIn(float time)
    {
        StartCoroutine(WhiteFadeInRoutine(time));
    }
    private IEnumerator WhiteFadeInRoutine(float time)
    {
        var elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            whiteFade.SetFloat("_Clip",(elapsedTime / time));
            yield return null;
        }

        whiteFade.SetFloat("_Clip",1.0f);
    }
}
