using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Fusion;
using UniRx;
using System;
using System.Threading;

namespace Unit
{
    public class CharacterStatus : NetworkBehaviour, IDamagable
    {
        [Networked]
        private float currentHP { get; set; }
        private ReactiveProperty<CharacterState> CharacterState { get; set; } = new ReactiveProperty<CharacterState>();//キャラクターの現在の状態
        private ReactiveProperty<GameObject> TargetObject = new ReactiveProperty<GameObject>(null);//攻撃対象をオブジェクト
        private ReactiveProperty<float> characterHP = new ReactiveProperty<float>(10f);//キャラクターの体力
        private ReactiveProperty<float> currentAttackRange = new ReactiveProperty<float>();//キャラクターの攻撃範囲
        private ReactiveProperty<float> currentSearchRange = new ReactiveProperty<float>();//キャラクターの索敵範囲
        private ReactiveProperty<bool> iVisible = new ReactiveProperty<bool>(false);//キャラクターが敵から見えているか
        private ReactiveProperty<bool> Damaged = new ReactiveProperty<bool>(false);//ダメージを受けているか
        private Subject<bool> _deadEvent = new Subject<bool>();

        [Networked] private TickTimer visibleTimer { get; set; }
        [Networked] private TickTimer attackTimer { get; set; }
        [SerializeField]
        CharacterBaseData MyCharacterBaseData;
        private bool CbSt;
        private IEnumerator CbStaticHitRateChange;
        private IEnumerator CbMoveHitRateChange;
        private const int RESETDEBUFFTIME= 15;
        NavMeshAgent agent;
        private float DefaultMoveSpeed;
        private int DamageCut = 0;
        private TeamType teamType;
        public bool attacked { private set; get; }
        public IObservable<bool> OnDeadEvent
        {
            get { return _deadEvent; }
        }
        public IObservable<float> OnCharacterHPChanged//characterHPの変化があったときに発行される
        {
            get { return characterHP; }
        }
        public IObservable<float> OnCurrentAttackRange//currentAttackRangeの変化があったときに発行される
        {
            get { return currentAttackRange; }
        }
        public IObservable<float> OnCurrentSearchRange//currentSearchRangeの変化があったときに発行される
        {
            get { return currentSearchRange; }
        }
        public IObservable<CharacterState> OnCharacterStateChanged//CharacterStateの変化があったときに発行される
        {
            get { return CharacterState; }
        }
        public IObservable<GameObject> OnCharacterTargetObject//TargetObjectの変化があったときに発行される
        {
            get { return TargetObject; }
        }
        public IObservable<bool> OniVisibleChanged//characteriVisibleが変更された際に発光されるイベント
        {
            get { return iVisible; }
        }
        public IObservable<bool> OnDamagedChanged//Damagedが変更された際に発光されるイベント
        {
            get { return Damaged; }
        }
        public TeamType GetTeamType
        {
            get { return teamType; }
        }

        [Networked] public float CurrentMoveSpeed { get; private set; }

        private float currentAttackPower;
        [Networked]
        public float CurrentAttackPower
        {
            get { return currentAttackPower; }
            private set { currentAttackPower = value; }
        }

        private float currentStaticHitRate;
        [Networked]
        public float CurrentStaticHitRate
        {
            get { return currentStaticHitRate; }
            private set { currentStaticHitRate = value; }
        }

        private float currentMoveHitRate;
        [Networked]
        public float CurrentMoveHitRate
        {
            get { return currentMoveHitRate; }
            private set { currentMoveHitRate = value; }
        }

        [SerializeField]
        private float currentArmor;
        public float CurrentArmor
        {
            get { return currentArmor; }
            private set { currentArmor = value; }
        }
        public bool CurrentIVisibleChanged { get => iVisible.Value; }
        public float CurrentHp { get => characterHP.Value; }
        public float CurrenSerchrange { get => currentSearchRange.Value; }

        void Start()
        {
            MyCharacterBaseData
                .OninitialSetting
                .Where(value => value.Equals(true))
                .Subscribe(_ =>
                {
                    Init();
                })
                .AddTo(this);
        }

        private void Init()
        {
            attacked = false;
            CurrentAttackPower = MyCharacterBaseData.MyAttackPower;         //ベースの攻撃力を現在の攻撃力にする
            currentAttackRange.Value = MyCharacterBaseData.MyattackRange;   //攻撃の射程距離の設定
            currentSearchRange.Value = MyCharacterBaseData.MysearchRange;   //索敵距離の設定
            DefaultMoveSpeed = MyCharacterBaseData.MyMoveSpeed;             //基本移動速度の設定
            CurrentArmor = MyCharacterBaseData.MyArmor;                     //基本の防御力の設定
            CurrentStaticHitRate = MyCharacterBaseData.MyStaticHitRate;     //静止時の攻撃命中率の設定
            CurrentMoveHitRate = MyCharacterBaseData.MyMoveHitRate;         //移動時の攻撃命中率の設定
            agent = GetComponent<NavMeshAgent>();
            ChangeHPValue(MyCharacterBaseData.MaxHP);
            OnCharacterHPChanged
                .Subscribe(hp => {
                    currentHP = hp;
                    DeadCheck();
                }).AddTo(this);

            //チームメイトか敵かの設定
            if (MyCharacterBaseData.HasInputAuthority)
            {
                teamType = TeamType.Teammate;
                //視界をFodWarで表示する。
                GameObject.Find("FogWar(NewMap)").GetComponent<csFogWar>().AddFogRevealer(new csFogWar.FogRevealer(this.gameObject.transform, (int)currentSearchRange.Value, true)); 
            }
            else
            {
                teamType = TeamType.Enemys;
            }



        }
        /// <summary>
        /// キャラクターにダメージを与える
        /// </summary>
        public void AddDamage(float damage)
        {
            Damaged.Value = true;   //ダメージを受けたフラグをオン
            float Penetrationdamage = 0;

            Penetrationdamage = CurrentArmor - damage - ((CurrentArmor - damage) * DamageCut / 100);

            if (Penetrationdamage < 0)
            {
                if (characterHP.Value > 0)
                {
                    characterHP.Value += Penetrationdamage;
                }
            }
            //Debug.Log("残り体力：" + characterHP.Value);
            Damaged.Value = false;//ダメージを受けたフラグをオフ

        }

        /// <summary>
        /// キャラクターの状態を取得
        /// </summary>
        public CharacterState GetCharacterState()
        {
            return CharacterState.Value;
        }

        /// <summary>
        /// 変動するステータスの変化用関数
        /// </summary>
        public void UpdateStatus(
            float CurrentAttackPower = -1,
            float CurrentSearchRange = -1,
            float CurrentStaticHitRate = -1,
            float CurrentMoveHitRate = -1,
            float CurrentMoveSpeed = -1,
            int DamageCut = 0)
        {
            if(CurrentAttackPower >= 0)
            {
                this.CurrentAttackPower = CurrentAttackPower;
            }
            else
            {
                this.CurrentAttackPower = MyCharacterBaseData.MyAttackPower;
            }

            if (CurrentSearchRange >= 0)
            {
                currentSearchRange.Value = CurrentSearchRange;
            }
            else
            {
                currentSearchRange.Value = MyCharacterBaseData.MysearchRange;
            }

            if (CurrentStaticHitRate >= 0)
            {
                this.CurrentStaticHitRate = CurrentStaticHitRate;
            }
            else
            {
                this.CurrentStaticHitRate = MyCharacterBaseData.MyStaticHitRate;
            }

            if (CurrentMoveHitRate >= 0)
            {
                this.CurrentMoveHitRate = CurrentMoveHitRate;
            }
            else
            {
                this.CurrentMoveHitRate = MyCharacterBaseData.MyMoveHitRate;
            }

            if (CurrentMoveSpeed >= 0)
            {
                this.CurrentMoveSpeed = CurrentMoveSpeed;
            }
            else
            {
                this.CurrentMoveSpeed = MyCharacterBaseData.MyMoveSpeed;
            }

            if (DamageCut >= 0)
            {
                this.DamageCut = DamageCut;
            }
            else
            {
                this.DamageCut = 0;
            }
        }

        /// <summary>
        /// チェインバフのスキルによる命中率のステータス変更
        /// </summary>
        public float ChainbuffStaticHitRateChange(bool ChainbuffState)
        {
            // Debug.Log(MyCharacterBaseData.MyCharacterID + "：チェインバフ効果前、命中率：" + CurrentStaticHitRate);
            if (ChainbuffState)
            {
                CurrentStaticHitRate *= 1.1f;
                CbStaticHitRateChange = ChangeStaticHitRate(CurrentStaticHitRate);
                StartCoroutine(CbStaticHitRateChange);
            }
            else
            {
                CbStaticHitRateChange = null;
                CurrentStaticHitRate /= 1.1f;
            }
            // Debug.Log(MyCharacterBaseData.MyCharacterID + "：チェインバフ効果、命中率：" + CurrentStaticHitRate);
            return CurrentStaticHitRate;
        }
        public float ChainbuffMoveHitRateChange(bool ChainbuffState)
        {
            CbSt = ChainbuffState;
            //Debug.Log(MyCharacterBaseData.MyCharacterID + "：チェインバフ効果前、移動命中率：" + CurrentMoveHitRate);
            if (ChainbuffState)
            {
                CurrentMoveHitRate *= 1.1f;
                CbMoveHitRateChange = ChangeMoveHitRate(CurrentMoveHitRate);
                StartCoroutine(CbMoveHitRateChange);
            }
            else
            {
                CbMoveHitRateChange = null;
                CurrentMoveHitRate /= 1.1f;
            }
            //Debug.Log(MyCharacterBaseData.MyCharacterID + "：チェインバフ効果、移動命中率：" + CurrentMoveHitRate);
            return CurrentMoveHitRate;
        }
        //チェインバフ状態中に他のスキルなどにより命中率が書き換えられた時用のコード
        private IEnumerator ChangeStaticHitRate(float ChangedStaticRate)
        {
            yield return new WaitUntil(() => ChangedStaticRate != CurrentStaticHitRate);
            if (CbSt.Equals(true))
                ChainbuffStaticHitRateChange(true);
        }
        private IEnumerator ChangeMoveHitRate(float ChangedMoveRate)
        {
            yield return new WaitUntil(() => ChangedMoveRate != CurrentMoveHitRate);
            if (CbSt.Equals(true))
                ChainbuffMoveHitRateChange(true);
        }

        public void Debuff()
        {
            Debug.Log("デバフ食らった");
            currentAttackRange.Value = 10f;
            currentSearchRange.Value = 10f;
            StartCoroutine(ResetDebuff());
        }

        IEnumerator ResetDebuff()
        {
            yield return new WaitForSeconds(RESETDEBUFFTIME);
            currentAttackRange.Value = MyCharacterBaseData.MyattackRange;
            currentSearchRange.Value = MyCharacterBaseData.MysearchRange;
        }

        /// <summary>
        /// ガーディアンリーダー2のスキルによるステータス変更
        /// </summary>
        public void GuadianChangeMoveSpeed(bool GuadianSkill)
        {
            // Debug.Log("味方変更前" + CurrentMoveSpeed);
            if (GuadianSkill.Equals(true))
                CurrentMoveSpeed *= 1.2f;
            else CurrentMoveSpeed /= 1.2f;
            // Debug.Log("味方変更後" + CurrentMoveSpeed);
        }
        public void GuadianChangeHitRate(float ChangeHitRate, bool GuadianSkill)
        {
            // Debug.Log("敵変更前" + CurrentStaticHitRate + ", " + CurrentMoveHitRate);
            if (GuadianSkill.Equals(true))
            {
                currentStaticHitRate -= ChangeHitRate;
                CurrentMoveHitRate -= ChangeHitRate;
            }
            else
            {
                currentStaticHitRate += ChangeHitRate;
                CurrentMoveHitRate += ChangeHitRate;
            }
            //Debug.Log("敵変更後" + CurrentStaticHitRate + ", " + CurrentMoveHitRate);
        }

        /// <summary>
        /// キャラクターの状態を変更
        /// </summary>
        public void ChangeCharacterState(CharacterState State)
        {
            if (State.Equals(global::CharacterState.Dead))
            {
                CharacterState.Value = State;
            }
            if (!CharacterState.Value.Equals(global::CharacterState.Dead))
            {
                CharacterState.Value = State;
            }
            else
            {
                Debug.LogWarning("すでに死亡しているためステートの変更をキャンセルしました");
            }
        }

        private void DeadCheck()
        {
            if (currentHP <= 0 && !CharacterState.Value.Equals(global::CharacterState.Dead))
            {
                _deadEvent.OnNext(true);
                Debug.Log("死亡処理を開始");
            }
        }

        /// <summary>
        /// 自分の体力の変更
        /// </summary>
        public void ChangeHPValue(float value)
        {
            if (characterHP.Value != value) characterHP.Value = value;
            else
            {
                Debug.Log("現在のHPと変更後のHPが同じため値を変更しませんでした");
            }
        }

        /// <summary>
        /// 自分の攻撃力を変更
        /// </summary>
        public void ChangeAttackPower(float value)
        {
            CurrentAttackPower = value;
        }

        /// <summary>
        /// 自分の移動速度を変更
        /// </summary>
        public void ChangeMoveSpeed(float value)
        {
            CurrentMoveSpeed = value;
        }

        /// <summary>
        /// 自分の体力を上限まで回復
        /// </summary>
        public void HpHeal(float value)
        {
            Debug.Log($"{value}回復しました");
            if (characterHP.Value + value > MyCharacterBaseData.MaxHP)
            {
                characterHP.Value = MyCharacterBaseData.MaxHP;
            }
            else
            {
                characterHP.Value += value;
            }
        }


        /// <summary>
        /// 自分の移動速度の変更
        /// </summary>
        public void ChangeMoveSpeedValue(bool SkillState)
        {
            if (SkillState.Equals(true)) CurrentMoveSpeed = DefaultMoveSpeed * 2;
            else CurrentMoveSpeed = DefaultMoveSpeed;

            agent.speed = CurrentMoveSpeed;
            Debug.Log("突撃兵の移動速度：" + CurrentMoveSpeed);

        }
        /// <summary>
        /// 攻撃対象を設定
        /// </summary>
        public void SetTarget(GameObject newTarget)
        {
            if (GetCharacterState() != global::CharacterState.Dead)
            {
                TargetObject.Value = newTarget;
                if (newTarget != null) Debug.Log($"{newTarget.name} を攻撃対象に設定");
            }
            else
            {
                Debug.LogError("キャラクターが既に死亡しています");
            }
        }
        /// <summary>
        /// キャラクターが発見された時の動作
        /// </summary>
        public void Idiscovered(bool value)
        {
            if (value.Equals(true))
            {
                iVisible.Value = true;
                setVisibleTimer(3f);
            }
        }
        public void setVisibleTimer(float time)
        {
            if(visibleTimer.RemainingTime(Runner) < time || visibleTimer.ExpiredOrNotRunning(Runner))visibleTimer = TickTimer.CreateFromSeconds(Runner, time);//点滅の原因、この前に
        }

        public void ChangeiVisible(bool value)
        {
            if (!HasStateAuthority) iVisible.Value = value;
        }

        public override void FixedUpdateNetwork()
        {
            if (visibleTimer.Expired(Runner))
            {
                if (HasStateAuthority)
                {
                    iVisible.Value = false;
                    visibleTimer = TickTimer.None;
                }
            }
            if (attackTimer.Expired(Runner))
            {
                attacked = false;
                attackTimer = TickTimer.None;
            }
        }


        /// <summary>
        /// 攻撃してから15秒経つと発見されるための痕跡が消える
        /// </summary>
        public void isAttack()
        {
            attacked = true;
            attackTimer = TickTimer.CreateFromSeconds(Runner, 15f);
        }
    }
}