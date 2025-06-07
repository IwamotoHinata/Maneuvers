using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class HeroSkill_offline : HeroSkillAbstract_offline
    {
        /// <summary>
        /// �p�b�V�u�X�L��
        /// </summary>
        public override void PassiveSkill()
        {
            Debug.Log("PassiveSkill()");
        }

        /// <summary>
        /// �A�N�e�B�u�X�L��
        /// </summary>
        public override void ActiveSkill()
        {
            Debug.Log("ActiveSkill()");
        }

        /// <summary>
        /// �A�N�e�B�u�X�L���Ď�
        /// </summary>
        /// <param name="skillTime"></param>
        /// <returns></returns>
        public override IEnumerator CheckUseSkill(float skillTime)
        {
            while (true)
            {
                //�����{�^��
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

                ActiveSkill();

                //�N�[���^�C��
                yield return new WaitForSeconds(skillTime);
            }
        }
    }
}
