using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;

public class OriginsLeader : BasicLeader
{
    //フィールド（変数）
    private RaycastHit[] hits; //コライダー格納用
    private const float fieldDistance = 10000.0f; //十分大きな距離（フィールドを囲む）
    private const float ultTimer = 15.0f; //ウルト時間

    protected override void UseUlt()
    {
        //全オブジェクト取得
        // ここから
        hits = Physics.SphereCastAll(
            Vector3.zero,
            fieldDistance,
            Vector3.forward
        );

        foreach (var hit in hits)
        {
            //敵状態判別
            bool enemy = false;
            if (hit.collider.gameObject.GetComponent<CharacterBaseData>())
            {
                enemy = hit.collider.gameObject.GetComponent<CharacterBaseData>().isHasInputAuthority();
            }

            //発見状態付与
            if (enemy && hit.collider.gameObject.GetComponent<CharacterStatus>())
            {
                Debug.Log("Idiscovered");
                hit.collider.gameObject.GetComponent<CharacterStatus>().setVisibleTimer(ultTimer);
            }
        }
        Debug.Log("All Enemy Discover.");
        // ここまで　を修正

        //発見状態解除
        Invoke(nameof(UltLimit), ultTimer);
    }

    private void UltLimit()
    {
        //発見状態解除
        // hits を変更
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.GetComponent<CharacterStatus>())
            {
                Debug.Log("noIdiscovered");
                hit.collider.gameObject.GetComponent<CharacterStatus>().Idiscovered(false);
            }
        }
    }
}