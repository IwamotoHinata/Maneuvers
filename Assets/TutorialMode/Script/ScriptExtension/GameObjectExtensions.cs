using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    /// <summary>
    /// レイヤーを親子のオブジェクトすべてに適応
    /// </summary>
    /// <param name="self"></param>
    /// <param name="layer"></param>
    public static void SetLayerToDown(this GameObject self, string layer)
    {
        //自身のレイヤーを変更
        self.layer = LayerMask.NameToLayer(layer);

        //子オブジェクトのレイヤーを変更
        foreach (Transform no in self.transform)
        {
            SetLayerToDown(no.gameObject, layer);
        }
    }
}
