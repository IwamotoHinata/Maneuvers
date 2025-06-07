using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;

public class OriginsLeader : BasicLeader
{
    //�t�B�[���h�i�ϐ��j
    private RaycastHit[] hits; //�R���C�_�[�i�[�p
    private const float fieldDistance = 10000.0f; //�\���傫�ȋ����i�t�B�[���h���͂ށj
    private const float ultTimer = 15.0f; //�E���g����

    protected override void UseUlt()
    {
        //�S�I�u�W�F�N�g�擾
        // ��������
        hits = Physics.SphereCastAll(
            Vector3.zero,
            fieldDistance,
            Vector3.forward
        );

        foreach (var hit in hits)
        {
            //�G��Ԕ���
            bool enemy = false;
            if (hit.collider.gameObject.GetComponent<CharacterBaseData>())
            {
                enemy = hit.collider.gameObject.GetComponent<CharacterBaseData>().isHasInputAuthority();
            }

            //������ԕt�^
            if (enemy && hit.collider.gameObject.GetComponent<CharacterStatus>())
            {
                Debug.Log("Idiscovered");
                hit.collider.gameObject.GetComponent<CharacterStatus>().setVisibleTimer(ultTimer);
            }
        }
        Debug.Log("All Enemy Discover.");
        // �����܂Ł@���C��

        //������ԉ���
        Invoke(nameof(UltLimit), ultTimer);
    }

    private void UltLimit()
    {
        //������ԉ���
        // hits ��ύX
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