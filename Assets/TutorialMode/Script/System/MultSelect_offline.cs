using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class MultSelect_offline : MonoBehaviour
    {
        private Camera mainCamera;
        private Vector3 selectStartPos; //�͈͎w��
        private Vector3 selectEndPos;

        void Start()
        {
            //�R���|�[�l���g�ݒ�
            mainCamera = Camera.main;

            //�����ݒ�
            StartCoroutine(Select());
        }

        /// <summary>
        /// ���j�b�g�͈͑I��
        /// </summary>
        /// <returns></returns>
        private IEnumerator Select()
        {
            while (true)
            {
                //�͈͑I���̊J�n�n�_
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
                selectStartPos = Input.mousePosition;

                //�͈͑I���̏I���n�_
                yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Mouse0));
                selectEndPos = Input.mousePosition;

                //Debug.Log(selectStartPos);
                //Debug.Log(selectEndPos);

                //������x�͈̔͑I��
                if (SelectAreaValue() > 10)
                {
                    //�͈͓����j�b�g�̔���
                    var UnitObjects = FindObjectsOfType<UnitBaseData_offline>(); //�V�[�����UnitBaseData.cs�����ׂĎ��
                    List<UnitStatus_offline> SelectedObjects = new List<UnitStatus_offline>();
                    foreach (var no in UnitObjects)
                    {
                        if (InSelectArea(no.gameObject))
                        {
                            //Debug.Log("�I��͈͓��Ƀ��j�b�g������");

                            if (no.isHasInputAuthority())
                            {
                                //Debug.Log("�v���C���[���������Ă��郆�j�b�g�ł���");

                                //���̃��j�b�g��I�𒆃��j�b�g�ɒǉ�
                                var UnitStatus_cs = no.GetComponent<UnitStatus_offline>();
                                UnitStatus_cs.SetISelected(true);
                                SelectedObjects.Add(UnitStatus_cs);
                            }
                        }
                    }

                    //���j�b�g�̑I������
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
                    foreach (var no in SelectedObjects)
                    {
                        no.SetISelected(false);
                    }
                }
            }
        }

        /// <summary>
        /// �͈͑I���̑傫��
        /// </summary>
        /// <returns></returns>
        private float SelectAreaValue()
        {
            float xRange = selectStartPos.x - selectEndPos.x;
            float yRange = selectStartPos.y - selectEndPos.y;
            float value = Mathf.Abs(xRange * yRange);
            return value;
        }

        /// <summary>
        /// �I�u�W�F�N�g���X�N���[���͈͎w������𔻒�
        /// </summary>
        /// <param name="targetObj"></param>
        /// <returns></returns>
        private bool InSelectArea(GameObject targetObj)
        {
            //�I�u�W�F�N�g�̃��[���h���W���X�N���[�����W�ɕϊ�
            var screenPos = mainCamera.WorldToScreenPoint(targetObj.transform.position);

            //�X�N���[���͈͓�����
            if (((screenPos.x >= selectStartPos.x
                && screenPos.x <= selectEndPos.x)
                || (screenPos.x <= selectStartPos.x
                && screenPos.x >= selectEndPos.x))
                && ((screenPos.y >= selectStartPos.y
                && screenPos.y <= selectEndPos.y)
                || (screenPos.y <= selectStartPos.y
                && screenPos.y >= selectEndPos.y)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
