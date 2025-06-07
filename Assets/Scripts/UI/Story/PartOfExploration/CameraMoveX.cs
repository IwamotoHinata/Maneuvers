using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveX : MonoBehaviour
{
    public float Max;
    public float Min;
    public float Yposition;
    public GameObject Camera;
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
        if (other.CompareTag("Player"))
        {
            if (other.transform.position.x > Max)
            {
                //最大位置を超えていたら
                Camera.transform.position = new Vector3(Max,Yposition, Camera.transform.position.z);
            }
            else
            {
                //最小位置を超えていたら
                if (other.transform.position.x < Min)
                {
                    Camera.transform.position = new Vector3(Min,Yposition, Camera.transform.position.z);
                }
                else
                {
                    //最大最小の間であれば
                    Camera.transform.position = new Vector3(other.transform.position.x,Yposition, Camera.transform.position.z);
                }
            }
        }
    }
}
