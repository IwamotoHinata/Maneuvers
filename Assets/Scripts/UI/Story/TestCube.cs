using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCube : MonoBehaviour
{
    private GameManager manager;
    
    private void Start()
    {
        manager = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        Debug.Log(manager.CurrentState.Value);
        manager.ScenarioToExplore(manager.CurrentState.Value);
    }
}
