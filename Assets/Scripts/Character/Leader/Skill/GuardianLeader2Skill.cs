using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Unit;

public class GuardianLeader2Skill : MonoBehaviour
{
    private CharacterStatus MyCharacterStatus;
    private LeaderInterface MyLeaderInterface; 
    private TeamType MyTeamType;
    [SerializeField] private GameObject SkillAreaPrefab;
    private GameObject MySkillArea;
    private float deultPoint = 200;
    // Start is called before the first frame update
    void Start()
    {
        MyLeaderInterface = GetComponent<LeaderInterface>();

        MyCharacterStatus = GetComponent<CharacterStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        LeaderSkill();
    }

    void LeaderSkill()
    {
        if (Input.GetMouseButtonDown(0)) // && MyLeaderInterface.ultPoint >= 200)
        {
            MyLeaderInterface.UseUltPoint(deultPoint);//�E���g200����
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            MySkillArea = Instantiate(SkillAreaPrefab, hit.point, Quaternion.identity);
            MyTeamType = MyCharacterStatus.GetTeamType;//�G�����������f����
            MySkillArea.GetComponent<Guardian2UltArea>().MyOwnerTypeSet(MyTeamType); //�X�L���G���A���ǂ���̃`�[���̂��̂�����
        }
    }
}
