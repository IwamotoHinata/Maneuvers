using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    /// <summary>
    /// ���C���[��e�q�̃I�u�W�F�N�g���ׂĂɓK��
    /// </summary>
    /// <param name="self"></param>
    /// <param name="layer"></param>
    public static void SetLayerToDown(this GameObject self, string layer)
    {
        //���g�̃��C���[��ύX
        self.layer = LayerMask.NameToLayer(layer);

        //�q�I�u�W�F�N�g�̃��C���[��ύX
        foreach (Transform no in self.transform)
        {
            SetLayerToDown(no.gameObject, layer);
        }
    }
}
