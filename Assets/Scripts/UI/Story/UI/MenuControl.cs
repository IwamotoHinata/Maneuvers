using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class MenuControl : MonoBehaviour
{
    private GameObject menu;
    private void Start()
    {
        menu = transform.Find("BackGround").gameObject;
        menu.SetActive(false);
        
        this.UpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.Escape))
            .ThrottleFirst(TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ =>
            {
                if (menu.activeSelf)
                {
                    menu.SetActive(false);
                }
                else
                {
                    menu.SetActive(true);
                }
            });
    }
}
