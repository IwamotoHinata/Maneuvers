using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveY : MonoBehaviour
{

    public float Max;
    public float Min;
    public float Xposition;
    public GameObject Camera;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.position.y > Max)
            {
                //最大位置を超えていたら
                Camera.transform.position = new Vector3(Xposition, Max, Camera.transform.position.z);
            }
            else
            {
                //最小位置を超えていたら
                if (other.transform.position.y < Min)
                {
                    Camera.transform.position = new Vector3(Xposition, Min, Camera.transform.position.z);
                }
                else
                {
                    //最大最小の間であれば
                    Camera.transform.position = new Vector3(Xposition,other.transform.position.y, Camera.transform.position.z);
                }
            }
        }
    }

}
