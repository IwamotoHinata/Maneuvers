using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class UnitView_Offline : MonoBehaviour
    {
        [SerializeField] private Slider hpSlider;
        [SerializeField] private Slider skillSlider;
        [SerializeField] private Image skillIcon;
        private float nowSkillCoolTime;
        
        private void Start()
        {
            //UseSkill();
        }

        public void SetHp(float value)
        {
            hpSlider.value = value;
        }

        public void SetSkill(float coolTime)
        {
            StartCoroutine(SkillCoolTime(coolTime));
            UseSkill();

        }

        IEnumerator SkillCoolTime(float coolTime)
        {
            nowSkillCoolTime = 0;
            while (coolTime >= nowSkillCoolTime)
            {
                yield return new WaitForSeconds(1.0f);
                nowSkillCoolTime += 1;
                skillSlider.value = nowSkillCoolTime / coolTime;
            }
            CanUseSkill();
            yield break;
        }

        private void UseSkill()
        {
            nowSkillCoolTime = 0;
            skillSlider.value = 0;
            skillIcon.color = Color.gray;
        }
        private void CanUseSkill()
        {
            skillSlider.value = 1;
            skillIcon.color = Color.white;
        }
    }
}

