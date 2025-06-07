using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWatcher : MonoBehaviour
{


    private Renderer myrenderer;

    // Start is called before the first frame update
    void Start()
    {
        myrenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //プレイヤーが入ると
        if (other.CompareTag("Player"))
        {
            //非表示
            myrenderer.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //プレイヤーが出ると
        if (other.CompareTag("Player"))
        {
            //表示
            myrenderer.enabled = true;
        }
    }

}
