using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MatchingTimerManager : MonoBehaviour
{
    public int _sec = 0;
    public int _min = 0;
    static public MatchingTimerManager instance;
    void Start()
    {
        if (instance)
        { 
            Destroy(this);
            return;
        }            
        else
            instance = this;


        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += delateTimer;
    }
    public void SetTime(int min, int sec)
    {
        _sec = sec;
        _min = min;
    }

    public void resetTimer()
    {
        _sec = 0;
        _min = 0;
    }

    //タイトルシーンに移行したときはタイトルを飛ばしてメインメニュを表示させる
    //メインゲームシーンに移行したときにマッチングタイマーを削除
    public void delateTimer(Scene nextScene , LoadSceneMode mode)
    {
        if (nextScene.name == "MatchingTest")
        {
            Destroy(GameObject.Find("TitlePanel"));//タイトル用のパネルを破壊
            GameObject.Find("MatchingCanvas").transform.Find("MainMenuPanel").gameObject.SetActive(true);//メインメニュ用のパネルを表示
            BGMPlayer.Instance.playBGM(BGMType.Title);
        }

        if (nextScene.name == "MainGame")
        { 
            Destroy(this.gameObject);
        }           
    }
}
