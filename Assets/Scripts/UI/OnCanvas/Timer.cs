using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Timer : MonoBehaviour
{
    private readonly ReactiveProperty<int> _time = new ReactiveProperty<int>(600);//�����l10��
    public IReadOnlyReactiveProperty<int> Time => _time;

    private void Start()
    {
        _time.AddTo(this);
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(5.0f);//�ŏ��̃��j�b�g���N������܂ł̎���(5s)
        while (_time.Value > 0)
        {
            yield return new WaitForSeconds(1.0f);
            _time.Value -= 1;
        }
    }
}
