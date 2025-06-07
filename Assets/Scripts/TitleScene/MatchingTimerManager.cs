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

    //�^�C�g���V�[���Ɉڍs�����Ƃ��̓^�C�g�����΂��ă��C�����j����\��������
    //���C���Q�[���V�[���Ɉڍs�����Ƃ��Ƀ}�b�`���O�^�C�}�[���폜
    public void delateTimer(Scene nextScene , LoadSceneMode mode)
    {
        if (nextScene.name == "MatchingTest")
        {
            Destroy(GameObject.Find("TitlePanel"));//�^�C�g���p�̃p�l����j��
            GameObject.Find("MatchingCanvas").transform.Find("MainMenuPanel").gameObject.SetActive(true);//���C�����j���p�̃p�l����\��
            BGMPlayer.Instance.playBGM(BGMType.Title);
        }

        if (nextScene.name == "MainGame")
        { 
            Destroy(this.gameObject);
        }           
    }
}
