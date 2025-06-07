using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public class ReconScanBox : NetworkBehaviour
    {
        [SerializeField] private LayerMask layerMask; //�R���C�_�[���擾���郌�C���[
        [SerializeField]
        private Vector3 boxSize = new Vector3(300, 60, 600); //�{�b�N�X�T�C�Y
        private const float activeTimer = 20.0f; //�X�L�����ʎ���

        /// <summary>
        /// �G�����F
        /// </summary>
        public void ChangeEnemyVisible()
        {
            //�R���C�_�[�̎擾
            Collider[] hits = Physics.OverlapBox(transform.position,
                boxSize / 2,
                transform.rotation,
                layerMask);

            //�G�̔���
            foreach (var unit in hits)
            {
                if (unit.gameObject.TryGetComponent<CharacterBaseData>(out var targetBaseData)
                    && unit.gameObject.TryGetComponent<CharacterStatus>(out var targetStatus))
                {
                    if (!targetBaseData.isHasInputAuthority()) //�G���j�b�g
                    {
                        //Debug.Log("�G���j�b�g����");
                        RPC_ChangeVisible(targetStatus); //�z�X�g���Ŏ��E����
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

        //OverlapBox�̃f�o�b�O�p
        void OnDrawGizmos()
        {
            // �����p�̍s���ݒ�
            Matrix4x4 matrix = Matrix4x4.TRS(transform.position, transform.rotation, boxSize);

            // ���݂�Gizmos�̍s���ۑ����ĐV�����s���ݒ�
            Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
            Gizmos.matrix *= matrix;

            // ����
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

            // �s������ɖ߂�
            Gizmos.matrix = oldGizmosMatrix;
        }
    }
}
