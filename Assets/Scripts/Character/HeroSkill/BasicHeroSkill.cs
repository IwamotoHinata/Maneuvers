using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;
using Fusion;
using System.Linq;
using UniRx;
using Unity.VisualScripting;

namespace Unit
{ 
    public class BasicHeroSkill : HeroSkill
    {  
        private CharacterBaseData MyCharacterBaseData;
        private CharacterStatus MyCharacterStatus;
        [SerializeField] float Range;//効果範囲
        [SerializeField] float HealMe;//自身をどれだけ回復するか
        [SerializeField] float HealHero;//ヒーローを何ポイント回復するか
        [SerializeField] float HealMinion;//ミニオンを何ポイント回復するか
        [SerializeField] float AutSkilUse;
        [SerializeField] float EffectSizeNum;
        private Collider[] hearlCols;//回復対象
        private Collider myCol;
        private bool a,b,visible;
        [SerializeField] GameObject FriendryEffect;
        [SerializeField]  GameObject EnemyEffect;
        
        void Start()
        {
            MyCharacterBaseData = GetComponent<CharacterBaseData>();
            MyCharacterStatus = GetComponent<CharacterStatus>();
            StartCoroutine(CheckUseSkillBasic());
            myCol = GetComponent<Collider>();
            /*
            MyCharacterStatus
                   .OniVisibleChanged
                   .Subscribe(value =>
                   {
                       visible = value;
                   }
               ).AddTo(this);
            */

        }

        /// <summary>
        ///パッシブスキルの処理を記述（使用する場所は任せます）
        /// </summary>
        public override void PassiveSkill()
        {
            //パッシブクラスの内容を記述
            Debug.Log("基本兵");
        }

        /// <summary>
        /// アクティブスキルの処理を記述
        /// </summary>
        public override void ActiveSkill()//これはinputAuthorityを持つプレイヤーサイドのみが呼び出せる。確認はこれの継承元を見よ。
        {
            RPC_HealingSkill();//下記のRPCを呼び出している
            Debug.Log("基本兵-active-");
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        private void RPC_HealingSkill()
        {
            if (MyCharacterBaseData.isHasStateAuthority()) //RPCは全体へ向けて行われるのでホストでのみ効果を発揮。
            {

                Debug.Log("基本兵ホストで起動");
                //自身を回復
                MyCharacterStatus.HpHeal(HealMe);
                hearlCols = Physics.OverlapSphere(this.transform.position, Range);
                for (int r = 1; r <= hearlCols.Length; r++)//
                {
                    if (hearlCols[r - 1] == myCol)
                    { //取得コライダーが自分の場合
                        Debug.Log("自分を回復");
                    }
                    else
                    {//取得コライダーが自分じゃない場合
                        if (hearlCols[r - 1].gameObject.TryGetComponent<CharacterBaseData>(out CharacterBaseData colCharacterBaseDate)) //ユニットか確認
                        {
                            if (colCharacterBaseDate.GetCharacterOwnerType() == MyCharacterBaseData.GetCharacterOwnerType()) { //同じチームか確認
                                if (hearlCols[r - 1].gameObject.TryGetComponent<MinionMove>(out MinionMove colMinionMove))
                                { //ミニオンかどうか確認
                                    if (hearlCols[r - 1].gameObject.TryGetComponent<CharacterStatus>(out CharacterStatus ObjCharacterStatus))//ミニオンかどうかを確認する有効な手立てが今のところこれだけ
                                    {
                                        Debug.Log("ミニオン回復");
                                        ObjCharacterStatus.HpHeal(HealMinion);
                                    }
                                }
                                else {
                                    if (hearlCols[r - 1].gameObject.TryGetComponent<CharacterStatus>(out CharacterStatus ObjCharacterStatus))//ミニオンかどうかを確認する有効な手立てが今のところこれだけ
                                    {
                                        Debug.Log("ヒーロ－回復");
                                        ObjCharacterStatus.HpHeal(HealMinion);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Debug.Log("基本兵のスキル使用しました、所有権は" + MyCharacterBaseData.HasInputAuthority +"発見状態は"+ MyCharacterStatus.CurrentIVisibleChanged);

            //エフェクトについて。この処理はホストクライアント双方で発動する
            if (MyCharacterBaseData.HasInputAuthority)//持ち主の場合
            {
                GameObject  effectObj = GameObject.Instantiate(FriendryEffect,this.transform.position,this.transform.rotation);
                effectObj.transform.localScale = Vector3.one * EffectSizeNum * Range;
                SoundManager.Instance.shotSe(SeType.BasicSkill);
                GameObject.Destroy(effectObj,5f);//5秒後に破壊

            } else if (MyCharacterStatus.CurrentIVisibleChanged) //持ち主ではない場合見られているか確認

            {
                GameObject effectObj = GameObject.Instantiate(EnemyEffect, this.transform.position, this.transform.rotation);
                effectObj.transform.localScale = Vector3.one * EffectSizeNum * Range;
                SoundManager.Instance.shotSe(SeType.BasicSkill);
                GameObject.Destroy(effectObj, 5f);//5秒後に破壊
            }


        }

        private IEnumerator CheckUseSkillBasic()
        {
            Debug.Log("継承先のものが起動");
            yield return null;
            myCharacterBaseData = GetComponent<CharacterBaseData>();
            myCharacterMove = GetComponent<CharacterMove>();
            _basicLeader = FindObjectOfType<BasicLeader>();

            //スキルのクールタイムが決まるまで待機
            yield return new WaitUntil(() => myCharacterBaseData.MySkillTime != 0);
            float skillTime = myCharacterBaseData.MySkillTime;

            while (true)
            {
                yield return new WaitUntil(() => ((Input.GetKeyDown(KeyCode.E) || MyCharacterStatus.CurrentHp < myCharacterBaseData.MyHp*AutSkilUse) && myCharacterMove.ReturnIsSelect() && myCharacterBaseData.isHasInputAuthority() ));
                {
                    _basicLeader.ReduceUltPoint();
                    ActiveSkill();
                    SoundManager.Instance.shotSe(SeType.Skill);
                    //Debug.Log("スキル使った！！+スキルの仕様にかかる時間は"　+ skillTime);
                }
                RPC_SkillUsed();
                yield return new WaitForSeconds(skillTime);
            }
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        void RPC_SkillUsed()
        {
            skillSubject.OnNext(MyCharacterBaseData.MySkillTime);
        }
    }
}

