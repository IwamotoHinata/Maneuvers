using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;


public class MultSelect : MonoBehaviour
{
    private Vector3 SelectStartPos;
    private Vector3 SelectEndPos;
    [SerializeField]
    private GameObject MarkPoint;
    private Camera mainCamera;

   private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(Select());
    }

    IEnumerator Select()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.GetKey(KeyCode.Mouse0)); //範囲選択の開始地点の決定
            SelectStartPos = Input.mousePosition;
            var MarkStartPoint = Instantiate(MarkPoint, SelectStartPos, Quaternion.identity);

            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Mouse0)); //範囲選択の終了地点の決定
            SelectEndPos = Input.mousePosition;
            var MarkEndPoint = Instantiate(MarkPoint, SelectEndPos, Quaternion.identity);

            if (SelectAreaValue() <= 10)
            {
                Destroy(MarkStartPoint);
                Destroy(MarkEndPoint);
            }
            else
            {
                var OwnerObjects = FindObjectsOfType<CharacterBaseData>();
                List<CharacterSelect> SelectedObject = new List<CharacterSelect>();
                foreach (var characters in OwnerObjects)
                {
                    if (InSelectArea(characters.gameObject))
                    {
                        if (characters.GetCharacterOwnerType() == OwnerType.Player && characters.MyUnitType != UnitType.Funnnel)
                        {
                            var CharacterSelectCs = characters.GetComponent<CharacterSelect>();
                            CharacterSelectCs.MultSelected(true);
                            
                            SelectedObject.Add(CharacterSelectCs);
                        }
                    }
                }

                yield return new WaitUntil(() => Input.GetKey(KeyCode.Mouse0));
                Destroy(MarkStartPoint);
                Destroy(MarkEndPoint);
                foreach (var charactersCs in SelectedObject)
                {
                    charactersCs.MultSelected(false);
                }
            }
        }
    }
    
    private bool InSelectArea(GameObject targetObject)
    {
       
        var screenPos = mainCamera.WorldToScreenPoint(targetObject.transform.position);
        if ((screenPos.x >= SelectStartPos.x
             && screenPos.x <= SelectEndPos.x)
            || (screenPos.x <= SelectStartPos.x
                && screenPos.x >= SelectEndPos.x))
        {
            if ((screenPos.y >= SelectStartPos.y
                 && screenPos.y <= SelectEndPos.y)
                || (screenPos.y <= SelectStartPos.y
                    && screenPos.y >= SelectEndPos.y))
            {
                return true;
            }
        }
        
        return false;
    }

    private float SelectAreaValue()
    {
       
        float xRange = SelectStartPos.x - SelectEndPos.x;
        float yRange = SelectStartPos.y - SelectEndPos.y;
        float Value = Mathf.Abs(xRange * yRange);
        return Value;
    }
}