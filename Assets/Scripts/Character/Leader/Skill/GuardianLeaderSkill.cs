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
    [SerializeField] private float radius_GurdianLeaderUlt; // 円の半径
    private Vector3 center_GurdianLeaderUlt; // 円の中心点
    [SerializeField] private float waitStartTime; // ウルト開始待ち時間
    [SerializeField] private float gurdianLeaderultTime; // ウルト効果時間
    private float countTime = 1000;
    // ウルト間隔
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
            MyLeaderInterface.UseUltPoint(deultPoint);//ウルトポイント使用
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            center_GurdianLeaderUlt = hit.point; //ウルトの位置を決定
            MyTeamType = MyCharacterStatus.GetTeamType;//敵か味方か判断する
            // ウルト時間の計測
            countTime = 0.0f;
            ulttimeOut = maxUlttimeOut;

            if (countTime < gurdianLeaderultTime)
            {
                countTime += Time.deltaTime;
                if (countTime > waitStartTime)
                {
                    // 爆撃開始
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
            // 指定された半径の円内のランダム位置を取得
            var circlePos = radius_GurdianLeaderUlt * Random.insideUnitCircle;

            // XZ平面で指定された半径、中心点の円内のランダム位置を計算
            var spawnPos = new Vector3(circlePos.x, 0, circlePos.y) + center_GurdianLeaderUlt;

            // Prefabを追加
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
