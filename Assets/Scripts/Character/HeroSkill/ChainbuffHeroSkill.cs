
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Unit
{
    public class ChainbuffHeroSkill : HeroSkill
    {
        //���̃X�N���v�g�̓`�F�C���o�t���j�b�g�����łȂ��`�F�C���o�t�̉e�����󂯂邷�ׂẴ��j�b�g�ɕt�^����邱�Ƃ�z�肵�č쐬���Ă��܂�
        //���̃��j�b�g�ƃ`�F�C�����邽�߂ɂ̓��j�b�g�̎q�I�u�W�F�N�g�Ƃ���"ChainbuffArea"�Ƃ�����̃I�u�W�F�N�g��u���A
        //�����"ChainbuffArea"�Ƃ����^�O�����A�R���|�[�l���g�Ƃ���collider������K�v������܂��B

        private CharacterStatus MyCharacterStatus;
        private CharacterBaseData MyCharacterBaseData;
        private float MaxHp;
        private float healValue = 3;
        private float healHp;
        private GameObject ChainAffectCharacter;
        private SphereCollider sc;
        private int BeforeChainbuffState;
        private int UnitsNum; //�������j�b�g�̐�
        private GameObject ChainCharacter;//�����̃`�F�C���o�t�G���A
        private ChainbuffHeroSkill thisCs; //���̃X�N���v�g���i�[����B
        private int ChainbuffUnitType;
        private OwnerType Ownertype;
        private int ChainbuffState;//�����̃`�F�C����ԁB�`�F�C���o�t���K�w�Ƃ��Ă݂����������ǂ̗����ʒu�ɂ��邩�𐔒l�Ŏ����B���l���������قǃ`�F�C���o�t�̑匳�ɋ߂��B
        private ChainbuffHeroSkill[] ChainUnitsCs; //���̃I�u�W�F�N�g�ƃ`�F�C�����Ă��郆�j�b�g��ChainAffectedHeroSkill�X�N���v�g���i�[
        private bool ChainbuffActive; //�������`�F�C���o�t��Ԃ��ۂ�
        private ChainbuffHeroSkill ChainAffectCharacterCs; //�`�F�C���o�t�������Ă��������̃`�F�C���o�t�Ɋւ���X�N���v�g
        private PlayerDeck deck;


        // Start is called before the first frame update
        void Start()
        {
            //�q�I�u�W�F�N�g�̃`�F�C���o�t�G���A���Ɍ����Ă���
            ChainCharacter = transform.Find("ChainbuffArea").gameObject;
            sc = ChainCharacter.GetComponent<SphereCollider>();
            thisCs = GetComponent<ChainbuffHeroSkill>();

            MyCharacterBaseData = GetComponent<CharacterBaseData>();
            MyCharacterStatus = GetComponent<CharacterStatus>();

            MyCharacterBaseData
            .OninitialSetting
            .Where(value => value == true)
            .Subscribe(_ =>
            {
                Init();
            }).AddTo(this);

            StartCoroutine(HpHealLoop());
            StartCoroutine(ChangeChainbuffState()); //�`�F�C���o�t��Ԃ̐��l�ύX����������`�F�C���o�t���j�b�g���������̐G��Ă���`�F�C���o�t��Ԃ̃��j�b�g�ɕύX��ʒB
        }

        private void Init()
        {

            deck = FindObjectOfType<PlayerDeck>();
            UnitsNum = deck.MyPlayerCharacterDeck.getSelectedDeck().Unit.Length;
            ChainUnitsCs = new ChainbuffHeroSkill[UnitsNum];

            MaxHp = MyCharacterBaseData.MaxHP; 
            ChainbuffUnitType = (int)UnitType.Chainbuff;

            //�`�F�C���o�t�̃��j�b�g���������̏���
            if (MyCharacterBaseData.MyCharacterID == ChainbuffUnitType)
            {
                sc.enabled = true;
                ChainbuffState = 0;
            }
            //��������Ȃ��������̏���
            else
            {
                sc.enabled = false;
                ChainbuffState = UnitsNum;
            }
            ChainbuffActive = false;
        }

        // Update is called once per frame
        void Update()
        {

        }


        private void OnTriggerEnter(Collider other)
        {

            //�����̃`�F�C���o�t�G���A�ȊO�̃`�F�C���o�t�G���A�ɐG�ꂽ��
            if (other.gameObject.CompareTag("ChainbuffArea") && other.transform.root.gameObject != this.gameObject)
            {

                Debug.Log("�V���ȃ`�F�C���o�t�ɐG�ꂽ");
                BeforeChainbuffState = ChainbuffState;
                ChainAffectCharacter = other.transform.root.gameObject; //�G�ꂽ�`�F�C���o�t�G���A�̐e�ł��郆�j�b�g���擾

                if (MyCharacterBaseData.GetCharacterOwnerType() != ChainAffectCharacter.GetComponent<CharacterBaseData>().GetCharacterOwnerType())
                    return;

                ChainAffectCharacterCs = ChainAffectCharacter.GetComponent<ChainbuffHeroSkill>();

                //�����G�ꂽ���肪���������`�F�C���o�t�̑匳�ɋ߂��Ȃ玩���̏�Ԃ�����������
                if (ChainAffectCharacterCs.ChainbuffState < ChainbuffState)
                {
                    ChainbuffState = ChainAffectCharacterCs.ChainbuffState + 1;
                }

                //�������`�F�C���o�t�G���A�ɐG��Ă��鑊����L�^����
                for (int i = 0; i < UnitsNum; i++)
                {
                    if (ChainUnitsCs[i] == null)
                    {
                        ChainUnitsCs[i] = ChainAffectCharacterCs;
                        break;
                    }
                }
                //�`�F�C���o�t��Ԃ̎��q�I�u�W�F�N�g��collider���A�N�e�B�u�ɂ���
                ColliderChange();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            //�����̃`�F�C���o�t�G���A�ȊO�̃`�F�C���o�t�G���A���痣���Ƃ�
            if (other.gameObject.CompareTag("ChainbuffArea") && other.transform.root.gameObject != this.gameObject)
            {
                //������G�ꂽ���肪�`�F�C���o�t��Ԃ���Ȃ������Ƃ���������߂�
                if (ChainbuffState == UnitsNum)
                    return;

                BeforeChainbuffState = ChainbuffState;
                ChainAffectCharacter = other.transform.root.gameObject; //�G�ꂽ�`�F�C���o�t�G���A�̐e�ł��郆�j�b�g���擾
                ChainAffectCharacterCs = ChainAffectCharacter.GetComponent<ChainbuffHeroSkill>();//����̃`�F�C���o�t��Ԃ��擾

                if (MyCharacterBaseData.GetCharacterOwnerType() != ChainAffectCharacter.GetComponent<CharacterBaseData>().GetCharacterOwnerType())
                    return;

                //�G��Ă�����̂��L�^����z�񂩂�O��
                for (int i = 0; i < UnitsNum; i++)
                {
                    if (ChainUnitsCs[i] == ChainAffectCharacterCs)
                    {
                        ChainUnitsCs[i] = null;
                        break;
                    }
                }

                ChainbuffStateChange();

                //���葤�ŏ����ł��ĂȂ����������ɂ���Ă���
                for (int i = 0; i < UnitsNum; i++)
                {
                    if (ChainAffectCharacterCs.ChainUnitsCs[i] == thisCs)
                    {
                        ChainAffectCharacterCs.ChainUnitsCs[i] = null;
                        ChainAffectCharacterCs.ExitChainStateChange();//�X�e�[�g�̏����������s���֐�
                    }

                }

                //�q�I�u�W�F�N�g��collider���A�N�e�B�u�ɂ���
                if (MyCharacterBaseData.MyCharacterID != ChainbuffUnitType)
                    ColliderChange();

            }
        }

        private void ChainbuffStateChange()
        {
            //�`�F�C���o�t�{�̂ŉ��ɂ��G��Ă��Ȃ���Ԃł���΃`�F�C����Ԃ���������i�`�F�C���o�t�G���A�͎c�����܂܁j
            if (MyCharacterBaseData.MyCharacterID == ChainbuffUnitType)
            {
                for (int i = 0; i < UnitsNum; i++)
                {
                    if (ChainUnitsCs[i] != null)
                        return;

                    if (i == UnitsNum - 1)
                    {
                        if(ChainbuffActive != false)
                        {
                            ChainbuffActive = false;
                            MyCharacterStatus.ChainbuffStaticHitRateChange(ChainbuffActive);
                            MyCharacterStatus.ChainbuffMoveHitRateChange(ChainbuffActive);
                        }
                    }
                }
            }

            else
            {
                int MinState = UnitsNum;
                //�G��Ă���`�F�C���o�t��Ԃ̂��̂�����΂���̉��̊K�w�̏�ԂɂȂ�A���ɂ��G��Ă��Ȃ���΃`�F�C���o�t��Ԃ̐��l���`�F�C�����Ă��Ȃ���Ԃ̐��l�ɂ���
                for (int i = 0; i < UnitsNum; i++)
                {
                    if (ChainUnitsCs[i] != null)
                    {
                        if (ChainUnitsCs[i].ChainbuffState < MinState)
                            MinState = ChainUnitsCs[i].ChainbuffState;
                    }

                    if (i == UnitsNum - 1)
                    {
                        if (MinState != UnitsNum) ChainbuffState = MinState + 1;

                        else ChainbuffState = UnitsNum;
                        break;
                    }
                }
            }
        }

        public void ExitChainStateChange()
        {

            BeforeChainbuffState = ChainbuffState;

            ChainbuffStateChange();

            //�q�I�u�W�F�N�g��collider���A�N�e�B�u�ɂ���
            if (MyCharacterBaseData.MyCharacterID != ChainbuffUnitType)
                ColliderChange();
        }

        private void ColliderChange()
        {
            //�`�F�C���o�t��Ԃ̎��q�I�u�W�F�N�g��collider���A�N�e�B�u�ɂ���
            if (ChainbuffState < UnitsNum && ChainbuffActive != true)
            {
                sc.enabled = true;
                ChainbuffActive = true;//�X�e�[�^�X��ύX
                MyCharacterStatus.ChainbuffStaticHitRateChange(ChainbuffActive);
                MyCharacterStatus.ChainbuffMoveHitRateChange(ChainbuffActive);
                Debug.Log("�`�F�C���o�t����");
            }
            //�q�I�u�W�F�N�g��collider���A�N�e�B�u�ɂ���
            else if (ChainbuffState == UnitsNum && ChainbuffActive != false)
            {
                sc.enabled = false;
                ChainbuffActive = false;
                MyCharacterStatus.ChainbuffStaticHitRateChange(ChainbuffActive);
                MyCharacterStatus.ChainbuffMoveHitRateChange(ChainbuffActive);
                Debug.Log("�`�F�C���o�t����");
            }
        }
        IEnumerator ChangeChainbuffState()
        {
            yield return new WaitUntil(() => BeforeChainbuffState != ChainbuffState);//�`�F�C���o�t�̏�Ԃ��ς������
            //Debug.Log("�A���J�n");
            for (int i = 0; i < UnitsNum; i++)
            {
                if (ChainUnitsCs[i] != null)
                {
                    // Debug.Log(" �A���O�@��ԕω��O�F" + Before + "��ԕω���F" + current);
                    ChainUnitsCs[i].ChainbuffStateChangeReport();//�G��Ă��閡���ɏ�Ԃ�ύX�������Ƃ�`����

                    if (ChainbuffState == UnitsNum)
                    {
                        // Debug.Log("�`�F�C���o�t��������");
                        ChainUnitsCs[i] = null;
                    }
                }
            }
            StartCoroutine(ChangeChainbuffState()); //�`�F�C���o�t��Ԃ̐��l�ύX����������`�F�C���o�t���j�b�g���������̐G��Ă���`�F�C���o�t��Ԃ̃��j�b�g�ɕύX��ʒB
        }

        //���̃��j�b�g�̃`�F�C���o�t��Ԃ̐��l�̕ύX���������ۂ��̕ϓ����L�^
        public void ChainbuffStateChangeReport()
        {
            
            //�������`�F�C���o�t���G��Ă������j�b�g���`�F�C���o�t��ԂłȂ�������
            for (int i = 0; i < UnitsNum; i++)
            {
                if (MyCharacterBaseData.MyCharacterID != ChainbuffUnitType)
                {
                    if (ChainUnitsCs[i] != null && ChainUnitsCs[i].ChainbuffState == UnitsNum)
                    {
                        ChainUnitsCs[i] = null;
                    }
                }
            }

            ChainbuffStateChange();

            if (ChainbuffState == UnitsNum)
            {
                for (int i = 0; i < UnitsNum; i++)
                {
                     if (ChainUnitsCs[i] != null)
                    {
                        ChainUnitsCs[i].ChainbuffStateChangeReport();
                        ChainUnitsCs[i] = null;
                    }
                }
                if (MyCharacterBaseData.MyCharacterID != ChainbuffUnitType)
                    ColliderChange();
            }
           // Debug.Log(" �L�^�ω��O�F" + BeforeChainbuffState + "�L�^�ω���F" + ChainbuffState);

        }
        

        //�`�F�C���o�t���ʂɂ�鎩����
        IEnumerator HpHealLoop()
        {
            yield return new WaitUntil(() => MyCharacterStatus.CurrentHp < MaxHp && ChainbuffActive);
            while (ChainbuffActive)
            {
                if (MyCharacterStatus.CurrentHp < MaxHp)
                {
                    healHp = MyCharacterStatus.CurrentHp + healValue;
                    if (healHp > MaxHp)
                        healHp = MaxHp;
                    MyCharacterStatus.ChangeHPValue(healHp);
                    //Debug.Log("�`�F�C���o�t�񕜒�");
                    yield return new WaitForSeconds(1);
                    if (healHp >= MaxHp) break;
                }
            }
            StartCoroutine(HpHealLoop());
        }

         

        private void OnDestroy()
        {
            for (int i = 0; i < UnitsNum; i++)
            {
                if (ChainUnitsCs[i] != null)
                {
                    for (int j = 0; j < UnitsNum; j++)
                    {
                        if (ChainUnitsCs[i].ChainUnitsCs[j] == thisCs)
                        {
                            ChainUnitsCs[i].ChainUnitsCs[j] = null;
                            ChainUnitsCs[i].ExitChainStateChange();//�X�e�[�g�̏����������s���֐�
                        }
                    }
                    ChainUnitsCs[i] = null;

                }

            }
        }
    }
}

