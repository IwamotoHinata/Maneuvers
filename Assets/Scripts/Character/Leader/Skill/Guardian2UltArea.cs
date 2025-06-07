using UnityEngine;
using Unit;

public class Guardian2UltArea : MonoBehaviour
{
    private TeamType MyTeamType;
    private void OnTriggerEnter(Collider other)
    {
        GuadianLeader2SkillEffect(other.gameObject, true);
    }

    private void OnTriggerExit(Collider other)
    {
        GuadianLeader2SkillEffect(other.gameObject, false);
    }
    private void GuadianLeader2SkillEffect(GameObject other, bool SkillState)
    {
        if (other.TryGetComponent<CharacterBaseData>(out var cbd) && other.TryGetComponent<CharacterStatus>(out var CS))
        {
            //�����Ȃ�
            if (MyTeamType == CS.GetTeamType)
            {
                CS.GuadianChangeMoveSpeed(SkillState);
            }
            
            else
            {
                //�~�j�I���Ȃ�
                if (other.GetComponent<MinionMove>())
                    CS.GuadianChangeHitRate(25, SkillState);
                //���j�b�g�Ȃ�
                else
                    CS.GuadianChangeHitRate(15, SkillState);
            }
        }
    }

    public void MyOwnerTypeSet(TeamType type)
    {
        MyTeamType = type;
        //Debug.Log(MyTeamType);
    }
}
