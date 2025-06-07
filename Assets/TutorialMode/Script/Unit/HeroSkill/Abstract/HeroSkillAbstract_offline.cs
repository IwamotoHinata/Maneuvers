using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public abstract class HeroSkillAbstract_offline : MonoBehaviour
    {
        public abstract void PassiveSkill();
        public abstract void ActiveSkill();
        public abstract IEnumerator CheckUseSkill(float skillTime);
    }
}
