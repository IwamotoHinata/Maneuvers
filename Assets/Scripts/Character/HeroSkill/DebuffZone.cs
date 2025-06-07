using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace Unit
{
    public class DebuffZone : NetworkBehaviour
    {
        [Networked] private TickTimer _destroyTimer { get; set; }//破壊するまでの時間
        [Networked] private TickTimer _debuffInterval { get; set; }//破壊するまでの時間
        private Collider[] _enemys = new Collider[100];  //受け取るコライダー
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
            //デバフ関係の実行
            if (_debuffInterval.Expired(Runner) && _debuffCount < 110)//11秒間0.1秒ごとにデバフをつけるか否か判定を行う。
            {
                //コライダー取得
                Physics.OverlapCapsuleNonAlloc(new Vector3(this.transform.position.x, this.transform.position.y - 50, this.transform.position.z),
                                               new Vector3(this.transform.position.x, this.transform.position.y + 50, this.transform.position.z),
                                               _radius, _enemys);//もともとは15

                
                //デバフの実行
                for (int i = 0; i < 100; i++)
                {
                    //取得したコライダーが無ければ終了
                    if (_enemys[i] == null)
                    {
                        Debug.Log("デバフ終了");
                        break;
                    }

                    //デバフの処理
                    //条件：配列野中がnullでなく、Unitタグがあり、InputAuthorityを有していない
                    if (_enemys[i] != null && _enemys[i].gameObject.tag == "Unit" && _enemys[i].gameObject.GetComponent<CharacterBaseData>().isHasInputAuthority() == false)
                    {
                        RPC_Debuff(i);
                    }
                }

                _debuffCount++;
                _debuffInterval = TickTimer.CreateFromSeconds(Runner, 0.1f);
            }

            //このエフェクトを破壊させる
            if (_destroyTimer.Expired(Runner))
            {
                Runner.Despawn(Object);
            }
                
        }

        //RPC関数の引数でColliderが使えなかったのでこのような処理にしている
        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        public void RPC_Debuff(int num, RpcInfo info = default)
        {
            _enemys[num].gameObject.GetComponent<CharacterStatus>().Debuff();
            Debug.Log(_enemys[num].gameObject.name + "にデバフを付与しました");
        }


    }
}
