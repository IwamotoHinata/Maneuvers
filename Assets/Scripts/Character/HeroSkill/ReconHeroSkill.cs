using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements.Experimental;
using Fusion;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

namespace Unit
{
    public class ReconHeroSkill : HeroSkill
    {
        [SerializeField] private GameObject rotationObject; //子オブジェクト
        [SerializeField] private ReconScanBox scanBox; //子オブジェクトのスクリプト

        private Camera mainCamera;
        private CharacterBaseData MyCharacterBaseData;
        private GameObject _skillEffect;//スキルエフェクトを格納する変数
        private GameObject[] _effects = new GameObject[20];

        [SerializeField] private GameObject _skillGuide;
        private IDisposable _disposable;

        private Vector3 _mousePosPre;
        private bool _isCursorMove = false;

        void Start()
        {
            mainCamera = Camera.main;
            _skillEffect = (GameObject)Resources.Load("Effect/Recon/LightShaft");
            MyCharacterBaseData = GetComponent<CharacterBaseData>();
            StartCoroutine(CheckUseSkillRecon());
            _skillGuide.SetActive(false);
        }

        private void Update()
        {
            Vector3 mousePos = Input.mousePosition;
            if (mousePos == _mousePosPre)
                _isCursorMove = false;
            else
                _isCursorMove = true;

            _mousePosPre = mousePos;
        }

        /// <summary>
        ///パッシブスキルの処理を記述（使用する場所は任せます）
        /// </summary>
        public override void PassiveSkill()
        {
            //パッシブクラスの内容を記述
            Debug.Log("リコン");
        }

        /// <summary>
        /// アクティブスキルの処理を記述
        /// </summary>
        public override void ActiveSkill()
        {
            _effects = new GameObject[20];
            Debug.Log("リコン-active-");

            //方向指定
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 direction1 = (hit.point - transform.position).normalized;
                rotationObject.transform.forward = new Vector3(direction1.x, 0, direction1.z);
            }

            //敵への発見状態付与
            scanBox.ChangeEnemyVisible();

            //エフェクトを生成
            //マウス座標を取得し、Rayを飛ばす
            RaycastHit hit_dir;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit_dir);
            var direction2 = calVecter(transform.position, hit_dir.point);
            for (int i = 0; i < 20; i++)
            {
                _effects[i]= Instantiate(_skillEffect, transform.position + new Vector3(0, 10, 0) + direction2 * 20 * i, Quaternion.identity);
                Destroy(_effects[i], 5.0f);
            }
            SoundManager.Instance.shotSe(SeType.ReconSkill);
        }

        /// <summary>
        /// ユニットからRayの着地点への単位ベクトルを計算して返す関数
        /// </summary>
        /// <param name="myUnit">ユニットの座標</param>
        /// <param name="rayPoint">Rayの着地点の座標</param>
        /// <returns></returns>
        private Vector3 calVecter(Vector3 myUnit, Vector3 rayPoint)
        {
            //ベクトルの減算を行う。後はreturnの際に単位ベクトルに変換する
            var heading = rayPoint - myUnit;
            return heading.normalized;
        }

        private IEnumerator CheckUseSkillRecon()
        {
            yield return null;
            myCharacterBaseData = GetComponent<CharacterBaseData>();
            myCharacterMove = GetComponent<CharacterMove>();
            _basicLeader = FindObjectOfType<BasicLeader>();
            yield return new WaitUntil(() => myCharacterBaseData.MySkillTime != 0);
            float skillTime = myCharacterBaseData.MySkillTime;

            while (true)
            {
                yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.E) && myCharacterMove.ReturnIsSelect() && myCharacterBaseData.isHasInputAuthority()));
                {
                    myCharacterMove.isUseSkill = true;
                    SoundManager.Instance.shotSe(SeType.Skill);
                    _disposable = this.UpdateAsObservable().Where(_ => _isCursorMove == true)
                        .Subscribe(_ => ShowSkillGuide()).AddTo(this);
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1));
                    _disposable.Dispose();
                    _skillGuide.SetActive(false);
                    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse0))//スキル使用中止
                    {
                        myCharacterMove.isUseSkill = false;
                        Debug.Log("スキルの使用が中止されました。");
                    }
                    else if (Input.GetKeyDown(KeyCode.Mouse1))//スキル使用
                    { 
                        _basicLeader.ReduceUltPoint();
                        ActiveSkill();
                        RPC_SkillUsed();
                        myCharacterMove.isUseSkill = false;
                        yield return new WaitForSeconds(skillTime);
                        Debug.Log("リコンスキル使用可能");
                    }
                }
            }
        }
        
        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        void RPC_SkillUsed()
        {
            skillSubject.OnNext(MyCharacterBaseData.MySkillTime);
        }

        void ShowSkillGuide()
        {
            _skillGuide.SetActive(true);
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            _skillGuide.transform.LookAt(hit.point);
        }
    }
}