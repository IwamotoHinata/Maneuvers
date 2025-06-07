using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButtons : MonoBehaviour
{
    [SerializeField] private GameObject GameSettingPanel;
    [SerializeField] private GameObject OperationSettingPanel;
    [SerializeField] private GameObject AudioSettingsPanel;
    [SerializeField] private GameObject StaffRollPanel;
    [SerializeField] private GameObject RibbonPanel;

    [SerializeField] private OptionManager optionManager;
    private void Start()
    {
        AllClear();
    }

    private void AllClear()
    {
        GameSettingPanel.SetActive(false);
        OperationSettingPanel.SetActive(false);
        AudioSettingsPanel.SetActive(false);
        StaffRollPanel.SetActive(false);
        RibbonPanel.SetActive(false);
    }
    public void ShowOptionPanel(int panelNum)
    {
        AllClear();
        RibbonPanel.SetActive(true);
        switch (panelNum)
        {
            case 0:
                GameSettingPanel.SetActive(true);
                break;
            case 1:
                OperationSettingPanel.SetActive(true);
                break;
            case 2:
                AudioSettingsPanel.SetActive(true);
                break;
            case 3:
                StaffRollPanel.SetActive(true);
                break;
            case 4:
                RibbonPanel.SetActive(false);
                break;
        }
    }

    public void OnKillLogButton(bool isKill)
    {
        optionManager.IsShowKillLog.Value = isKill;
    }

    public void OnFlagLogButton(bool isFlag)
    {
        optionManager.IsShowFlagLog.Value = isFlag;
    }

    public void OnShowHpNumButton(bool isHp)
    {
        optionManager.IsShowHpNum.Value = isHp;
    }

    public void OnShowARNumButton(bool isAR)
    {
        optionManager.IsShowARNum.Value = isAR;
    }

    public void OnMoveLineButton(bool isLine)
    {
        optionManager.IsShowMoveLine.Value = isLine;
    }
}
