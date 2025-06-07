using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Unit;

public class SwitchOnOffButton : MonoBehaviour
{
    [SerializeField] private Button onButton;
    [SerializeField] private Button offButton;

    [SerializeField] private EnumOptions _enumOptions;
    [SerializeField] private OptionManager _optionManager;
    private void OnEnable()
    {
        onButton.onClick.AddListener(ClickOn);
        offButton.onClick.AddListener(ClickOff);
        SelectButton();
    }

    void ClickOn()
    {
        onButton.image.color = Color.white;
        offButton.image.color = Color.gray;
    }

    void ClickOff()
    {
        offButton.image.color = Color.white;
        onButton.image.color = Color.gray;
    }

    private void SelectButton()
    {
        switch (_enumOptions)
        {
            case EnumOptions.KillLog:
                bool kill = (_optionManager.IsShowKillLog.Value) ? onButton : offButton;
                return;
            case EnumOptions.FlagLog:
                bool flag = (_optionManager.IsShowFlagLog.Value) ? onButton : offButton;
                return;
            case EnumOptions.UnitHp:
                bool hp = (_optionManager.IsShowHpNum.Value) ? onButton : offButton;
                return;
            case EnumOptions.UnitAR:
                bool ar = (_optionManager.IsShowARNum.Value) ? onButton : offButton;
                return;
            case EnumOptions.MoveLine:
                bool line = (_optionManager.IsShowMoveLine.Value) ? onButton : offButton;
                return;
        }
    }
}

enum EnumOptions
{
    KillLog,
    FlagLog,
    UnitHp,
    UnitAR,
    MoveLine,
}