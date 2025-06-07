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
        [Header("ステータスを表すTMP")]
        [SerializeField]
        private TextMeshProUGUI CharacterStateTMP;
        [Header("ユニットに関する画像")]
        [SerializeField]
        private Image InSelectImage;
        [SerializeField]
        private Image CharacterAttackAreaImage;
        [SerializeField]
        private Image CharacterSearchAreaImage;
        [SerializeField]
        private Image UnitImage;
        [Header("HPバー")]
        [SerializeField]
        private Image HpBar;
        private float MaxHP;
        [Header("常にカメラ方向になるオブジェクト")]
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
                    Debug.Log("DisplayProfile初期値の設定がされました");
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
            //アイコンの変更
            setUnitImage();
            //表示される索敵範囲と攻撃範囲の適応


            //現在体力の同期
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


            //現在状態の同期
            MyCharacterStatus
                .OnCharacterStateChanged
                .Subscribe(CharacterState =>
                {
                    CharacterStateTMP.text = CharacterState.ToString();
                }
            ).AddTo(this);

            //選択された際に、それが分かるようにするImageを表示
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
        /// ユニットイメージを設定
        /// </summary>
        private void setUnitImage()
        {
            UnitImage.color = HasInputAuthority ? new Color32(121, 166 , 242, 200) : new Color32(242, 137, 121, 200);
        }

        //ネットワークを介して状態を表すテキストを更新
        public void StateShow(string state)
        {
            CharacterStateTMP.text = state;
        }

        //体力を表すテキストを更新
        public void SetHp(float currentHP)
        {
            HpBar.fillAmount = currentHP;
        }
    }
}