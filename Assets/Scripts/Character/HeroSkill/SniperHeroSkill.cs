using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;

namespace Unit
{
    public class SniperHeroSkill : HeroSkill
    {
        private CharacterBaseData MyCharacterBaseData;
        private CharacterMove MyStateMove;
        private CharacterStatus MyCharacterStatus;
        [SerializeField] bool SkillStock = false;
        [SerializeField] bool SniperSkillActive = false; //パッシブのオンオフのフラグ

        public bool getSkillActive() { return SniperSkillActive; }

        private bool CountTimeStopped = false;//20秒間移動・攻撃をしていないかを検出するbool
        [Networked] private TickTimer timer { get; set; }//CountTimeStopped用のタイマー

        void Start()
        {
            MyCharacterBaseData = GetComponent<CharacterBaseData>();
            StartCoroutine(CheckUseSkill(MyCharacterBaseData.MySkillTime)); //アクティブスキル
            MyStateMove = GetComponent<CharacterMove>();
            MyCharacterStatus = GetComponent<CharacterStatus>();
            timer = TickTimer.CreateFromSeconds(Runner, 20.0f);
            if (MyCharacterBaseData.isHasInputAuthority()) SoundManager.Instance.shotSe(SeType.SniperSkillCharge);

            MyCharacterStatus   //リロードしたときに
                .OnCharacterStateChanged
                .Where(CharacterState => CharacterState == CharacterState.Reload)
                .Subscribe(_ =>
                {
                    SniperSkillActive = false;   //スキルをオフにする
                    if(MyCharacterBaseData.isHasInputAuthority())

                    RPC_SniperSkillChangeStatus(SniperSkillActive);    //ステータスを変更

                }
            ).AddTo(this);

            MyCharacterStatus   //移動・攻撃をした際にCountTimeStoppedとtimerを初期化
                .OnCharacterStateChanged
                .Where(CharacterState => CharacterState == CharacterState.Attack || CharacterState == CharacterState.Move)
                .Subscribe(_ =>
                {
                    if (MyCharacterBaseData.isHasInputAuthority())
                    {
                        RPC_ResetTimer();
                        SoundManager.Instance.shotSe(SeType.SniperSkillCharge);
                    }
                }
            ).AddTo(this);

            MyCharacterStatus   //ダメージを受けた時に
                .OnDamagedChanged
                .Where(Damaged => true)
                .Subscribe(_ =>
                {
                    SkillStock = false; //保管中のスキルをオフ
                    SniperSkillActive = false; //パッシブのオンオフのフラグ
                    if (MyCharacterBaseData.isHasInputAuthority())
                    {
                        RPC_ResetTimer();
                        SoundManager.Instance.shotSe(SeType.SniperSkillCharge);
                    }
                }
            ).AddTo(this);
        }

        public override void FixedUpdateNetwork()
        {
            //20秒間移動・攻撃をしていないかを検出する
            if (timer.Expired(Runner))
            { 
                CountTimeStopped = true;
                timer = TickTimer.None;
            }
                

            //残り時間を知りたいとき(デバッグ用)
            //if(MyCharacterBaseData.isHasInputAuthority())
              //  Debug.Log(timer.RemainingTime(Runner));


            if (CountTimeStopped　&& !SkillStock && MyCharacterBaseData.isHasInputAuthority())  //止まっている時間が20秒を超えて且つスキルを保管中をじゃなければパッシブ発動
                PassiveSkill();
        }

        /// <summary>
        ///パッシブスキルの処理を記述（使用する場所は任せます）
        /// </summary>
        public override void PassiveSkill()
        {
            //パッシブクラスの内容を記述
            SniperSkillActive = true;
            RPC_SniperSkillChangeStatus(SniperSkillActive);    //ステータスを変更
            // Debug.Log("スナイパー");
        }

        /// <summary>
        /// アクティブスキルの処理を記述
        /// </summary>
        public override void ActiveSkill()
        {
            //スキルの内容を記述
            Debug.Log("スナイパー-active-");
            if (SniperSkillActive && !SkillStock)
            {
                SniperSkillActive = false;   //オフにする
                RPC_SniperSkillChangeStatus(SniperSkillActive);    //ステータスを変更
                SkillStock = true;
            }
            else if(SkillStock)
            {
                SniperSkillActive = true;   //オンにする
                RPC_SniperSkillChangeStatus(SniperSkillActive);    //ステータスを変更
                SkillStock = false;
            }
            
        }

        /// <summary>
        /// タイマーとCountTimeStoppedを初期化するためのメソッド
        /// </summary>
        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        public void RPC_ResetTimer()
        {
            timer = TickTimer.CreateFromSeconds(Runner, 20.0f);
            CountTimeStopped = false;
        }
        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        public void RPC_SniperSkillChangeStatus(bool SkillState, RpcInfo info = default)    //スナイパーのスキルによるステータスの変更
        {
            //ステータスの変更処理
            if (SkillState)   //ステータス変更
            {
                MyCharacterStatus.UpdateStatus(CurrentAttackPower:150f,CurrentSearchRange:0f, CurrentStaticHitRate:95f, CurrentMoveHitRate:95f);
            }
            else   //ステータスをデフォルトに戻す
            {
                MyCharacterStatus.UpdateStatus();
            }
        }
    }
}