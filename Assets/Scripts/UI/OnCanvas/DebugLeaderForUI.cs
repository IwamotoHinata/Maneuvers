using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class DebugLeaderForUI : MonoBehaviour
{
    public ReactiveProperty<float> UltPoint { get; } = new ReactiveProperty<float>(0);

    public float maxUltPoint;

    private void Start()
    {
        StartCoroutine(GainUltForRandom());
    }

    IEnumerator GainUltForRandom()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            UltPoint.Value += 10.0f;
        }
    }
}