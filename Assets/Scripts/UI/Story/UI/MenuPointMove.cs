using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class MenuPointMove : MonoBehaviour
{
    private int pos = 0;
    private RectTransform rectTransform;
    private bool isClick = false;
    [SerializeField] private MessageManager mManager;
    
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        var se = GameManager.Instance.GetComponent<ScenarioAudio>();
        
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow)
                || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
                {
                    pos += 1;
                    se.ShotSE("cursor");
                }
                
                switch (pos % 2)
                {
                    case 0:
                        rectTransform.localPosition = new Vector3(-120.0f, 60.0f);
                        break;
                    case 1:
                        rectTransform.localPosition = new Vector3(-120.0f, -60.0f);
                        break;
                }
            }).AddTo(gameObject);

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && !isClick)
                {
                    se.ShotSE("select");
                    isClick = true;
                    //mManager.Choice(pos % 2, GameManager.Instance.CurrentState.Value);
                }
            }).AddTo(gameObject);
    }
}
