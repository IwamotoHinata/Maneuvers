using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    private Dictionary<string,Sprite> spritesDic = new Dictionary<string, Sprite>();
    private Image image;
    
    private void Awake()
    {
        image = GetComponent<Image>();
        foreach (var s in sprites)
        {
            spritesDic.Add(s.name,s);
        }
    }

    public void SetCharacterImage(EmotionType eType, bool isTransparent=false)
    {
        switch (eType)
        {
            case EmotionType.Empty:
                image.color = Color.clear;
                break;
            case EmotionType.sa:
                image.color = Color.white;
                image.sprite = spritesDic["sa"];
                break;
            case EmotionType.s1 :
                image.color = Color.white;
                image.sprite = spritesDic["s1"];
                break;
            case EmotionType.s1q :
                image.color = Color.black;
                image.sprite = spritesDic["s1"];
                break;
            case EmotionType.sd:
                image.color = Color.white;
                image.sprite = spritesDic["sd"];
                break;
            case EmotionType.h1:
                image.color = Color.white;
                image.sprite = spritesDic["h1"];
                break;
            case EmotionType.h1q:
                image.color = Color.black;
                image.sprite = spritesDic["h1"];
                break;
            case EmotionType.sor1:
                image.color = Color.white;
                image.sprite = spritesDic["sor1"];
                break;
            case EmotionType.sor1q:
                image.color = Color.black;
                image.sprite = spritesDic["sor1"];
                break;
            case EmotionType.cul:
                image.color = Color.white;
                image.sprite = spritesDic["cul"];
                break;
            case EmotionType.cul2:
                image.color = Color.white;
                image.sprite = spritesDic["cul2"];
                break;
            case EmotionType.mimi:
                image.color = Color.white;
                image.sprite = spritesDic["mimi"];
                break;
            case EmotionType.mimi2:
                image.color = Color.white;
                image.sprite = spritesDic["mimi2"];
                break;
            case EmotionType.seri:
                image.color = Color.white;
                image.sprite = spritesDic["seri"];
                break;
            
            case EmotionType.a1:
                image.color = Color.white;
                image.sprite = spritesDic["a1"];
                break;
            case EmotionType.a2:
                image.color = Color.white;
                image.sprite = spritesDic["a2"];
                break;
            case EmotionType.a3:
                image.color = Color.white;
                image.sprite = spritesDic["a3"];
                break;
            case EmotionType.a4:
                image.color = Color.white;
                image.sprite = spritesDic["a4"];
                break;
            case EmotionType.a5:
                image.color = Color.white;
                image.sprite = spritesDic["a5"];
                break;
            case EmotionType.a6:
                image.color = Color.white;
                image.sprite = spritesDic["a6"];
                break;
            case EmotionType.a7:
                image.color = Color.white;
                image.sprite = spritesDic["a7"];
                break;
            case EmotionType.b1:
                image.color = Color.white;
                image.sprite = spritesDic["b1"];
                break;
            case EmotionType.b2:
                image.color = Color.white;
                image.sprite = spritesDic["b2"];
                break;
            case EmotionType.b3:
                image.color = Color.white;
                image.sprite = spritesDic["b3"];
                break;
            case EmotionType.b4:
                image.color = Color.white;
                image.sprite = spritesDic["b4"];
                break;
            case EmotionType.b5:
                image.color = Color.white;
                image.sprite = spritesDic["b5"];
                break;
            case EmotionType.b6:
                image.color = Color.white;
                image.sprite = spritesDic["b6"];
                break;
            case EmotionType.c1:
                image.color = Color.white;
                image.sprite = spritesDic["c1"];
                break;
            case EmotionType.c2:
                image.color = Color.white;
                image.sprite = spritesDic["c2"];
                break;
            case EmotionType.c3:
                image.color = Color.white;
                image.sprite = spritesDic["c3"];
                break;
            case EmotionType.c4:
                image.color = Color.white;
                image.sprite = spritesDic["c4"];
                break;
            case EmotionType.c5:
                image.color = Color.white;
                image.sprite = spritesDic["c5"];
                break;
            case EmotionType.c6:
                image.color = Color.white;
                image.sprite = spritesDic["c6"];
                break;
            case EmotionType.c7:
                image.color = Color.white;
                image.sprite = spritesDic["c7"];
                break;
            case EmotionType.c8:
                image.color = Color.white;
                image.sprite = spritesDic["c8"];
                break;
            case EmotionType.c9:
                image.color = Color.white;
                image.sprite = spritesDic["c9"];
                break;
            case EmotionType.d1:
                image.color = Color.white;
                image.sprite = spritesDic["d1"];
                break;
            case EmotionType.d2:
                image.color = Color.white;
                image.sprite = spritesDic["d2"];
                break;
            case EmotionType.d3:
                image.color = Color.white;
                image.sprite = spritesDic["d3"];
                break;
            case EmotionType.d4:
                image.color = Color.white;
                image.sprite = spritesDic["d4"];
                break;
            case EmotionType.mb:
                image.color = Color.white;
                image.sprite = spritesDic["mb"];
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(eType), eType, "感情タイプの指定が不適切です");
        }
        if (isTransparent)
        {
            image.color -= new Color(0f,0f,0f,0.5f);
        }
    }
}
