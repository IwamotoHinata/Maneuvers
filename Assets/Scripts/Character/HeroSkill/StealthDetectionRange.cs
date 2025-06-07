using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;

public class StealthDetectionRange : MonoBehaviour
{
    //�t�B�[���h
    private SphereCollider col_detectionRange; //�G���m�R���C�_�[
    private int unitNum = 0; //�͈͓��̃��j�b�g��
    private const int detectionRadius = 10; //���m�͈͂P�O��

    // Start is called before the first frame update
    void Start()
    {
        //�R���|�[�l���g�ݒ�
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

    //�ϐ��A�N�Z�X
    public int getUnitNum()
    {
        return unitNum;
    }
}
