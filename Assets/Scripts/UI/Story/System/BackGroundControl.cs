using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class BackGroundControl : MonoBehaviour
{
    private Image bgSprite;
    private Image bgbSprite;
    private Dictionary<string, Sprite> bgSpriteDic = new Dictionary<string, Sprite>();
    [SerializeField] private Image titleSprite;

    private void Start()
    {
        foreach (var b in Resources.LoadAll<Sprite>("BackGround"))
        {
            bgSpriteDic.Add(b.name, b);
        }
    }

    public void FindBackGround()
    {
        try
        {
            bgSprite = GameObject.FindWithTag("BackGround").GetComponent<Image>();
        }
        catch (NullReferenceException e)
        {
            Debug.Log("背景のタグが設定されていません．");
        }
    }

    public void SetBackGround(BackGroundType bgType)
    {
        switch (bgType)
        {
            case BackGroundType.None:
                ;
                break;
            case BackGroundType.bg1:
                bgSprite.sprite = bgSpriteDic["_scenarioBack1"];
                break;
            case BackGroundType.bg2:
                bgSprite.sprite = bgSpriteDic["_scenarioBack2"];
                break;
            case BackGroundType.White2:
                bgSprite.sprite = bgSpriteDic["bg_white2"];
                break;
            case BackGroundType.bg3:
                bgSprite.sprite = bgSpriteDic["_scenarioBack3"];
                break;
            case BackGroundType.bg4:
                bgSprite.sprite = bgSpriteDic["_scenarioBack4"];
                break;
            case BackGroundType.bg5:
                bgSprite.sprite = bgSpriteDic["_scenarioBack5"];
                break;
            case BackGroundType.bg6:
                bgSprite.sprite = bgSpriteDic["_scenarioBack6"];
                break;
            case BackGroundType.bg7:
                bgSprite.sprite = bgSpriteDic["_scenarioBack7"];
                break;
            case BackGroundType.bg8:
                bgSprite.sprite = bgSpriteDic["_scenarioBack8"];
                break;
            case BackGroundType.Corridor:
                bgSprite.sprite = bgSpriteDic["bg_corridor"];
                break;
            case BackGroundType.Entrance:
                bgSprite.sprite = bgSpriteDic["bg_entrance"];
                break;
            case BackGroundType.SchoolGate:
                bgSprite.sprite = bgSpriteDic["bg_school_gate"];
                break;
            case BackGroundType.SchoolGate2:
                bgSprite.sprite = bgSpriteDic["bg_school_gate2"];
                break;
            case BackGroundType.BehindSchoolBuilding:
                bgSprite.sprite = bgSpriteDic["bg_behind_school"];
                break;
        }
    }
    
    public void FindBackGroundBad()
    {
        try
        {
            bgbSprite = GameObject.FindWithTag("BackGroundBad").GetComponent<Image>();
        }
        catch (NullReferenceException e)
        {
            Debug.Log("バッド背景のタグが設定されていません．");
        }
    }
    
    public void SetBackGroundBad(BackGroundType bgType)
    {
        switch (bgType)
        {
            case BackGroundType.Bad1:
                bgbSprite.sprite = bgSpriteDic["bg_bad1"];
                bgbSprite.color = Color.white;
                break;
            case BackGroundType.Bad2:
                bgbSprite.sprite = bgSpriteDic["bg_bad2"];
                bgbSprite.color = Color.white;
                break;
            case BackGroundType.Bad3:
                bgbSprite.sprite = bgSpriteDic["bg_bad3"];
                bgbSprite.color = Color.white;
                break;
            case BackGroundType.Bad4:
                bgbSprite.sprite = bgSpriteDic["bg_bad4"];
                bgbSprite.color = Color.white;
                break;
        }
    }

    public void SetTitleBackGround(BackGroundType bgType)
    {
        switch (bgType)
        {
            case BackGroundType.Bad1:
                titleSprite.sprite = bgSpriteDic["bg_title1"];
                break;
            case BackGroundType.Bad2:
                titleSprite.sprite = bgSpriteDic["bg_title2"];
                break;
            case BackGroundType.Bad3:
                titleSprite.sprite = bgSpriteDic["bg_title3"];
                break;
            case BackGroundType.Bad4:
                titleSprite.sprite = bgSpriteDic["bg_title4"];
                break;
        }
    }
}
