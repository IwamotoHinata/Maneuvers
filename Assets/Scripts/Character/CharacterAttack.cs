using System.Collections;
using UnityEngine;
using UniRx;
using Fusion;

namespace Unit
{
    public class CharacterAttack : CharacterStateActionBase

    {
        CharacterBaseData MyCharacterBaseData;
        CharacterStatus MyCharacterStatus;
        //private LeaderManager CommonLeaderManager;
        private bool _isCharacterlive = true;
        private float _damage;
        private float _reloadTime;
        //[SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _firePosition;
        IDamagable _damagable;
        [Networked] private TickTimer _attackIntervalTimer { set; get; }
        private GameObject _targetObject = null;
        private bool _canAttack = false;
        CharacterStatus _targetStatus;


        public void Start()
        {
            base.Spawned();

            MyCharacterBaseData = GetComponentInParent<CharacterBaseData>();
            MyCharacterStatus = GetComponentInParent<CharacterStatus>();
            //CommonLeaderManager = GameObject.Find("LeaderManager").GetComponent<LeaderManager>();

            //攻撃に関する処理はホスト側しか行わない
            if (MyCharacterBaseData.isHasStateAuthority())
            {
                MyCharacterBaseData
                    .OninitialSetting
                    .Where(value => value == true)
                    .Subscribe(_ => { Init(); }).AddTo(this);
            }
            
        }

        private void Init()
        {
            Debug.Log("CharacterAttack:初期値の設定がされました");
            _damage = MyCharacterStatus.CurrentAttackPower; //現在の攻撃力をダメージとして登録
            _reloadTime = MyCharacterBaseData.MyReloadSpeed;

            //characterが死亡したときは以降処理を実行しないように知る
            MyCharacterStatus
                .OnCharacterStateChanged
                .Where(CharacterState => CharacterState == CharacterState.Dead)
                .Subscribe(_ =>
                    {
                        EndStateAction();
                    }
                ).AddTo(this);


            MyCharacterStatus
                .OnCharacterTargetObject
                .Subscribe(_target =>
                    {
                        if (_target != null)
                        {

                            _targetObject = _target;

                            if (_targetObject.gameObject.TryGetComponent(out IDamagable DamageCs))
                            {
                                _damagable = DamageCs;
                                if (_targetObject.gameObject.TryGetComponent(out CharacterStatus CharacterStatusCs))
                                {
                                    if(!CharacterStatusCs.GetCharacterState().Equals(CharacterState.Dead))
                                    {
                                        _targetStatus = CharacterStatusCs;
                                        StartStateAction();
                                    }
                                    else
                                    {
                                        Debug.LogWarning("_targetObject. is already dead. The attack cannot be processed");
                                        EndStateAction();
                                    }
                                }
                                else
                                {
                                    Debug.LogWarning("_targetObject does not have CharacterStatusCs. The attack cannot be processed");
                                    EndStateAction();
                                }
                            }
                            else
                            {
                                Debug.LogWarning("_targetObject does not have DamageCs.The attack cannot be processed");
                                EndStateAction();
                            }
                        }
                        else
                        {
                            EndStateAction();
                        }
                    }
                ).AddTo(this);
        }


        public override void FixedUpdateNetwork()
        {
            if (_attackIntervalTimer.ExpiredOrNotRunning(Runner) && _canAttack)
            {
                if (_isCharacterlive)
                {
                    if (CanAttackState())
                    {
                        if (_targetObject == null)
                        {
                            EndStateAction();
                            return;
                        }
                        MyCharacterStatus.isAttack();
                        transform.parent.gameObject.transform.LookAt(_targetObject.transform.position); //攻撃対象に向く
                                                                                                        //命中率を引いたら攻撃力だけのダメージを与える。そうでなければ0ダメ０時を与える。
                        if (MyCharacterStatus.GetCharacterState().Equals(CharacterState.Attack))
                        {
                            //停止中の攻撃の処理 注意ヒットレートは%
                            if (MyCharacterBaseData.MyStaticHitRate > Random.Range(0, 100f))
                            {
                                _damagable.AddDamage(_damage);
                                //KillCheck(_targetObject, _targetStatus);
                            }
                            else
                            {
                                _damagable.AddDamage(0);
                            }
                        }
                        else if (MyCharacterStatus.GetCharacterState().Equals(CharacterState.MoveAttack))
                        {
                            //移動中の攻撃の処理
                            if (MyCharacterBaseData.MyMoveHitRate > Random.Range(0, 100f))
                            {
                                _damagable.AddDamage(_damage);
                                //KillCheck(_targetObject, _targetStatus);
                            }
                            else
                            {
                                _damagable.AddDamage(0);
                            }
                        }
                        if(Object.HasStateAuthority) RPC_ShotBullet();
                        //ShotEffect(); //攻撃エフェクトを発生させる
                        if (Object.HasInputAuthority)
                        {
                            if (gameObject.TryGetComponent(out SniperHeroSkill sniper) && sniper.getSkillActive()) SoundManager.Instance.shotSe(SeType.SniperSkillAttack);
                            else SoundManager.Instance.shotSe(SeType.GuardianAttack);
                        }
                        _attackIntervalTimer = TickTimer.CreateFromSeconds(Runner, _reloadTime);
                    }
                }
                else
                {
                    EndStateAction();
                    _attackIntervalTimer = TickTimer.None;
                }
            }
        }

        IEnumerator ShotEffect() //射撃エフェクト
        {
            for (int i = 0; i < 3; i++)
            {
                if (Object.HasInputAuthority) RPC_ShotBullet();
                yield return new WaitForSeconds(0.2f);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_ShotBullet()
        {
            //var bullet = Instantiate(_bulletPrefab, _firePosition.transform.position, _firePosition.transform.rotation);
            //Destroy(bullet, 2);
        }

        /// <summary>
        /// characterが戦闘できる状態かどうか
        /// </summary>
        private bool CanAttackState()
        {
            if (MyCharacterStatus.GetCharacterState().Equals(CharacterState.Move))
            {
                if (MyCharacterBaseData.MyUnitType.Equals(UnitType.Stealth))
                {
                    var StealthHeroSkill_CS = GetComponent<StealthHeroSkill>();
                    if (StealthHeroSkill_CS.GetStealthState())
                    {
                        return false;
                    }
                    else
                    {
                        MyCharacterStatus.ChangeCharacterState(CharacterState.MoveAttack);
                    }
                }
                else
                {
                    MyCharacterStatus.ChangeCharacterState(CharacterState.MoveAttack);
                }
            }
            else if (MyCharacterStatus.GetCharacterState().Equals(CharacterState.VigilanceMove) ||
                     MyCharacterStatus.GetCharacterState().Equals(CharacterState.Idle))
            {
                if (MyCharacterBaseData.MyUnitType.Equals(UnitType.Stealth))
                {
                    var StealthHeroSkill_CS = GetComponent<StealthHeroSkill>();
                    if (StealthHeroSkill_CS.GetStealthState())
                    {
                        return false;
                    }
                    else
                    {
                        MyCharacterStatus.ChangeCharacterState(CharacterState.Attack);
                    }
                }
                else
                {
                    MyCharacterStatus.ChangeCharacterState(CharacterState.Attack);
                }
            }
            
            if (MyCharacterStatus.GetCharacterState().Equals(CharacterState.Attack)
                || MyCharacterStatus.GetCharacterState().Equals(CharacterState.MoveAttack))
            {
                //攻撃状態のときstaticHitRateがゼロであれば攻撃しない
                if (MyCharacterStatus.GetCharacterState().Equals(CharacterState.Attack) && MyCharacterStatus.CurrentStaticHitRate <= 0)
                {
                    Debug.Log("命中率0のため攻撃しない");
                    return false;
                }
                    

                //移動攻撃状態のときMoveHitRateがゼロであれば攻撃しない
                if (MyCharacterStatus.GetCharacterState().Equals(CharacterState.MoveAttack) && MyCharacterStatus.CurrentMoveHitRate <= 0)
                {
                    Debug.Log("命中率0のため攻撃しない");
                    return false;
                }

                //それぞれ命中率が0でなければ攻撃する
                return true;
            }
            return false;
        }

        public override void StartStateAction()
        {
            if (MyCharacterStatus.GetCharacterState() == CharacterState.Move)//現在移動中の場合は移動攻撃状態に移行
                MyCharacterStatus.ChangeCharacterState(CharacterState.MoveAttack);
            else
                MyCharacterStatus.ChangeCharacterState(CharacterState.Attack); //移動攻撃をしていなければ停止攻撃状態に移行
            _canAttack = true;
        }

        public override void StateAction()
        {
        }

        public override void EndStateAction()
        {
            _canAttack = false;
            if (MyCharacterStatus.GetCharacterState().Equals(CharacterState.Attack))
                MyCharacterStatus.ChangeCharacterState(CharacterState.Idle); //攻撃状態で敵がいなくなった場合は待機状態に移行
            else if (MyCharacterStatus.GetCharacterState().Equals(CharacterState.MoveAttack))
                MyCharacterStatus.ChangeCharacterState(CharacterState.Move); //移動攻撃状態で敵がいなくなった場合は移動状態に移行
        }

        private void KillCheck(GameObject Target, CharacterStatus TargetState)
        {
            if (TargetState.GetCharacterState() == CharacterState.Dead)  //ターゲットが死んだら
            {
                try
                {
                    //CommonLeaderManager.KilledEnemyLeaderCharcter(this.Object.InputAuthority);
                }
                catch (System.NullReferenceException)
                {
                    Debug.LogWarning("CommonLeaderManagerが取得できていません");
                    throw;
                }
            }
        }
    }
}