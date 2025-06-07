using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

namespace Unit
{
    public class DebufferHeroSkill : HeroSkill
    {
        private GameObject _debuffZone; //デバフを行う範囲を可視化するプレハブ
        [SerializeField] private GameObject _debuffArea; //デバフが設置できる範囲

        [Tooltip("デバフゾーンを設置できる射程")] [SerializeField]
        private float _debufflength;

        [SerializeField]
        private Image _skillGuide;
        private IDisposable _disposable;

        private Vector3 _mousePosPre;
        private bool _isCursorMove = false;

        void Start()
        {
            //オブジェクトを取得
            _debuffZone = (GameObject) Resources.Load("Effect/Debuffer/Debuffer");
            StartCoroutine(checkUseSkillDebuffer());
            _skillGuide.enabled = false;
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
            Debug.Log("デバッファー");
        }

        /// <summary>
        /// アクティブスキルの処理を記述
        /// </summary>
        public override void ActiveSkill()
        {
            debuffSkill();
            Debug.Log("デバッファー-active-");
        }

        private void debuffSkill()
        {
            //範囲の消去
            _debuffArea.SetActive(false);
            SoundManager.Instance.shotSe(SeType.DebufferSkill);
        }

        /// <summary>
        /// デバフゾーンを生成させる関数
        /// </summary>
        /// <param name="position">生成させる座標</param>
        /// <param name="info"></param>
        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        public void RPC_SpawnDebuffZone(Vector3 position, RpcInfo info = default)
        {
            Runner.Spawn(_debuffZone, new Vector3(position.x, this.transform.position.y, position.z),
                Quaternion.Euler(0f, 0f, 0f), Object.InputAuthority);
        }


        private float CharacterToTragetDistance(Vector3 point) //キャラクターと設置予定地点までの距離
        {
            float xRange = Mathf.Abs(transform.position.x - point.x);
            float zRange = Mathf.Abs(transform.position.z - point.z);
            var distance = Mathf.Sqrt(Mathf.Pow(xRange, 2) + Mathf.Pow(zRange, 2));
            Debug.Log("距離：" + distance);
            return distance;
        }

        private IEnumerator checkUseSkillDebuffer()
        {
            yield return null;
            myCharacterBaseData = GetComponent<CharacterBaseData>();
            myCharacterMove = GetComponent<CharacterMove>();
            _basicLeader = FindObjectOfType<BasicLeader>();
            yield return new WaitUntil(() => myCharacterBaseData.MySkillTime != 0);
            float skillTime = myCharacterBaseData.MySkillTime;

            while (true)
            {
                yield return new WaitUntil(() =>
                    (Input.GetKeyDown(KeyCode.E) && myCharacterMove.ReturnIsSelect() &&
                     myCharacterBaseData.isHasInputAuthority()));
                {
                    //デバフ範囲の可視化
                    _debuffArea.SetActive(true);
                    RectTransform rect = _debuffArea.GetComponent<RectTransform>();
                    rect.sizeDelta = new Vector2(_debufflength * 10, _debufflength * 10);
                    _disposable = this.UpdateAsObservable().Where(_ => _isCursorMove == true)
                        .Subscribe(_ => ShowSkillGuide()).AddTo(this);
                    myCharacterMove.isUseSkill = true;
                    SoundManager.Instance.shotSe(SeType.Skill);
                    yield return new WaitUntil(() =>
                        Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse0) ||
                        Input.GetKeyDown(KeyCode.Mouse1));
                    _disposable.Dispose();
                    _skillGuide.enabled = false;
                    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse0)) //スキル使用中止
                    {
                        _debuffArea.SetActive(false);
                        myCharacterMove.isUseSkill = false;
                        Debug.Log("スキルの使用が中止されました。");
                    }
                    else if (Input.GetKeyDown(KeyCode.Mouse1)) //スキル使用
                    {
                        Vector3 mousePosition = Input.mousePosition;
                        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
                        RaycastHit hit;
                        Physics.Raycast(ray, out hit);
                        if (CharacterToTragetDistance(hit.point) <= _debufflength)
                        {
                            _basicLeader.ReduceUltPoint();
                            RPC_SpawnDebuffZone(hit.point);
                            ActiveSkill();
                            RPC_SkillUsed();
                            myCharacterMove.isUseSkill = false;
                            yield return new WaitForSeconds(skillTime);
                            Debug.Log("debufferスキル使用可能");
                        }
                        else
                        {
                            _debuffArea.SetActive(false);
                            Debug.Log("範囲外です。スキル使用中止");
                        }
                    }
                }
            }
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        void RPC_SkillUsed()
        {
            skillSubject.OnNext(myCharacterBaseData.MySkillTime);
        }

        void ShowSkillGuide()
        {
            _skillGuide.enabled = true;
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            _skillGuide.transform.position = hit.point;
            Color guideColor = Color.blue;
            float _alpha;
            if (CharacterToTragetDistance(hit.point) <= _debufflength)
            {
                _alpha = 1;
            }
            else
            {
                _alpha = 0.3f;
            }
            _skillGuide.color = new Color(guideColor.r, guideColor.g, guideColor.b, _alpha);
        }
    }
}