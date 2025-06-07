
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Fusion;

namespace Unit
{
    public class Taisoukou : HeroSkill
    {
        private CharacterBaseData MyCharacterBaseData;
        private CharacterStatus MyCharacterStatus;
        private Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        private RaycastHit hit;
        private bool TaisouSkillActive = false; //パッシブのオンオフのフラグ

        void Start()
        {
            MyCharacterStatus = GetComponent<CharacterStatus>();
            MyCharacterBaseData = GetComponent<CharacterBaseData>();
            StartCoroutine(CheckUseSkill(MyCharacterBaseData.MySkillTime));
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        ///パッシブスキルの処理を記述（使用する場所は任せます）
        /// </summary>
        public override void PassiveSkill()
        {
            TaisouSkillActive = true;
            //MyCharacterStatus.ArmorPiercing(TaisouSkillActive);    //ステータスを変更
            //パッシブクラスの内容を記述
            Debug.Log("耐装甲");
        }

        /// <summary>
        /// アクティブスキルの処理を記述
        /// </summary>
        public override void ActiveSkill()
        {
           
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.distance < 50)
                {
                    //敵判別
                    bool enemyHit = false;
                    if (hit.collider.gameObject.GetComponent<CharacterBaseData>())
                    {
                        enemyHit = hit.collider.gameObject.GetComponent<CharacterBaseData>().isHasInputAuthority();
                    }

                    //位置入替
                    if (enemyHit == false && hit.collider.gameObject.GetComponent<CharacterStatus>())
                    {
                        //5秒後にオブジェクトのHPを初期値にする
                        Invoke(nameof(SwapObj), 5);
                    }

                }
            }

            //スキルの内容を記述
            Debug.Log("耐装甲-active-");

        }

        //Invokeで使う
        void SwapObj()
        {
            Vector3 tmp = GameObject.Find("Taisoukou").transform.position;

            hit.collider.gameObject.transform.position = tmp;

            tmp = hit.point;

            GameObject.Find("Taisoukou").transform.position = tmp;
        }
    }
}