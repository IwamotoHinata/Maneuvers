using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;

public class StealthDetectionRange : MonoBehaviour
{
    //フィールド
    private SphereCollider col_detectionRange; //敵検知コライダー
    private int unitNum = 0; //範囲内のユニット数
    private const int detectionRadius = 10; //検知範囲１０ｍ

    // Start is called before the first frame update
    void Start()
    {
        //コンポーネント設定
        gameObject.AddComponent<SphereCollider>();
        gameObject.AddComponent<Rigidbody>();
        GetComponent<Rigidbody>().isKinematic = true;
        col_detectionRange = GetComponent<SphereCollider>();
        col_detectionRange.isTrigger = true;
        col_detectionRange.radius = detectionRadius;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Unit" &&
            collider.gameObject.GetComponent<CharacterBaseData>())
        {
            //Debug.Log("detectUnit");
            if (!collider.gameObject.GetComponent<CharacterBaseData>().isHasInputAuthority())
            {
                unitNum++;
                //Debug.Log("unitNum : " + unitNum);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Unit" &&
            collider.gameObject.GetComponent<CharacterBaseData>())
        {
            //Debug.Log("detectUnit");
            if (!collider.gameObject.GetComponent<CharacterBaseData>().isHasInputAuthority())
            {
                unitNum--;
                //Debug.Log("unitNum : " + unitNum);
            }
        }
    }

    //変数アクセス
    public int getUnitNum()
    {
        return unitNum;
    }
}
