using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Unit;


namespace Online
{
    public class LeaderManager : NetworkBehaviour
    {
        private Dictionary<PlayerRef, NetworkObject> LeaderDic = new Dictionary<PlayerRef, NetworkObject>();    //InputAuthority�ƊeLeader��R�Â������m
        [SerializeField]
        private GameObject HostObject;  //�J���e�B�X�g���[�_�[
        [SerializeField]
        private GameObject ClientObject;    //�I���W�����[�_�[
        private CharacterMove _characterMove;
        private void Start()
        {
            //���[�_�[�I�u�W�F�N�g�𐶐�
            if (GameLauncher.Runner.GameMode == GameMode.Host)
            {
                foreach (var leader in RoomPlayer.Players)
                {
                    SpawnLeader(leader, HostObject); //�����ɃJ���e�B�X�g���[�_�[���X�|�[��
                }
            }

        }

        private void SpawnLeader(RoomPlayer leader, GameObject LeaderObject)
        {
            /*LeaderDic.Add(  //LeaderObject��o�^���Đ���   
                leader.Object.InputAuthority,
                GameLauncher.Runner.Spawn(  //�z�X�g�̃��[�_�[�𐶐�
                    LeaderObject,
                    this.gameObject.transform.position,
                    Quaternion.identity,
                    leader.Object.InputAuthority
                )
            );*/
            NetworkObject Temp = GameLauncher.Runner.Spawn(  //�z�X�g�̃��[�_�[�𐶐�
                                    LeaderObject,
                                    this.gameObject.transform.position,
                                    Quaternion.identity,
                                    leader.Object.InputAuthority
                                 );

            LeaderDic.Add(leader.Object.InputAuthority, Temp);
        }

        public void SetCharacter(PlayerRef player, GameObject myUnit)
        {
            if (LeaderDic[player].tag == "CultistLeader")
            {  //�I�������v���C���[�̃��[�_�[���J���e�B�X�g�Ȃ�
                //LeaderDic[player].GetComponent<CultistLeader>().SetMySelectedCharacter(myUnit); //�I������Unit��Set
                Debug.Log("set");
            }

        }

        public void RemoveCharacter(PlayerRef player)
        {
            if (LeaderDic[player].tag == "CultistLeader")  //�I�������v���C���[�̃��[�_�[���J���e�B�X�g�Ȃ�
                LeaderDic[player].GetComponent<CultistLeader>().RemoveMySelectedCharacter(); //�I������Unit��Remove
        }

        public void KilledEnemyLeaderCharcter(PlayerRef player) //�G���L�������Ƃ���
        {
            if (LeaderDic[player].tag == "CultistLeader")   //�L�������̂��J���e�B�X�g���[�_�[�Ȃ�
                LeaderDic[player].GetComponent<CultistLeader>().PassiveSkill(); //�p�b�V�u�X�L������


        }
    }
}
