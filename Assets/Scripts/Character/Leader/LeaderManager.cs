using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Unit;


namespace Online
{
    public class LeaderManager : NetworkBehaviour
    {
        private Dictionary<PlayerRef, NetworkObject> LeaderDic = new Dictionary<PlayerRef, NetworkObject>();    //InputAuthorityと各Leaderを紐づけたモノ
        [SerializeField]
        private GameObject HostObject;  //カルティストリーダー
        [SerializeField]
        private GameObject ClientObject;    //オリジンリーダー
        private CharacterMove _characterMove;
        private void Start()
        {
            //リーダーオブジェクトを生成
            if (GameLauncher.Runner.GameMode == GameMode.Host)
            {
                foreach (var leader in RoomPlayer.Players)
                {
                    SpawnLeader(leader, HostObject); //両方にカルティストリーダーをスポーン
                }
            }

        }

        private void SpawnLeader(RoomPlayer leader, GameObject LeaderObject)
        {
            /*LeaderDic.Add(  //LeaderObjectを登録して生成   
                leader.Object.InputAuthority,
                GameLauncher.Runner.Spawn(  //ホストのリーダーを生成
                    LeaderObject,
                    this.gameObject.transform.position,
                    Quaternion.identity,
                    leader.Object.InputAuthority
                )
            );*/
            NetworkObject Temp = GameLauncher.Runner.Spawn(  //ホストのリーダーを生成
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
            {  //選択したプレイヤーのリーダーがカルティストなら
                //LeaderDic[player].GetComponent<CultistLeader>().SetMySelectedCharacter(myUnit); //選択したUnitをSet
                Debug.Log("set");
            }

        }

        public void RemoveCharacter(PlayerRef player)
        {
            if (LeaderDic[player].tag == "CultistLeader")  //選択したプレイヤーのリーダーがカルティストなら
                LeaderDic[player].GetComponent<CultistLeader>().RemoveMySelectedCharacter(); //選択したUnitをRemove
        }

        public void KilledEnemyLeaderCharcter(PlayerRef player) //敵をキルしたときに
        {
            if (LeaderDic[player].tag == "CultistLeader")   //キルしたのがカルティストリーダーなら
                LeaderDic[player].GetComponent<CultistLeader>().PassiveSkill(); //パッシブスキル発動


        }
    }
}
