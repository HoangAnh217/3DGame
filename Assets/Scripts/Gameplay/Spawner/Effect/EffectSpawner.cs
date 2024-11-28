using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : Spawner
{
    public static  EffectSpawner Instance { get; private set; }
    [HideInInspector] public static string EffectLine = "LineRenderer";
    [SerializeField] private LineRenderer _lineRenderer;
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }
    public void DrawRangeCircle(float rad, Color color)
    {
        int segments = 50;
        _lineRenderer.positionCount = segments + 1;
        _lineRenderer.useWorldSpace = false;

        float angle = 0f;
        for (int i = 0; i < segments + 1; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * rad;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * rad;
            _lineRenderer.SetPosition(i, new Vector3(x, 0, z));
            angle += 360f / segments;
        }
        _lineRenderer.loop = true; // Kết nối điểm cuối với điểm đầu
         ChangeCircleColor(color); // Đặt màu ban đầu là xanh
    }

    private void ChangeCircleColor(Color color)
    {
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
    }

}
