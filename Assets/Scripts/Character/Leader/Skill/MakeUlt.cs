using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeUlt : MonoBehaviour
{
    [SerializeField] private float ultPoint; //ウルトポイント
    [SerializeField] private float maxUltPoint; //ウルトポイント上限
    private float time; 
    [SerializeField] private float maxtime; //チャージ時間

    // 円の半径
    [SerializeField] private float radius_Ult1;

    // 円の中心点
    [SerializeField] private Vector3 center_Ult1;

    // 配置するPrefab
    [SerializeField] private GameObject prefab_Ult1;

    // 待ち時間
    [SerializeField] private float waitStartTime;

    // 効果時間
    [SerializeField] private float ult1Time;

    [SerializeField] private float countTime = 1000;

    // ウルト間隔
    [SerializeField] private float minUlttimeOut;

    [SerializeField] private float maxUlttimeOut;

    [SerializeField] private float ulttimeOut;
    
    private float timeElapsed;


    private void Start()
    {
        
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if (ultPoint < maxUltPoint && time <= 0.0)
        {
            ultPoint += 1;
            time = maxtime;
        }

        if (Input.GetMouseButtonDown(0) && ultPoint >= maxUltPoint)
        {
            ultPoint = 0;
            // ウルトの位置指定
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10.0f;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint( mousePos );
            center_Ult1.x = worldPos.x;
            center_Ult1.z = worldPos.z;

            // ウルト時間の計測
            countTime = 0.0f;

            ulttimeOut = maxUlttimeOut;
        }

        if (countTime < ult1Time)
        {
            countTime += Time.deltaTime;
            if (countTime > waitStartTime)
            {
                // ウルト起動
                MakeUlt1();
            }
        }
    }

    private void MakeUlt1()
    {
        timeElapsed += Time.deltaTime;

        if(timeElapsed >= ulttimeOut) 
        {
            // 指定された半径の円内のランダム位置を取得
            var circlePos = radius_Ult1 * Random.insideUnitCircle;

            // XZ平面で指定された半径、中心点の円内のランダム位置を計算
            var spawnPos = new Vector3(circlePos.x, 0, circlePos.y) + center_Ult1;

            // Prefabを追加
            Instantiate(prefab_Ult1, spawnPos, Quaternion.identity);

            if (ulttimeOut > minUlttimeOut)
            {
                ulttimeOut -= (maxUlttimeOut - minUlttimeOut) / ult1Time;
            }

            timeElapsed = 0.0f;
        }
    }
}
