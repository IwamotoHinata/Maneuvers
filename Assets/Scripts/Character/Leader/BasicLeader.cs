using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Online;
using UniRx;
using UnityEngine;

public abstract class BasicLeader : MonoBehaviour
{
    [SerializeField]
    protected readonly ReactiveProperty<float> ultPoint = new ReactiveProperty<float>();

    public IReactiveProperty<float> UltPoint => ultPoint; //リーダースキルのポイント
    public float maxUltPoint = 300; 
    private float time;
    private float Limittime;

    protected LeaderObserver LeaderObserver;

    public void setLeaderObserver(LeaderObserver observer) { LeaderObserver = observer; }

    protected virtual void UseUlt()
    {
        
    }
    void AddUltPoint()
    {
        if (ultPoint.Value <= maxUltPoint)
        {
            ultPoint.Value += 1;
            time = 0.0f;
        }
    }

    public void ReduceUltPoint()
    {
        if (ultPoint.Value > 0)
        {
            ultPoint.Value -= 1;
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(AddUltPoint), 0, 1);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Z))
        {
            if (ultPoint.Value >= maxUltPoint)
            {
                ultPoint.Value -= maxUltPoint;
                Debug.Log("Ult");

                UseUlt();
                SoundManager.Instance.shotSe(SeType.LeaderSkill);
            }
            else
            {
                Debug.Log("No UltPoint");
            }
        }
    }
}