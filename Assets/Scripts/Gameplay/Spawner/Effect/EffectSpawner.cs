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
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject parentCoin;

    private TextMeshPro damageTextPrefab;
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }
    public Tween SpawnAndMoveCoin(Vector3 vec)
    {
        GameObject coin = Instantiate(coinPrefab, parentCoin.transform);
        RectTransform coinRect = coin.GetComponent<RectTransform>();
        coinRect.anchoredPosition = UtilityFuntion.ConvertWordSpaceToUI(transform.position,canvas);


        // Bay từ vị trí bắt đầu đến vị trí UI mục tiêu
        return coinRect.DOAnchorPos(coinPrefab.GetComponent<RectTransform>().anchoredPosition, 1f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            Despawm(coin.transform);
        });
    }
    public void DisplayDamageText(int damage,Canvas canvasParent)
    {
        Transform damageTextInstance = Spawn(prefabs[0], Vector3.zero, Quaternion.identity);
        damageTextInstance.SetParent(canvasParent.transform);
        damageTextInstance.SetParent(canvasParent.transform, false);
        RectTransform rectTransform = damageTextInstance.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(30, 130, 0);
        rectTransform.localRotation = Quaternion.Euler(30,0,0);
        damageTextInstance.gameObject.SetActive(true);
        TextMeshPro damageText = damageTextInstance.GetComponent<TextMeshPro>();
        damageTextInstance.GetComponent<TextMeshPro>().text = damage.ToString(); // Hiển thị số lượng sát thương
        rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y+0.7f,0.5f)
            .SetEase(Ease.OutQuad); // Hiệu ứng di chuyển mượt mà

        // Làm mờ Text sau khi di chuyển xong
        damageText.DOFade(0, 0.5f).OnKill(() => Despawm(damageTextInstance.transform));
    }
    public override void Despawm(Transform obj)
    {
        obj.SetParent(holder);
        base.Despawm(obj);
    }
}
