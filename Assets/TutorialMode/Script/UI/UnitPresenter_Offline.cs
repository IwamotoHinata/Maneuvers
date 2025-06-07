using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;

namespace Tutorial
{
    public class UnitPresenter_Offline : MonoBehaviour
    {
        [SerializeField] private UnitView_Offline unitView;

        private UnitStatus_offline _unitStatus;
        private BasicHeroSkill_offline _unitSkill;

        [SerializeField] private TextManager_Offline _textManager;

        public void OnCreateUnit(GameObject unit)
        {
            _unitStatus = unit.GetComponent<UnitStatus_offline>();
            _unitStatus.HP.Subscribe(x =>
            {
                Debug.Log(x);
                unitView.SetHp(x / unit.GetComponent<UnitBaseData_offline>().hp);
            }).AddTo(this);
            
            _unitSkill = unit.GetComponent<BasicHeroSkill_offline>();
            _unitSkill.skillObservable_offline.Subscribe(x =>
            {
                unitView.SetSkill(x);
            }).AddTo(this);
        }
    }
}