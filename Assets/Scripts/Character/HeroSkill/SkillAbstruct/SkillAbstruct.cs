using Fusion;
namespace Unit
{ 
    public abstract class SkillAbstruct : NetworkBehaviour
    {
        public abstract void ActiveSkill();//スキルの内容を記述
        public abstract void PassiveSkill();//アクティブスキルの内容を記述
    }
}
