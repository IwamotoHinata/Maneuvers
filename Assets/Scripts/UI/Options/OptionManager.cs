using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    #region ゲーム内設定

    private readonly ReactiveProperty<bool> _isShowKillLog = new ReactiveProperty<bool>(true);
    public IReactiveProperty<bool> IsShowKillLog => _isShowKillLog;
    private readonly ReactiveProperty<bool> _isShowFlagLog = new ReactiveProperty<bool>(true);
    public IReactiveProperty<bool> IsShowFlagLog => _isShowFlagLog;
    private readonly ReactiveProperty<bool> _isShowHpNum = new ReactiveProperty<bool>(true);
    public IReactiveProperty<bool> IsShowHpNum => _isShowHpNum;
    private readonly ReactiveProperty<bool> _isShowARNum = new ReactiveProperty<bool>(true);
    public IReactiveProperty<bool> IsShowARNum => _isShowARNum;
    private readonly ReactiveProperty<bool> _isShowMoveLine = new ReactiveProperty<bool>(true);
    public IReactiveProperty<bool> IsShowMoveLine => _isShowMoveLine;

    public void KillLog(bool isKillLog)
    {
        _isShowKillLog.Value = isKillLog;
        if (isKillLog)
        {
            Debug.Log("killTrue");
            return;
        }

        Debug.Log("killFalse");
    }

    public void FlagLog(bool isFlag)
    {
        _isShowFlagLog.Value = isFlag;
        if (isFlag)
        {
            return;
        }
    }

    public void ShowHpNum(bool isHp)
    {
        _isShowHpNum.Value = isHp;
        if (isHp)
        {
            return;
        }
    }

    public void ShowARNum(bool isAR)
    {
        _isShowARNum.Value = isAR;
        if (isAR)
        {
            return;
        }
    }

    public void ShowMoveLine(bool isLine)
    {
        _isShowMoveLine.Value = isLine;
        if (isLine)
        {
            return;
        }
    }

    public void ChangeUserName()
    {
    }

    #endregion

    #region 操作設定

    #endregion

    #region 音量調節

    #endregion

    #region スタッフロール

    #endregion

}