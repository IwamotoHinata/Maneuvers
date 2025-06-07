using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choice : MonoBehaviour
{
    [SerializeField] private List<GameObject> parts;

    public void Show()
    {
        foreach (var p in parts)
        {
            p.SetActive(true);
        }
    }
    
    public void Hide()
    {
        foreach (var p in parts)
        {
            p.SetActive(false);
        }
    }
}
