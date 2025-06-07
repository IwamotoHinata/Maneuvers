using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Online
{
    /// <summary>
    /// ローカルのリーダーオブジェクトを観測するクラス．
    /// <para>リーダーオブジェクトの生成とRPC関数の実行を行う．</para>
    /// </summary>
    public class LeaderObserver : NetworkBehaviour
    {
        [SerializeField] private List<GameObject> leaderObjects;
        [SerializeField] private NetworkObject guardianUltArea;

        private readonly List<GameObject> _leaderCollection = new List<GameObject>();
        public List<GameObject> Leader => _leaderCollection;

        private void Start()
        {
            if (Object.HasInputAuthority)
            {
                var leader = Instantiate(leaderObjects[RoomPlayer.Local.MyLeader], Vector3.zero, Quaternion.identity);
                leader.GetComponent<BasicLeader>().setLeaderObserver(this);
                _leaderCollection.Add(leader);
                
                FindObjectOfType<ShowUICanvas>().SetLeaderObserver(this);
            }
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_TeleportUnit(Vector3 pos, NetworkObject unit)
        {
            if (unit != null)
            {
                unit.transform.position = pos;
            }
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SpawnGuardianArea(Vector3 pos)
        {
            Runner.Spawn(
                guardianUltArea,
                pos,
                Quaternion.identity,
                Object.InputAuthority
            );
        }
    }
}