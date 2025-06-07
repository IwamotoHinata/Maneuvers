using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Fusion;
using System.Linq;

namespace Unit
{
    struct UnitSearchElement
    {
        public GameObject unitGameObject;
        public CharacterStatus unitCharacterStatus;
        public bool undiscovered;
        public bool discovered;
    }

    public class CharacterVisible : NetworkBehaviour
    {
        [Header("基本データの参照")]
        [SerializeField]
        private CharacterBaseData MyCharacterBaseData;
        [SerializeField]
        CharacterStatus MyCharacterStatus;
        [SerializeField]
        private GameObject[] _VisibleObjects;
        private int _enemyCount = 5;//エネミーの量
        [SerializeField]private List<UnitSearchElement> _allEnemy = new List<UnitSearchElement>();//すべての敵
        private List<CharacterSpawner> _characterSpawner = new List<CharacterSpawner>();
        private bool _canLoopAction = false;
        private bool _inBush = false;
        private GameObject _inBushObject;
        void Start()
        {
            MyCharacterBaseData
                .OninitialSetting
                .Where(value => value)
                .Subscribe(_ =>
                {
                    //Debug.Log("CharacterVisible:初期値の設定がされました");
                    //Debug.LogWarning("StateAuthority = " + MyCharacterBaseData.isHasStateAuthority() + "InputAuthority" + MyCharacterBaseData.isHasInputAuthority());
                    StartCoroutine(Init());
                })
                .AddTo(this);
            foreach (var Objects in _VisibleObjects)//リスポーン直後に見えてしなっているのを修正
            {
                if (MyCharacterBaseData.isHasInputAuthority())
                {
                    Objects.SetActive(true);
                }
                else
                {
                    Objects.SetActive(false);
                }
            }
        }

        IEnumerator Init()
        {
            SetEnemySpawner();
            StartCoroutine(searchSystemloop());//索敵の仕組み
            yield return new WaitForSeconds(2f);

            if (MyCharacterBaseData.isHasInputAuthority())//
            {
                MyCharacterStatus.Idiscovered(false);
            }
        }

        public void Visible(bool value)//実際の見た目をつかさどる
        {
            if (!MyCharacterBaseData.isHasInputAuthority())
            {
                foreach (var Objects in _VisibleObjects)
                {
                    Objects.SetActive(value);
                }
            }
        }

        /// <summary>
        /// allEnemyの敵に対して索敵範囲内かの確認、索敵範囲内の場合は未発見の敵（undiscoveredEnemy）に格納
        /// </summary>
        IEnumerator searchSystemloop()//基本的な主要ループ
        {
            while (true)
            {
                if (MyCharacterStatus.GetCharacterState().Equals(global::CharacterState.Dead)) yield break; //死んでたら処理中断
                yield return new WaitUntil(() => _canLoopAction);//何かの理由で止めたいとき用


                //if (MyCharacterBaseData.isHasInputAuthority()) Debug.LogWarning("Visible_allenemyNum" + _allEnemy.Count);
                if (MyCharacterBaseData.isHasStateAuthority())//所有者のみ判定中->ホストのみ
                {
                    if (_allEnemy.Count != _enemyCount)
                    {
                        SetAllEnemy();
                    }
                    for (int i = 0; i < _allEnemy.Count; i++)
                    {
                        var item = _allEnemy[i];
                        if (item.unitGameObject != null && item.undiscovered.Equals(false))
                        {
                            if (Vector3.Distance(transform.position, item.unitGameObject.transform.position) <= item.unitCharacterStatus.CurrenSerchrange)//ここの距離が怪しい、自分を発見しうる敵か確認中
                            {
                                item.undiscovered = true;
                            }
                        }
                        if (item.undiscovered.Equals(true) && item.discovered.Equals(false))//すでに自分を発見済みの敵であれば視界確認をパス
                        {
                            if (checkSightPass(item.unitGameObject) && item.unitGameObject != null)//視界確認
                            {
                                item.discovered = true;
                            }
                        }
                        if (item.discovered.Equals(true))//自身を発見できる敵が1以上いたら
                        {

                            if (MyCharacterBaseData.MyUnitType.Equals(UnitType.Stealth)) //ステルスの特殊判定
                            {
                                var stealthHeroSkill_CS = GetComponent<StealthHeroSkill>();
                                if (stealthHeroSkill_CS.GetStealthState())
                                {
                                    MyCharacterStatus.Idiscovered(false);
                                }
                            }
                            else //その他のキャラ
                            {
                                MyCharacterStatus.Idiscovered(true);//敵から見つかっている場合自分を表示
                                break;
                            }
                        }
                        else
                        {
                            MyCharacterStatus.Idiscovered(false);//敵から見つかっていない場合自分を非表示
                        }
                    }

                    yield return new WaitForSeconds(0.2f);
                    resetList();//
                }
                else { yield break; }
            }
        }
        public void SetEnemySpawner()
        {
            var _allSpawner = FindObjectsOfType<CharacterSpawner>();
            while (_characterSpawner.Count < _enemyCount)
            {
                foreach (var item in _allSpawner)
                {
                    if (_characterSpawner.Contains(item))
                    {
                        continue;//追加することが可能なユニット
                    }
                    if (!item.HasInputAuthority && MyCharacterBaseData.isHasInputAuthority())
                    {
                        _characterSpawner.Add(item);//相手側のユニットをぶち込んでいる。
                    }
                    else if (item.HasInputAuthority && !MyCharacterBaseData.isHasInputAuthority())
                    {
                        _characterSpawner.Add(item);
                    }
                }
            }
            SetAllEnemy();
        }
        /// <summary>
        /// 未発見の敵全体を取得
        /// </summary>
        public void SetAllEnemy()
        {
            foreach (var item in _characterSpawner)
            {
                if (!item.UnitAlive) continue;
                var _tmpUnit = new UnitSearchElement();
                _tmpUnit.unitGameObject = item.SpawnedUnit;
                _tmpUnit.unitCharacterStatus = item.SpawnedUnit.GetComponent<CharacterStatus>();
                _tmpUnit.undiscovered = false;
                _tmpUnit.discovered = false;

                if (_allEnemy.Contains(_tmpUnit)) continue;//追加することが可能なユニット
                _allEnemy.Add(_tmpUnit);//相手側のユニットをぶち込んでいる。
            }
            _canLoopAction = true;
        }
        /// <summary>
        /// 対象を視認できるかどうか。壁およびブッシュの計算
        /// </summary>
        private bool checkSightPass(GameObject checkTarget)//Rayの処理軍団
        {
            try
            { 
                //障害物判定用のrayの数値
                var _diff = transform.position - checkTarget.transform.position;
                var _distance = _diff.magnitude;
                var _direction = _diff.normalized;

                List<LagCompensatedHit> _hits = new List<LagCompensatedHit>();
                var layerMask = LayerMask.GetMask("Stage");//壁を検出
                Runner.LagCompensation.RaycastAll(checkTarget.transform.position + new Vector3(0, 15, 0), _direction, _distance, player: Object.InputAuthority, _hits, layerMask);//これがRay、Fusin推しの奴。_hitsに情報格納
                if (_hits.Count > 0) //１つでも壁に当たっていたら
                {
                    Debug.DrawRay(checkTarget.transform.position + new Vector3(0, 15.5f, 0), _direction * _distance, Color.blue, 1);
                    return false;
                }

                if (_distance < 25)
                {
                    Debug.DrawRay(checkTarget.transform.position + new Vector3(0, 15.5f, 0), _direction * _distance, Color.yellow, 1);
                    return true;
                }

                var _bushhits = new List<LagCompensatedHit>();
                //Debug.LogWarning("リセット後の配列の数" + _bushhits.Count);
                var BushMask = LayerMask.GetMask("Bush");//草を検出
                Runner.LagCompensation.RaycastAll(checkTarget.transform.position + new Vector3(0, 15, 0), _direction, _distance, player: Object.InputAuthority, _bushhits, BushMask);//これがRay、Fusin推しの奴。_hitsに情報格納
                if (_bushhits.Count > 1)//命中物2つ以上あるか=間に草があるか
                {
                    Debug.DrawRay(checkTarget.transform.position + new Vector3(0, 15.5f, 0), _direction * _distance, Color.green, 1);
                    return false;
                }
                else if (_bushhits.Count.Equals(1))
                {
                    if (Math.Abs((_bushhits[0].GameObject.transform.position - transform.position).x) < 6.3f && Math.Abs((_bushhits[0].GameObject.transform.position - transform.position).y) < 6.3f && MyCharacterStatus.attacked)
                    {
                        Debug.DrawRay(checkTarget.transform.position + new Vector3(0, 15.5f, 0), _direction * _distance, Color.yellow, 1);
                        return true;
                    }
                    else
                    {
                        Debug.DrawRay(checkTarget.transform.position + new Vector3(0, 15.5f, 0), _direction * _distance, Color.green, 1);
                        return false;
                    }
                }
                Debug.DrawRay(checkTarget.transform.position + new Vector3(0, 15.5f, 0), _direction * _distance, Color.yellow, 1);            //デバック用のDrawRay
                return true;

            }
            catch (NullReferenceException e)
            {
                Debug.LogWarning("攻撃対象が参照できませんでした");
                return false;
                throw;
            }
           
        }

        /// <summary>
        /// すべてのListの格納されているオブジェクトの整理
        /// </summary>
        private void resetList()
        {
            _allEnemy.RemoveAll(s => s.unitGameObject == null);//allEnemyのList内でnullを削除する

            //視界が通っていない場合はdiscoveredEnemyから削除する
            for (int i = 0; i < _allEnemy.Count; i++)
            {
                UnitSearchElement item = _allEnemy[i];
                if (Vector3.Distance(this.transform.position, item.unitGameObject.transform.position) >= item.unitCharacterStatus.CurrenSerchrange)
                {
                    item.undiscovered = false;
                    item.discovered = false;
                }
                else if (!checkSightPass(item.unitGameObject))
                {
                    item.discovered = false;
                }
            }

        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Bush"))
            {
                _inBushObject = col.gameObject;
                _inBush = true;
            }
        }
        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Bush"))
            {
                _inBushObject = null;
                _inBush = false;
            }
        }
    }
}