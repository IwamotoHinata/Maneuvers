using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SkillExplanationPopOut : MonoBehaviour
{
    [SerializeField] private Image[] skillExplanationImage;
    [SerializeField] private Image[] skillIconImage;
    private PlayerDeck _playerDeck;
    private AsyncOperationHandle<Sprite> handle;
    public int[] icon = new int[4]; //îzóÒÇÃêÈåæ
    private void Start()
    {
        _playerDeck = FindObjectOfType<PlayerDeck>();
        SetSkillIconImage();
    }
    async void SetSkillIconImage()
    {
        var units = _playerDeck.MyPlayerCharacterDeck.getSelectedDeck().Unit;
        String explanationPath = null;
        for (int i = 0; i < skillIconImage.Length; i++)
        {
            switch (units[i])
            {
                case 0:
                    explanationPath = "UI/Skill/Explanation/Basic.png";
                    icon[i] = 0;
                    break;
                case 1:
                    explanationPath = "UI/Skill/Explanation/Assault.png";
                    icon[i] = 1;
                    break;
                case 2:
                    explanationPath = "UI/Skill/Explanation/Recon.png";
                    icon[i] = 2;
                    break;
                case 3:
                    explanationPath = "UI/Skill/Explanation/Debuffer.png";
                    icon[i] = 3;
                    break;
                case 4:
                    explanationPath = "UI/Skill/Explanation/Sniper.png";
                    icon[i] = 4;
                    break;
                case 5:
                    explanationPath = "UI/Skill/Explanation/Tank.png";
                    icon[i] = 5;
                    break;
                case 6:
                    explanationPath = "UI/Skill/Explanation/Chainbuff.png";
                    icon[i] = 6;
                    break;
                case 9:
                    explanationPath = "UI/Skill/Explanation/Fanneler.png";
                    icon[i] = 9;
                    break;
                case 10:
                    explanationPath = "UI/Skill/Explanation/Stealth.png";
                    icon[i] = 10;
                    break;
            }
            handle = Addressables.LoadAssetAsync<Sprite>(explanationPath);
            await handle.Task;
            skillExplanationImage[i].sprite = handle.Result;
            skillExplanationImage[i].enabled = false;
        }
        Addressables.Release(handle);
    }
    public void OnMouseOver(int index)
    {
        skillExplanationImage[index].enabled = true;
    }
    public void OnMouseExit(int index)
    {
        skillExplanationImage[index].enabled = false;
    }
}
