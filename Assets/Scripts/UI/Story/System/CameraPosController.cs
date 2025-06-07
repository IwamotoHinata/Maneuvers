using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosController : MonoBehaviour
{
    [SerializeField] private Transform secondCameraTransform;
    [SerializeField] private Transform targetTransform;
    private Vector3 cameraPos;
    
    private void Start()
    {
        cameraPos = transform.position;
    }

    private void Update()
    {
        if (targetTransform.position.x > -5.0f)
        {
            transform.position = secondCameraTransform.position;
        }
        else
        {
            transform.position = cameraPos;
        }
    }
}
