using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp4Goal : MonoBehaviour
{
    private GameManager manager;
    private void Start()
    {
        manager = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            player.GetComponent<Player>().StopWalk();
            manager.ScenarioToExplore(manager.CurrentState.Value);
        }
    }
}
