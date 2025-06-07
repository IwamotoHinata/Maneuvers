using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Fusion;

namespace Unit
{
    public class AssaultHeroSkill : HeroSkill
    {
        [SerializeField] private float SkillActiveTime = 15;
        private float beforeHP;
        private IEnumerator CountLastTime;
        private CharacterStatus MyCharacterStatus;
        private bool AssaultSkillActive = false;

        //エフェクト
        [SerializeField] private GameObject _assaultEffect;
        [SerializeField] private GameObject _assaultCancelEffect;

        void Start()
        {
            StartCoroutine(CheckUseSkillAssault());
            MyCharacterStatus = GetComponent<CharacterStatus>();
            CountLastTime = DefaultChangeSpeed();

        }

        /// <summary>
        ///パッシブスキルの処理を記述（使用する場所は任せます）
        /// </summary>
        public override void PassiveSkill()
        {
            //パッシブクラスの内容を記述
            Debug.Log("突撃兵");
        }

        /// <summary>
        /// アクティブスキルの処理を記述
        /// </summary>
        public override void ActiveSkill()
        {
            //スキルの内容を記述
            Debug.Log("突撃兵-active-");
            RPC_AssaultSkill();
        }

        private IEnumerator DefaultChangeSpeed()
        {
            yield return new WaitForSeconds(SkillActiveTime);
            AssaultSkillActive = false;
            MyCharacterStatus.ChangeMoveSpeedValue(AssaultSkillActive);
            _assaultEffect.SetActive(false);//エフェクトの終了
        }

        /// <summary>
        /// PRCを用いたアクティブスキル
        /// </summary>
        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        private void RPC_AssaultSkill()
        {
            AssaultSkillActive = true;
            MyCharacterStatus.ChangeMoveSpeedValue(AssaultSkillActive);

            //時間経過でスキルの効果解除
            StartCoroutine(CountLastTime);

            //エフェクトを表示する
            _assaultEffect.SetActive(true);
            SoundManager.Instance.shotSe(SeType.AssaultSkillStart);
            ParticleSystem skillParticle = _assaultEffect.GetComponent<ParticleSystem>();
            skillParticle.Play();

            //ダメージを受けたらスキルの効果解除
            beforeHP = MyCharacterStatus.CurrentHp;
            MyCharacterStatus
                   .OnCharacterHPChanged
                   .Subscribe(characterHP =>
                   {
                       if (characterHP < beforeHP && AssaultSkillActive == true)
                       {
                           Debug.Log("攻撃を受けたのでスキル解除");
                           AssaultSkillActive = false;
                           StopCoroutine(CountLastTime);//時間経過でスキルの効果を解除するコルーチンを停止
                           CountLastTime = null;
                           CountLastTime = DefaultChangeSpeed();
                           MyCharacterStatus.ChangeMoveSpeedValue(AssaultSkillActive);

                           //強制終了時のエフェクト再生
                           _assaultCancelEffect.SetActive(true);
                           var cancelParticle = _assaultCancelEffect.GetComponent<ParticleSystem>();
                           cancelParticle.Play();
                           SoundManager.Instance.shotSe(SeType.AssaultSkillStop);

                           _assaultEffect.SetActive(false);
                       }
                   }).AddTo(this);
        }

        /// <summary>
        /// Assault用のクールタイム管理関数
        /// </summary>
        /// <param name="skillTime"></param>
        /// <returns></returns>
        private IEnumerator CheckUseSkillAssault()
        {
            yield return null;
            myCharacterBaseData = GetComponent<CharacterBaseData>();
            myCharacterMove = GetComponent<CharacterMove>();
            _basicLeader = FindObjectOfType<BasicLeader>();
            //スキルのクールタイムが決まるまで待機
            yield return new WaitUntil(() => myCharacterBaseData.MySkillTime != 0);
            float skillTime = myCharacterBaseData.MySkillTime;

            while (true)
            {
                yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.E) && myCharacterMove.ReturnIsSelect() && myCharacterBaseData.isHasInputAuthority()));
                {
                    _basicLeader.ReduceUltPoint();
                    ActiveSkill();
                    SoundManager.Instance.shotSe(SeType.Skill);
                }
                yield return new WaitUntil(() => AssaultSkillActive == false);//スキルの効果が切れるまで（AssaultSkillActiveがfalseになるまで）待機
                Debug.Log("クールタイム開始");
                RPC_SkillUsed();
                yield return new WaitForSeconds(skillTime);
            }
        }
        
        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        void RPC_SkillUsed()
        {
            skillSubject.OnNext(myCharacterBaseData.MySkillTime);
        }
    }
}

