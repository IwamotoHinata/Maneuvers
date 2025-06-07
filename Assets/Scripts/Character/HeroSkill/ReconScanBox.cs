using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public class ReconScanBox : NetworkBehaviour
    {
        [SerializeField] private LayerMask layerMask; //コライダーを取得するレイヤー
        [SerializeField]
        private Vector3 boxSize = new Vector3(300, 60, 600); //ボックスサイズ
        private const float activeTimer = 20.0f; //スキル効果時間

        /// <summary>
        /// 敵を視認
        /// </summary>
        public void ChangeEnemyVisible()
        {
            //コライダーの取得
            Collider[] hits = Physics.OverlapBox(transform.position,
                boxSize / 2,
                transform.rotation,
                layerMask);

            //敵の判定
            foreach (var unit in hits)
            {
                if (unit.gameObject.TryGetComponent<CharacterBaseData>(out var targetBaseData)
                    && unit.gameObject.TryGetComponent<CharacterStatus>(out var targetStatus))
                {
                    if (!targetBaseData.isHasInputAuthority()) //敵ユニット
                    {
                        //Debug.Log("敵ユニット発見");
                        RPC_ChangeVisible(targetStatus); //ホスト側で視界処理
                    }
                }
            }
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void RPC_ChangeVisible(CharacterStatus targetStatus)
        {
            targetStatus.Idiscovered(true);
            targetStatus.setVisibleTimer(activeTimer);
        }

        //OverlapBoxのデバッグ用
        void OnDrawGizmos()
        {
            // 可視化用の行列を設定
            Matrix4x4 matrix = Matrix4x4.TRS(transform.position, transform.rotation, boxSize);

            // 現在のGizmosの行列を保存して新しい行列を設定
            Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
            Gizmos.matrix *= matrix;

            // 可視化
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

            // 行列を元に戻す
            Gizmos.matrix = oldGizmosMatrix;
        }
    }
}
