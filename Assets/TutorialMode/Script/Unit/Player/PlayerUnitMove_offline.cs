using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Tutorial
{
    public class PlayerUnitMove_offline : MonoBehaviour, IUpdated, IUnitMove_offline
    {
        private Camera _mainCamera;
        private NavMeshAgent _agent;
        private UnitStatus_offline MyUnitStatus;
        private List<Vector3> _bookPos = new List<Vector3>(); //�\�񂵂����W
        private List<UnitState_offline> _bookState = new List<UnitState_offline>(); //�\�񂵂����

        void Start()
        {
            //UpdateManager
            GameObject.Find("UpdateManager").
                GetComponent<UpdateManager_offline>().
                upd.Add(this);

            //�R���|�[�l���g�ݒ�
            _mainCamera = Camera.main;
            MyUnitStatus = GetComponent<UnitStatus_offline>();
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = MyUnitStatus.moveSpeed;
        }

        public bool UpdateRequest() { return MyUnitStatus.iActive; }
        public void Updated()
        {
            SetBookPos(); //���W�\��
            isArrive(); //���B����
        }

        /// <summary>
        /// ���W�̗\��
        /// </summary>
        private void SetBookPos()
        {
            //�E�}�E�X�{�^��&���j�b�g�I�����
            if (Input.GetMouseButtonDown(1) && MyUnitStatus.iSelected)
            {
                //���C���΂��ă}�E�X���W���m��
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (_bookPos.Count > 0) //�\����W��1�ȏ゠��Ƃ�
                    {
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            AddBook(hit.point);
                        }
                        else
                        {
                            //�\�񃊃X�g�����Z�b�g���A�V���ȍ��W��o�^
                            RemoveBook("all");
                            AddBook(hit.point);
                            ToBook();
                        }
                    }
                    else if (_bookPos.Count == 0) //�\����W���Ȃ��Ƃ�
                    {
                        //�����o��
                        AddBook(hit.point);
                        ToBook();
                        MyUnitStatus.SetIMove(true);
                    }
                }
            }
        }

        /// <summary>
        /// ���W�ւ̓����𔻒�
        /// </summary>
        private void isArrive()
        {
            if (_bookPos.Count >= 1) //�\����W����ȏ゠�鎞
            {
                //���ʏ�̋����v�Z
                Vector2 myPos = new Vector2(transform.position.x, transform.position.z);
                Vector2 targetPos = new Vector2(_bookPos[0].x, _bookPos[0].z);
                float distancePlane = Vector2.Distance(myPos, targetPos);

                //�ړI�n�ɓ���
                if (distancePlane < 1f)
                {
                    if (_bookPos.Count > 1) //�\����W����ȏ゠�鎞
                    {
                        RemoveBook("zero");
                        ToBook();
                    }
                    else //�\����W�����
                    {
                        RemoveBook("zero");
                        MyUnitStatus.SetIMove(false);
                        MyUnitStatus.SetIMoveState(UnitState_offline.Idle);
                    }
                }
            }
        }

        /// <summary>
        /// �\��̒ǉ�
        /// </summary>
        /// <param name="pos"></param>
        private void AddBook(Vector3 pos)
        {
            _bookPos.Add(pos);
            if (Input.GetKey(KeyCode.Q)) _bookState.Add(UnitState_offline.VigilanceMove);
            else _bookState.Add(UnitState_offline.Move);
        }

        /// <summary>
        /// �\��̍폜
        /// </summary>
        /// <param name="setting"></param>
        private void RemoveBook(string setting)
        {
            if (setting == "zero")
            {
                _bookPos.RemoveAt(0);
                _bookState.RemoveAt(0);
            }
            else if (setting == "all")
            {
                _bookPos.Clear();
                _bookState.Clear();
            }
        }

        /// <summary>
        /// �ړI�n�ւ̈ړ�
        /// </summary>
        private void ToBook()
        {
            _agent.SetDestination(_bookPos[0]);
            MyUnitStatus.SetIMoveState(_bookState[0]);
        }

        /// <summary>
        /// �ړ���~
        /// </summary>
        public void isStop()
        {
            _bookPos.Clear();
            _bookState.Clear();
            _agent.SetDestination(transform.position);
            MyUnitStatus.SetIMove(false);
            MyUnitStatus.SetIMoveState(UnitState_offline.Idle);
        }
    }
}
