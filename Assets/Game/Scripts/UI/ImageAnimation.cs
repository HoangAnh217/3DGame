using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ImageAnimation : MonoBehaviour
{
    public List<Sprite> frames;           // Các frame hình
    public float frameRate = 10f;         // Tốc độ (10 fps)

    private Image img;
    private int currentFrame;
    private float timer;

    void Start()
    {
        img = GetComponent<Image>();
        currentFrame = 0;
        timer = 0f;
    }

    void Update()
    {
        if (frames == null || frames.Count == 0) return;

        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % frames.Count;
            img.sprite = frames[currentFrame];
        }
    }
}
