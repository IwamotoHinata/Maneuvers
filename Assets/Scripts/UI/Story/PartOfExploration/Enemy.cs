using System;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject PlayerObject; //その名の通り
    public GameObject skin; //追従させるオブジェクトのプレハブ
    public GameObject Tgt; //追いかける対象のプレハブ
    public GameObject Searcher;
    public GameObject nextposition; //次に向かうべき巡回地点

    public float SeconSearchLnage; //草を無視した視界の長さ   
    public float BushWatch;//チェイス中の草を貫通する距離

    public float MitameUp; //見た目の位置調整
    private NavMeshAgent agent;
    private GameObject MySearcher;
    private GameObject myskin; //上を実際にインスタンスしたものを格納
    private GameObject MyTgt; //実際に作成した↑

    private bool Searchmood; //プレイヤーを追尾中はtrue

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //Navmesh設定

        myskin = Instantiate(skin); //スキン作成
        myskin.transform.position = transform.position; //スキンを自分の座標に移動

        MyTgt = Instantiate(Tgt); //追いかける目標作成
        MyTgt.transform.position = nextposition.transform.position; //追いかける対象を次の巡回地点へ

        MySearcher = Instantiate(Searcher); //探査装置作成
        MySearcher.transform.position = transform.position; //探査装置を自分の座標へ
        MySearcher.GetComponent<Searcher>().oya = gameObject; //探査装置の通達対象を自身に設定

        Searchmood = false; //追跡状態ではないことにする
    }

    private void Update()
    {
        if (Searchmood) //現在プレイヤーを追いかけているか確認
        {
            ///プレーヤーが見えるか確認
            RaycastHit2D Hit_info;
            Hit_info = Watch();
            if (Hit_info.collider.tag == "Player") //見えてたら目標更新
                MyTgt.transform.position = PlayerObject.transform.position;
        }

        agent.destination = MyTgt.transform.position;
        myskin.transform.position = new Vector3(transform.position.x, transform.position.y + MitameUp, 0);
        MySearcher.transform.position = transform.position;

        //OffMeshLinkでの移動が始まったらすぐに終わらせる。
        if (agent.isOnOffMeshLink) agent.CompleteOffMeshLink();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        ///対象が自身の追跡していたものか確認、今までプレイヤーを追いかけてきてたか確認。
        if (other.CompareTag("Tgt") && Searchmood)
        {
            ///プレーヤーが見えるか確認
            RaycastHit2D Hit_info;
            Hit_info = Watch();

            //プレイヤーが見えているなら追跡続行、目標を再度プレイヤーに合わせる。
            if (Hit_info.collider.tag == "Player")
            {
                MyTgt.transform.position = PlayerObject.transform.position;
            }
            else
            {
                ///見えていないのであれば追跡終了
                MyTgt.transform.position = nextposition.transform.position;
                Searchmood = false;
            }
            
        }

        if (other.gameObject == nextposition && other.CompareTag("PatrolPosition") && Searchmood == false) ///巡回地点に触れた場合、追尾中でなく、それが次の追跡地点でなければ
        {
        gameObject.GetComponent<Enemy>().nextposition =
        other.gameObject.GetComponent<PatrolPosition>().nextposition;
        MyTgt.transform.position = nextposition.transform.position;
        }
    }


    ///プレイヤーとの視線が通るか確認
    public void Search()
    {
        RaycastHit2D Hit_info;
        ///Rayを投げる
        Hit_info = Watch();

        ///対象がプレイヤーか確認
        if (Hit_info.collider.tag == "Player")
        {
            ///追跡対象の座標をプレイヤーに
            MyTgt.transform.position = PlayerObject.transform.position;
            Searchmood = true;
        }
    }

    public RaycastHit2D Watch()
    {
        RaycastHit2D Hit_info;
        var muki = new Vector2(PlayerObject.transform.position.x - transform.position.x,
            PlayerObject.transform.position.y - transform.position.y).normalized;
        var lange = new Vector2(PlayerObject.transform.position.x - transform.position.x,
            PlayerObject.transform.position.y - transform.position.y).magnitude;
        //一定距離以内であれば草を無効化
        int layer_mask; //レイヤーマスク作成
        if (Vector2.Distance(transform.position, PlayerObject.transform.position) > SeconSearchLnage && !Searchmood)
        {   //追尾中でなくかつ十分に距離が離れていれば草は有効
            layer_mask = LayerMask.GetMask("Wall", "Player", "Bush");
        }
        else
        {
            //追尾中または非常に近い場合
            if (Vector2.Distance(transform.position, PlayerObject.transform.position) > BushWatch)
            {
                //ブッシュウォッチより遠い
                layer_mask = LayerMask.GetMask("Wall", "Player", "Bush");
            }
            else
            {
                //ブッシュウォッチより近い
                layer_mask = LayerMask.GetMask("Wall", "Player");
            }

        }

        Hit_info = Physics2D.Raycast(transform.position, muki, lange, layer_mask); //Ray発射
        return Hit_info;

    }

    private void OnDestroy()
    {
        Destroy(MySearcher);
        Destroy(myskin);
        Destroy(MyTgt);
    }
}