using System.Collections;
using System.Collections.Generic;
using Fusion;
using Online;
using UnityEngine;
using UniRx;
using Unit;

public class GuardianLeaderSkill : MonoBehaviour
{
    private CharacterStatus MyCharacterStatus;
    private LeaderInterface MyLeaderInterface;
    private TeamType MyTeamType;
    [SerializeField] private GameObject GurdianLeaderUltPrefab;
    private NetworkObject GurdianLeaderUlt;
    private float deultPoint = 300;
    [SerializeField] private float radius_GurdianLeaderUlt; // �~�̔��a
    private Vector3 center_GurdianLeaderUlt; // �~�̒��S�_
    [SerializeField] private float waitStartTime; // �E���g�J�n�҂�����
    [SerializeField] private float gurdianLeaderultTime; // �E���g���ʎ���
    private float countTime = 1000;
    // �E���g�Ԋu
    [SerializeField] private float minUlttimeOut;
    [SerializeField] private float maxUlttimeOut;
    [SerializeField] private float ulttimeOut;
    private float timeElapsed;

    void Start()
    {
        MyLeaderInterface = GetComponent<LeaderInterface>();

        MyCharacterStatus = GetComponent<CharacterStatus>();
    }

    void Update()
    {
        LeaderSkill();
    }

    void LeaderSkill()
    {
        if (Input.GetMouseButtonDown(0) && MyLeaderInterface.ultPoint >= 300)
        {
            MyLeaderInterface.UseUltPoint(deultPoint);//�E���g�|�C���g�g�p
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            center_GurdianLeaderUlt = hit.point; //�E���g�̈ʒu������
            MyTeamType = MyCharacterStatus.GetTeamType;//�G�����������f����
            // �E���g���Ԃ̌v��
            countTime = 0.0f;
            ulttimeOut = maxUlttimeOut;

            if (countTime < gurdianLeaderultTime)
            {
                countTime += Time.deltaTime;
                if (countTime > waitStartTime)
                {
                    // �����J�n
                    MakeUlt();
                }
            }
        }
    }
    private void MakeUlt()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= ulttimeOut)
        {
            // �w�肳�ꂽ���a�̉~���̃����_���ʒu���擾
            var circlePos = radius_GurdianLeaderUlt * Random.insideUnitCircle;

            // XZ���ʂŎw�肳�ꂽ���a�A���S�_�̉~���̃����_���ʒu���v�Z
            var spawnPos = new Vector3(circlePos.x, 0, circlePos.y) + center_GurdianLeaderUlt;

            // Prefab��ǉ�
            GurdianLeaderUlt = GameLauncher.Runner.Spawn(GurdianLeaderUltPrefab, spawnPos, Quaternion.identity, RoomPlayer.Local.Object.InputAuthority);
            GurdianLeaderUlt.GetComponent<DamageArea>().MyOwnerTypeSet(MyTeamType);
            if (ulttimeOut > minUlttimeOut)
            {
                ulttimeOut -= (maxUlttimeOut - minUlttimeOut) / gurdianLeaderultTime;
            }

            timeElapsed = 0.0f;
        }
    }
}
