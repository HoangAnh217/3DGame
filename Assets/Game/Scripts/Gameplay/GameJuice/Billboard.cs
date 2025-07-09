using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        // T?m camera ch�nh trong scene
        cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        // L�m cho �?i t�?ng n�y lu�n quay m?t v? ph�a camera
        transform.LookAt(transform.position + cameraTransform.forward);
    }
}
