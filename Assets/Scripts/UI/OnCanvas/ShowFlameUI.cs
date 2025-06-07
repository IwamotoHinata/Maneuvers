using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowFlameUI : MonoBehaviour
{
    private GameObject clickUnit = null;    // クリックしたUnitの識別
    private SkillExplanationPopOut skillPop;

    // コンポーネント
    [SerializeField]
    private GameObject charaPanel;
    [SerializeField]
    private GameObject[] flame = new GameObject[4];
    // Start is called before the first frame update
    void Start()
    {
        skillPop = charaPanel.GetComponent<SkillExplanationPopOut>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    clickUnit = hit.collider.gameObject;
                }
            }
        }

        if (clickUnit != null)
        {
            switch (clickUnit.name)
            {
                case "Basic(Clone)":
                    for (int i = 0; i < 5; i++)
                    {
                        if (skillPop.icon[i] == 0)
                        {
                            flame[i].SetActive(true);
                        }
                        else
                        {
                            flame[i].SetActive(false);
                        }
                    }
                    break;
                case "Assault(Clone)":
                    for (int i = 0; i < 5; i++)
                    {
                        if (skillPop.icon[i] == 1)
                        {
                            flame[i].SetActive(true);
                        }
                        else
                        {
                            flame[i].SetActive(false);
                        }
                    }
                    break;
                case "Recon(Clone)":
                    for (int i = 0; i < 5; i++)
                    {
                        if (skillPop.icon[i] == 2)
                        {
                            flame[i].SetActive(true);
                        }
                        else
                        {
                            flame[i].SetActive(false);
                        }
                    }
                    break;
                case "Debuffer(Clone)":
                    for (int i = 0; i < 5; i++)
                    {
                        if (skillPop.icon[i] == 3)
                        {
                            flame[i].SetActive(true);
                        }
                        else
                        {
                            flame[i].SetActive(false);
                        }
                    }
                    break;
                case "Sniper(Clone)":
                    for (int i = 0; i < 5; i++)
                    {
                        if (skillPop.icon[i] == 4)
                        {
                            flame[i].SetActive(true);
                        }
                        else
                        {
                            flame[i].SetActive(false);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
