using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class MainCanvas : MonoBehaviour
{   
   
    public static MainCanvas Instance { get; private set; }


    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI waveNotificationText;

    private UpgradeTurret upgradeTurret;

    private PlayerData playerData;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        playerData = PlayerData.instance;
        upgradeTurret = GetComponentInChildren<UpgradeTurret>();
        UpdateMoneyUI(playerData.GetMoney());
    }

    public void UpdateMoneyUI(int currentMoney)
    {
        moneyText.text = currentMoney.ToString();
    }
    public void ShowUpgradePanel(TurretController turret, GameObject turretObject, Vector3 posWorldSpace)
    {
        upgradeTurret.ShowUpgradePanel(turret, turretObject, posWorldSpace);
    }
    public void ShowWaveNotification(int currentWave, int maxWave,float time)
    {
        waveNotificationText.gameObject.SetActive(true);
        waveNotificationText.text = $"Wave {currentWave} / {maxWave}";
        waveNotificationText.alpha = 0f; // Đảm bảo bắt đầu từ 0 alpha

        waveNotificationText.DOFade(1f, time/4) // Fade-in
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                // Đợi 1.5s rồi fade-out
                waveNotificationText.DOFade(0f, time/4)
                    .SetEase(Ease.InOutQuad)
                    .SetDelay(time/2)
                    .OnComplete(() =>
                    {
                        waveNotificationText.gameObject.SetActive(false);
                    });
            });
    }

}
