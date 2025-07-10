using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectSpawner : Spawner
{
    public static  EffectSpawner Instance { get; private set; }
    [Header("effect")]
    private Canvas canvas;
    [SerializeField] private GameObject parentCoin;

    public static string TextFloat = "DamageText";
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }
    protected override void Start()
    {
        base.Start();
        GameObject canvasObj = GameObject.Find("MainCanvas");
        canvas = canvasObj.GetComponent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found in the scene.");
        }
    }
    public void SpawnCoin(int coinValue,Vector3 pos)
    {   
        Vector2 canvasPosition = ConvertWorldToCanvasPosition(pos);
        GameObject coin = Spawn("CoinPrefab", pos,Quaternion.identity).gameObject;
        coin.transform.SetParent(parentCoin.transform, true); // Đặt coin vào parentCoin
        RectTransform coinRect = coin.GetComponent<RectTransform>();
        coinRect.anchoredPosition = canvasPosition;
        coinRect.DOAnchorPos(new Vector2(-100,0), 1f)
         .SetEase(Ease.InOutQuad)
         .OnComplete(() =>
         {
             PlayerData.instance.ReceiveMoney(coinValue);
             Despawm(coin.transform);
         });
    }
    private Vector2 ConvertWorldToCanvasPosition(Vector3 worldPosition)
    {
        // 1. Chuyển từ World Space sang Screen Space
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        // 2. Chuyển từ Screen Space sang Canvas Space
        RectTransform canvasRect = parentCoin.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            canvas.worldCamera,
            out Vector2 canvasPosition
        );

        return canvasPosition;
    }
    public override void Despawm(Transform obj)
    {
        obj.SetParent(holder);
        base.Despawm(obj);
    }
}
