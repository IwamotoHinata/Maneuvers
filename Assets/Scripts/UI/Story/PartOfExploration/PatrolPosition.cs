using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPosition : MonoBehaviour
{

    public GameObject nextposition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().nextposition = nextposition;
        }
    }
    */
}
