using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using Fusion;

namespace Unit
{
    /// <summary>
    /// スキルに関する継承クラス
    /// </summary>
    public class HeroSkill : SkillAbstruct,IDisposable
    {
        public BasicLeader _basicLeader;
        public CharacterBaseData myCharacterBaseData;
        public CharacterMove myCharacterMove;
        
        public IObservable<float> skillObservable => skillSubject;
        protected Subject<float> skillSubject = new Subject<float>();

        /// <summary>
        ///パッシブスキルの処理を記述（使用する場所は任せます）
        /// </summary>
        public override void PassiveSkill()
        {

        }

        /// <summary>
        /// アクティブスキルの処理を記述
        /// </summary>
        public override void ActiveSkill()
        {
        }
        
        /// <summary>
        /// アクティブスキルを使用する為のコルーチン、Inputautolityを持っているうえでセレクト状態だと発動します。
        /// </summary>
        /// <returns></returns>
        protected IEnumerator CheckUseSkill(float skillTime)
        {
            Debug.Log("継承元のものが起動");
            yield return null;
            myCharacterBaseData = GetComponent<CharacterBaseData>();
            myCharacterMove = GetComponent<CharacterMove>();
            //_basicLeader = GameObject.Find("Leader").GetComponent<BasicLeader>();
            while (true)
            {
                yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.E) && myCharacterMove.ReturnIsSelect() && myCharacterBaseData.isHasInputAuthority()));
                {
                    //_basicLeader.ReduceUltPoint();
                    ActiveSkill();
                    SoundManager.Instance.shotSe(SeType.Skill);
                    //Debug.Log("スキル使った！！+スキルの仕様にかかる時間は"　+ skillTime);
                }
                yield return new WaitForSeconds(skillTime);
            }
        }

        //オブジェクトが破壊された時にUniRxの購読を中止する
        public void Dispose()
        {
            skillSubject.Dispose();
        }
    }
}
