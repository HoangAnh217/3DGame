using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgrScrolling : MonoBehaviour
{
    private RawImage image;
    [SerializeField] private float x, y;
    [SerializeField] private float speed;
    private void Start()
    {
        image = GetComponent<RawImage>();
    }
    private void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + new Vector2(x, y) * Time.deltaTime*speed/5 , image.uvRect.size);
    }
}

