using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class HeroSkill_offline : HeroSkillAbstract_offline
    {
        /// <summary>
        /// パッシブスキル
        /// </summary>
        public override void PassiveSkill()
        {
            Debug.Log("PassiveSkill()");
        }

        /// <summary>
        /// アクティブスキル
        /// </summary>
        public override void ActiveSkill()
        {
            Debug.Log("ActiveSkill()");
        }

        /// <summary>
        /// アクティブスキル監視
        /// </summary>
        /// <param name="skillTime"></param>
        /// <returns></returns>
        public override IEnumerator CheckUseSkill(float skillTime)
        {
            while (true)
            {
                //発動ボタン
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

                ActiveSkill();

                //クールタイム
                yield return new WaitForSeconds(skillTime);
            }
        }
    }
}
