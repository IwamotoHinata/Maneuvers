using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public float WarpLange_X;
    public float WarpLange_Y;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Tgt") || other.CompareTag("Player"))
        {
            other.gameObject.transform.Translate(new Vector3(WarpLange_X, WarpLange_Y, 0));
        }
    }
}
