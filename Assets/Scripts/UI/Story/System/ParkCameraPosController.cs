using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParkCameraPosController : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    private Camera currentCamera;
    private List<Camera> cameras = new List<Camera>();

    private void Start()
    {
        cameras = GetComponentsInChildren<Camera>(true).ToList();
        currentCamera = cameras[0];
    }

    private void Update()
    {
        // TODO Very Very 汚い　クソ　カメラ　切り替え　コード
        if (CheckArea(-40.0f,40.0f,-22.5f,22.5f))
        {
            SwitchCamera(cameras[0]);
        }
        else if (CheckArea(40.0f, 120.0f, -22.5f,22.5f))
        {
            SwitchCamera(cameras[1]);
        }
        else if (CheckArea(120.0f, 200.0f, -22.5f,22.5f))
        {
            SwitchCamera(cameras[2]);
        }
        else if (CheckArea(200.0f, 280.0f, -22.5f,22.5f))
        {
            SwitchCamera(cameras[3]);
        }
        else if (CheckArea(-40.0f, 40.0f, 22.5f, 67.5f))
        {
            SwitchCamera(cameras[4]);
        }
        else if (CheckArea(40.0f, 120.0f, 22.5f, 67.5f))
        {
            SwitchCamera(cameras[5]);
        }
        else if (CheckArea(120.0f, 200.0f, 22.5f, 67.5f))
        {
            SwitchCamera(cameras[6]);
        }
        else if (CheckArea(200.0f, 280.0f, 22.5f, 67.5f))
        {
            SwitchCamera(cameras[7]);
        }
        else if (CheckArea(-40.0f,40.0f, 67.5f, 112.5f))
        {
            SwitchCamera(cameras[8]);
        }
        else if (CheckArea(40.0f, 120.0f, 67.5f, 112.5f))
        {
            SwitchCamera(cameras[9]);
        }
        else if (CheckArea(120.0f, 200.0f, 67.5f, 112.5f))
        {
            SwitchCamera(cameras[10]);
        }
        else if (CheckArea(200.0f, 280.0f, 67.5f, 112.5f))
        {
            SwitchCamera(cameras[11]);
        }
        else if (CheckArea(-40.0f,40.0f, 112.5f, 157.5f))
        {
            SwitchCamera(cameras[12]);
        }
        else if (CheckArea(40.0f, 120.0f, 112.5f, 157.5f))
        {
            SwitchCamera(cameras[13]);
        }
        else if (CheckArea(120.0f, 200.0f, 112.5f, 157.5f))
        {
            SwitchCamera(cameras[14]);
        }
        else if (CheckArea(200.0f, 280.0f, 112.5f, 157.5f))
        {
            SwitchCamera(cameras[15]);
        }
    }

    private void SwitchCamera(Camera c)
    {
        if (currentCamera != c)
        {
            c.enabled = true;
            currentCamera.enabled = false;
            currentCamera = c;
        }
    }

    private bool CheckArea(float minX, float maxX, float minY, float maxY)
    {
        var p = targetTransform.position;
        return minX <= p.x && p.x < maxX && minY <= p.y && p.y< maxY;
    }
}
