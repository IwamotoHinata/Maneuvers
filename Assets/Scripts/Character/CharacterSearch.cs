using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using Fusion;
using System;

namespace Unit
{
    public class CharacterSearch : NetworkBehaviour
    {
        [Header("参照元")]
        [SerializeField]
        CharacterBaseData MyCharacterBaseData;
        [SerializeField]
        CharacterStatus MyCharacterStatus;
        private LogManager AttackLogManager;
        private int _enemyCount = 5;//エネミーの量

        private bool canLoopAction = false;//ループ処理が可能かどうか


        private float _attackRange;
        private List<GameObject> _allEnemy = new List<GameObject>();
        private List<GameObject> _inAttackRangeEnemy = new List<GameObject>();
        private List<CharacterSpawner> _characterSpawner = new List<CharacterSpawner>();
        private GameObject _attackTargetObject = null;
        private GameObject _lastAttackTargetObject = null;
        private bool _isInit = false;
        private void Awake()
        {
            AttackLogManager = FindObjectOfType<LogManager>();
            MyCharacterBaseData
                .OninitialSetting
                .Where(value => value)
                .Subscribe(_ =>
                {
                    if (!_isInit)
                    {
                        Init();
                    }
                })
                .AddTo(this);
        }
        /// <summary>
        /// allEnemyの敵に対して索敵範囲内かの確認、索敵範囲内の場合は未発見の敵（undiscoveredEnemy）に格納
        /// </summary>
        IEnumerator searchSystemloop()//常に回っている
        {
            yield return new WaitForSeconds(1f);
            SetEnemySpawner();
            while (true)
            {
                if (MyCharacterBaseData.HasStateAuthority)
                {
                    if (MyCharacterStatus.GetCharacterState().Equals(global::CharacterState.Dead)) yield break; //死んでたら処理中断

                    yield return new WaitUntil(() => canLoopAction);//続けていいかを確認する

                    if (_allEnemy.Count != _enemyCount)
                    {
                        SetAllEnemy();
                    }
                    sortList(); //Listの整理、allenemyからnullを消し、_inAttackRangeenemyの中から範囲外の敵を消す。
                    if (_inAttackRangeEnemy.Count >= 2)//敵を順番に並べる
                    {
                        //敵を近い順番通りに並べる
                        for (int j = 0; j < _inAttackRangeEnemy.Count; j++)
                        {
                            for (int k = j + 1; k < _inAttackRangeEnemy.Count; k++)
                            {
                                if (Vector3.Distance(transform.position, _inAttackRangeEnemy[j].transform.position)
                                    > Vector3.Distance(transform.position, _inAttackRangeEnemy[k].transform.position))
                                {
                                    var _temp = _inAttackRangeEnemy[j];
                                    _inAttackRangeEnemy[j] = _inAttackRangeEnemy[k];
                                    _inAttackRangeEnemy[k] = _temp;
                                }
                            }
                        }
                    };
                    //最も近い敵にパスがあるか確認...パスなければ次に...繰り返し
                    _attackTargetObject = null;
                    foreach (var item in _inAttackRangeEnemy)
                    {
                        if (checkSightPass(item))//近い順位攻撃可能か考えて可能であれば抜けるループを抜ける
                        {
                            _attackTargetObject = item;
                            break;
                        }
                    }
                    if (_lastAttackTargetObject != _attackTargetObject)
                    {//目標更新
                        MyCharacterStatus.SetTarget(_attackTargetObject);
                        _lastAttackTargetObject = _attackTargetObject;
                    }
                    //if (MyCharacterBaseData.isHasInputAuthority()) Debug.LogWarning("リストをリセット");
                    yield return new WaitForSeconds(0.2f);
                }
                else { yield return new WaitForSeconds(0.2f); }
            }
        }

        /// <summary>
        /// 初期値の入力
        /// </summary>
        void Init()
        {
            _isInit = true;
            _attackRange = MyCharacterBaseData.MyattackRange;
            StartCoroutine(searchSystemloop());
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
            Debug.Log("SetEnemySpawner");
            SetAllEnemy();
        }
        /// <summary>
        /// 未発見の敵全体を取得
        /// </summary>
        void SetAllEnemy()
        {
            foreach (var spawner in _characterSpawner)
            {
                if (!spawner.UnitAlive) continue;
                var item = spawner.SpawnedUnit;
                if (_allEnemy.Contains(item)) continue;
                _allEnemy.Add(item);
            }

            canLoopAction = true;
            Debug.Log("SetAllEnemy");
        }

        /// <summary>
        /// 対象を視認できるかどうか。壁およびブッシュの計算
        /// </summary>
        private bool checkSightPass(GameObject checkTarget)
        {
            try
            {
                if (checkTarget == null) return false;
                //if (MyCharacterBaseData.isHasInputAuthority()) Debug.LogWarning("chhecktarget start");
                //障害物判定用のrayの数値
                var _diff = checkTarget.transform.position - transform.position;
                var _distance = _diff.magnitude;
                var _direction = _diff.normalized;
                //対象が非表示の場合はfalseを返す
                if (checkTarget.TryGetComponent<CharacterStatus>(out CharacterStatus status))
                {
                    if (!status.CurrentIVisibleChanged)
                    {
                        //if (MyCharacterBaseData.isHasInputAuthority()) Debug.LogWarning("非表示なので攻撃不能");
                        return false;
                    }
                }
                else
                {
                    //if (MyCharacterBaseData.isHasInputAuthority()) Debug.LogWarning("ユニットでないため攻撃不能");
                    return false;
                }

                //ここから先の処理はすべて、ユニットであり発見されている事に注意

                //間に壁があるか確認
                var layerMask = LayerMask.GetMask("Stage");
                List<LagCompensatedHit> _hits = new List<LagCompensatedHit>();
                Runner.LagCompensation.RaycastAll(transform.position + new Vector3(0, 15, 0), _direction, _distance + 1, player: Object.InputAuthority, _hits, layerMask);
                //Debug.LogWarning("確認した草配列の数" + _hits.Count);
                if (_hits.Count > 0) //１つでも壁に当たっていたら
                {
                    //if (MyCharacterBaseData.isHasInputAuthority()) Debug.LogWarning("壁のせいで攻撃不能");
                    Debug.DrawRay(checkTarget.transform.position + new Vector3(0, 15.5f, 0), _direction * _distance, Color.red, 1);
                    return false;
                }

                //草無視25m内部にいるかどうか
                if (_distance < 25)
                {
                    //if (MyCharacterBaseData.isHasInputAuthority()) Debug.LogWarning("25mいてで草にかかわらず攻撃可能");
                    return true;
                }

                //if (MyCharacterBaseData.isHasInputAuthority()) Debug.LogWarning("壁も草もなく攻撃可能");
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
        /// すべてのListについてnullのものをListから削除
        /// </summary>
        private void sortList()
        {
            _inAttackRangeEnemy = new List<GameObject>(_allEnemy);//すべての敵をいったん射程内と考える
            List<GameObject> tmpList = new List<GameObject>();//削除する項目を一時的に保存
            //敵が攻撃範囲ではない場合は攻撃範囲の敵から削除する
            foreach (var item in _inAttackRangeEnemy)
            {
                if (item == null)
                {
                    tmpList.Add(item);
                }
                else if (Vector3.Distance(transform.position, item.transform.position) > _attackRange || item.GetComponent<CharacterBaseData>().MyHp <= 0)
                {
                    tmpList.Add(item);
                }
            }
            foreach (var item in tmpList)//保存した項目に従って削除
            {
                _inAttackRangeEnemy.Remove(item);
            }
            tmpList.Clear();

            //allEnemyでnullを削除する
            _allEnemy.RemoveAll(s => s == null);
        }

        /// <summary>
        /// 対象の体力が0以上か
        /// </summary>
        bool itemAlive(GameObject item)
        {
            if (item.GetComponent<CharacterBaseData>().MyHp > 0)
                return true;
            else
                return false;
        }
    }
}
