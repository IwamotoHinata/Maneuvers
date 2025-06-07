using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

public class SaveButton : MonoBehaviour
{
    private Button button;
    private Text text;
    private SaveUtil saveUtil;
    private void Start()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
        saveUtil = new SaveUtil();

        button.OnClickAsObservable()
            .Subscribe(_ =>
            {
                var data = new SaveData("basyo");
                saveUtil.Save(data,text.name);
                button.interactable = false;
            });
    }
}
