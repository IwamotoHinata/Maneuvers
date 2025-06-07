using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class MultSelect_offline : MonoBehaviour
    {
        private Camera mainCamera;
        private Vector3 selectStartPos; //範囲指定
        private Vector3 selectEndPos;

        void Start()
        {
            //コンポーネント設定
            mainCamera = Camera.main;

            //初期設定
            StartCoroutine(Select());
        }

        /// <summary>
        /// ユニット範囲選択
        /// </summary>
        /// <returns></returns>
        private IEnumerator Select()
        {
            while (true)
            {
                //範囲選択の開始地点
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
                selectStartPos = Input.mousePosition;

                //範囲選択の終了地点
                yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Mouse0));
                selectEndPos = Input.mousePosition;

                //Debug.Log(selectStartPos);
                //Debug.Log(selectEndPos);

                //ある程度の範囲選択
                if (SelectAreaValue() > 10)
                {
                    //範囲内ユニットの判定
                    var UnitObjects = FindObjectsOfType<UnitBaseData_offline>(); //シーン上のUnitBaseData.csをすべて取る
                    List<UnitStatus_offline> SelectedObjects = new List<UnitStatus_offline>();
                    foreach (var no in UnitObjects)
                    {
                        if (InSelectArea(no.gameObject))
                        {
                            //Debug.Log("選択範囲内にユニットがいる");

                            if (no.isHasInputAuthority())
                            {
                                //Debug.Log("プレイヤーが所持しているユニットである");

                                //このユニットを選択中ユニットに追加
                                var UnitStatus_cs = no.GetComponent<UnitStatus_offline>();
                                UnitStatus_cs.SetISelected(true);
                                SelectedObjects.Add(UnitStatus_cs);
                            }
                        }
                    }

                    //ユニットの選択解除
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
                    foreach (var no in SelectedObjects)
                    {
                        no.SetISelected(false);
                    }
                }
            }
        }

        /// <summary>
        /// 範囲選択の大きさ
        /// </summary>
        /// <returns></returns>
        private float SelectAreaValue()
        {
            float xRange = selectStartPos.x - selectEndPos.x;
            float yRange = selectStartPos.y - selectEndPos.y;
            float value = Mathf.Abs(xRange * yRange);
            return value;
        }

        /// <summary>
        /// オブジェクトがスクリーン範囲指定内かを判定
        /// </summary>
        /// <param name="targetObj"></param>
        /// <returns></returns>
        private bool InSelectArea(GameObject targetObj)
        {
            //オブジェクトのワールド座標をスクリーン座標に変換
            var screenPos = mainCamera.WorldToScreenPoint(targetObj.transform.position);

            //スクリーン範囲内判定
            if (((screenPos.x >= selectStartPos.x
                && screenPos.x <= selectEndPos.x)
                || (screenPos.x <= selectStartPos.x
                && screenPos.x >= selectEndPos.x))
                && ((screenPos.y >= selectStartPos.y
                && screenPos.y <= selectEndPos.y)
                || (screenPos.y <= selectStartPos.y
                && screenPos.y >= selectEndPos.y)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
