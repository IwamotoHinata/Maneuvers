using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace Unit
{
    public class DebuffZone : NetworkBehaviour
    {
        [Networked] private TickTimer _destroyTimer { get; set; }//�j�󂷂�܂ł̎���
        [Networked] private TickTimer _debuffInterval { get; set; }//�j�󂷂�܂ł̎���
        private Collider[] _enemys = new Collider[100];  //�󂯎��R���C�_�[
        private int _debuffCount;
        [SerializeField] private int _radius;
        

        public override void Spawned()
        {
            _debuffCount = 0;
            _debuffInterval = TickTimer.CreateFromSeconds(Runner, 0.1f);
            _destroyTimer = TickTimer.CreateFromSeconds(Runner, 12.5f);
        }
        
        
        public override void FixedUpdateNetwork()
        {
            //�f�o�t�֌W�̎��s
            if (_debuffInterval.Expired(Runner) && _debuffCount < 110)//11�b��0.1�b���ƂɃf�o�t�����邩�ۂ�������s���B
            {
                //�R���C�_�[�擾
                Physics.OverlapCapsuleNonAlloc(new Vector3(this.transform.position.x, this.transform.position.y - 50, this.transform.position.z),
                                               new Vector3(this.transform.position.x, this.transform.position.y + 50, this.transform.position.z),
                                               _radius, _enemys);//���Ƃ��Ƃ�15

                
                //�f�o�t�̎��s
                for (int i = 0; i < 100; i++)
                {
                    //�擾�����R���C�_�[��������ΏI��
                    if (_enemys[i] == null)
                    {
                        Debug.Log("�f�o�t�I��");
                        break;
                    }

                    //�f�o�t�̏���
                    //�����F�z��쒆��null�łȂ��AUnit�^�O������AInputAuthority��L���Ă��Ȃ�
                    if (_enemys[i] != null && _enemys[i].gameObject.tag == "Unit" && _enemys[i].gameObject.GetComponent<CharacterBaseData>().isHasInputAuthority() == false)
                    {
                        RPC_Debuff(i);
                    }
                }

                _debuffCount++;
                _debuffInterval = TickTimer.CreateFromSeconds(Runner, 0.1f);
            }

            //���̃G�t�F�N�g��j�󂳂���
            if (_destroyTimer.Expired(Runner))
            {
                Runner.Despawn(Object);
            }
                
        }

        //RPC�֐��̈�����Collider���g���Ȃ������̂ł��̂悤�ȏ����ɂ��Ă���
        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        public void RPC_Debuff(int num, RpcInfo info = default)
        {
            _enemys[num].gameObject.GetComponent<CharacterStatus>().Debuff();
            Debug.Log(_enemys[num].gameObject.name + "�Ƀf�o�t��t�^���܂���");
        }


    }
}
