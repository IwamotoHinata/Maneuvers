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

        //�G�t�F�N�g
        [SerializeField] private GameObject _assaultEffect;
        [SerializeField] private GameObject _assaultCancelEffect;

        void Start()
        {
            StartCoroutine(CheckUseSkillAssault());
            MyCharacterStatus = GetComponent<CharacterStatus>();
            CountLastTime = DefaultChangeSpeed();

        }

        /// <summary>
        ///�p�b�V�u�X�L���̏������L�q�i�g�p����ꏊ�͔C���܂��j
        /// </summary>
        public override void PassiveSkill()
        {
            //�p�b�V�u�N���X�̓��e���L�q
            Debug.Log("�ˌ���");
        }

        /// <summary>
        /// �A�N�e�B�u�X�L���̏������L�q
        /// </summary>
        public override void ActiveSkill()
        {
            //�X�L���̓��e���L�q
            Debug.Log("�ˌ���-active-");
            RPC_AssaultSkill();
        }

        private IEnumerator DefaultChangeSpeed()
        {
            yield return new WaitForSeconds(SkillActiveTime);
            AssaultSkillActive = false;
            MyCharacterStatus.ChangeMoveSpeedValue(AssaultSkillActive);
            _assaultEffect.SetActive(false);//�G�t�F�N�g�̏I��
        }

        /// <summary>
        /// PRC��p�����A�N�e�B�u�X�L��
        /// </summary>
        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        private void RPC_AssaultSkill()
        {
            AssaultSkillActive = true;
            MyCharacterStatus.ChangeMoveSpeedValue(AssaultSkillActive);

            //���Ԍo�߂ŃX�L���̌��ʉ���
            StartCoroutine(CountLastTime);

            //�G�t�F�N�g��\������
            _assaultEffect.SetActive(true);
            SoundManager.Instance.shotSe(SeType.AssaultSkillStart);
            ParticleSystem skillParticle = _assaultEffect.GetComponent<ParticleSystem>();
            skillParticle.Play();

            //�_���[�W���󂯂���X�L���̌��ʉ���
            beforeHP = MyCharacterStatus.CurrentHp;
            MyCharacterStatus
                   .OnCharacterHPChanged
                   .Subscribe(characterHP =>
                   {
                       if (characterHP < beforeHP && AssaultSkillActive == true)
                       {
                           Debug.Log("�U�����󂯂��̂ŃX�L������");
                           AssaultSkillActive = false;
                           StopCoroutine(CountLastTime);//���Ԍo�߂ŃX�L���̌��ʂ���������R���[�`�����~
                           CountLastTime = null;
                           CountLastTime = DefaultChangeSpeed();
                           MyCharacterStatus.ChangeMoveSpeedValue(AssaultSkillActive);

                           //�����I�����̃G�t�F�N�g�Đ�
                           _assaultCancelEffect.SetActive(true);
                           var cancelParticle = _assaultCancelEffect.GetComponent<ParticleSystem>();
                           cancelParticle.Play();
                           SoundManager.Instance.shotSe(SeType.AssaultSkillStop);

                           _assaultEffect.SetActive(false);
                       }
                   }).AddTo(this);
        }

        /// <summary>
        /// Assault�p�̃N�[���^�C���Ǘ��֐�
        /// </summary>
        /// <param name="skillTime"></param>
        /// <returns></returns>
        private IEnumerator CheckUseSkillAssault()
        {
            yield return null;
            myCharacterBaseData = GetComponent<CharacterBaseData>();
            myCharacterMove = GetComponent<CharacterMove>();
            _basicLeader = FindObjectOfType<BasicLeader>();
            //�X�L���̃N�[���^�C�������܂�܂őҋ@
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
                yield return new WaitUntil(() => AssaultSkillActive == false);//�X�L���̌��ʂ��؂��܂ŁiAssaultSkillActive��false�ɂȂ�܂Łj�ҋ@
                Debug.Log("�N�[���^�C���J�n");
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

