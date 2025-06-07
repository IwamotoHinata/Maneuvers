using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public class StealthHeroSkill : HeroSkill
    {
        //フィールド
        CharacterBaseData MyCharacterBaseData; //キャラデータ参照
        CharacterStatus MyCharacterStatus;
        private GameObject obj_detectionRange; //ステルス解除範囲
        private StealthDetectionRange cs_detectionRange;
        private bool stealth = false; //ステルス状態

        void Start()
        {
            //敵検知範囲生成
            obj_detectionRange = new GameObject("detectionRange");
            obj_detectionRange.transform.parent = gameObject.transform;
            obj_detectionRange.AddComponent<StealthDetectionRange>();
            cs_detectionRange = obj_detectionRange.GetComponent<StealthDetectionRange>();

            //コンポーネント取得
            MyCharacterStatus = GetComponent<CharacterStatus>();
            MyCharacterBaseData = GetComponent<CharacterBaseData>();
        }

        void Update()
        {
            //以下仮置き
            PassiveSkill();
            if (stealth) ActiveSkillManager();
        }

        /// <summary>
        /// パッシブスキル
        /// </summary>
        public override void PassiveSkill()
        {
            //Debug.Log("ステルス-passive-");

            //攻撃力変更
            //Debug.Log("attackPower : " + attackPower);
            float attackPower = MyCharacterStatus.CurrentHp / MyCharacterBaseData.MaxHP * MyCharacterStatus.CurrentAttackPower;
            MyCharacterStatus.ChangeAttackPower(attackPower);
        }

        /// <summary>
        /// アクティブスキル
        /// </summary>
        public override void ActiveSkill()
        {
            //Debug.Log("ステルス-active-");

            //不可視状態（自身）
            if (MyCharacterBaseData.MaxHP - MyCharacterStatus.CurrentHp == 0)
            {
                //Debug.Log("ステルスのHPが最大->ステルス状態");
                stealth = true; //ステルス状態
                MyCharacterStatus.ChangeMoveSpeed(6.0f); //移動速度変更
            }
        }

        /// <summary>
        /// アクティブスキルの監視（終了判定）
        /// stealth = true　の時に発行
        /// </summary>
        private void ActiveSkillManager()
        {
            //終了判定
            if (cs_detectionRange.getUnitNum() != 0)
            {
                //Debug.Log("敵が１０ｍ以内");
                stealth = false;
            }
            else if (MyCharacterStatus.CurrentIVisibleChanged)
            {
                //Debug.Log("索敵された");
                stealth = false;
            }
            else if (MyCharacterBaseData.MaxHP - MyCharacterStatus.CurrentHp != 0)
            {
                //Debug.Log("ダメージを受けた（HP減少）");
                stealth = false;
            }

            //ステルス解除
            if (!stealth)
            {
                //Debug.Log("ステルス解除");
                MyCharacterStatus.ChangeMoveSpeed(10.0f); //移動速度変更
            }
        }

        //変数アクセス
        public bool GetStealthState()
        {
            return stealth;
        }
    }
}
