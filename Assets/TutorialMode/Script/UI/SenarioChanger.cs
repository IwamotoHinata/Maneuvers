using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenarioChanger : MonoBehaviour
{
    [SerializeField] private TextManager_Offline StoryText;

    private void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Unit")
        {
            StoryText.changeSenario();
            Destroy(this);
        }
    }
}