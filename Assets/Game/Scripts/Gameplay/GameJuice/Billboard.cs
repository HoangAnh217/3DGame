using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        // T?m camera chính trong scene
        cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Làm cho ð?i tý?ng này luôn quay m?t v? phía camera
        transform.LookAt(transform.position + cameraTransform.forward);
    }
}
