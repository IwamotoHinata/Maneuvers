using UnityEngine;
using UniRx;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using Fusion;

namespace Unit
{
    public class DisplayProfile : NetworkBehaviour
    {
        [Header("�X�e�[�^�X��\��TMP")]
        [SerializeField]
        private TextMeshProUGUI CharacterStateTMP;
        [Header("���j�b�g�Ɋւ���摜")]
        [SerializeField]
        private Image InSelectImage;
        [SerializeField]
        private Image CharacterAttackAreaImage;
        [SerializeField]
        private Image CharacterSearchAreaImage;
        [SerializeField]
        private Image UnitImage;
        [Header("HP�o�[")]
        [SerializeField]
        private Image HpBar;
        private float MaxHP;
        [Header("��ɃJ���������ɂȂ�I�u�W�F�N�g")]
        [SerializeField]
        private GameObject LookAtCameraObject;
        CharacterBaseData MyCharacterBaseData;
        CharacterStatus MyCharacterStatus;
        CharacterMove MyCharacterMove;
        private GameObject Camera;
        private List<Sprite> IconList;

        private void FixedUpdate()
        {
            //LookAtCameraObject.transform.LookAt(Camera.transform, transform.up);
            LookAtCameraObject.transform.rotation = Camera.transform.rotation;
        }
        public void Start()
        {
            Camera = FindObjectOfType<Camera>().gameObject;
            MyCharacterBaseData = GetComponentInParent<CharacterBaseData>();
            MyCharacterStatus = GetComponentInParent<CharacterStatus>();
            MyCharacterMove = GetComponentInParent<CharacterMove>();
            InSelectImage.color = Color.clear;

            MyCharacterBaseData
                .OninitialSetting
                .Where(value => value == true)
                .Subscribe(_ =>
                {
                    Debug.Log("DisplayProfile�����l�̐ݒ肪����܂���");
                    Init();
                })
                .AddTo(this);

            if (MyCharacterBaseData.isHasInputAuthority())
            {
                CharacterAttackAreaImage.gameObject.SetActive(true);
                CharacterSearchAreaImage.gameObject.SetActive(true);
            }
        }
        private void Init()
        {
            //�A�C�R���̕ύX
            setUnitImage();
            //�\���������G�͈͂ƍU���͈͂̓K��


            //���ݑ̗͂̓���
            MaxHP = MyCharacterBaseData.MaxHP;

            MyCharacterStatus
                .OnCharacterHPChanged
                .Subscribe(characterHP =>
                {
                    float currentHP = characterHP / MaxHP;
                    //Debug.Log($"SetHp(currentHP) = {currentHP}");
                    SetHp(currentHP);
                }
                ).AddTo(this);


            //���ݏ�Ԃ̓���
            MyCharacterStatus
                .OnCharacterStateChanged
                .Subscribe(CharacterState =>
                {
                    CharacterStateTMP.text = CharacterState.ToString();
                }
            ).AddTo(this);

            //�I�����ꂽ�ۂɁA���ꂪ������悤�ɂ���Image��\��
            MyCharacterMove
                .isSelect
                .Subscribe(SelectValue =>
                {
                    if (SelectValue == true)
                    {
                        InSelectImage.color = Color.green;
                        CharacterAttackAreaImage.color = Color.red;
                        CharacterSearchAreaImage.color = Color.yellow;
                    }
                    else
                    {
                        InSelectImage.color = Color.clear;
                        CharacterAttackAreaImage.color = Color.clear;
                        CharacterSearchAreaImage.color = Color.clear;
                    }
                }
            ).AddTo(this);
        }


        public void setAttackRangeImage(float AttackRange)
        {
            Vector2 AttackAreaSize = new Vector2(AttackRange * 20, AttackRange * 20);
            CharacterAttackAreaImage.rectTransform.sizeDelta = AttackAreaSize;
        }
        public void setSearchRangeImage(float SearchRange)
        {
            Vector2 SearchAreaSize = new Vector2(SearchRange * 20, SearchRange * 20);
            CharacterSearchAreaImage.rectTransform.sizeDelta = SearchAreaSize;
        }


        /// <summary>
        /// ���j�b�g�C���[�W��ݒ�
        /// </summary>
        private void setUnitImage()
        {
            UnitImage.color = HasInputAuthority ? new Color32(121, 166 , 242, 200) : new Color32(242, 137, 121, 200);
        }

        //�l�b�g���[�N����ď�Ԃ�\���e�L�X�g���X�V
        public void StateShow(string state)
        {
            CharacterStateTMP.text = state;
        }

        //�̗͂�\���e�L�X�g���X�V
        public void SetHp(float currentHP)
        {
            HpBar.fillAmount = currentHP;
        }
    }
}