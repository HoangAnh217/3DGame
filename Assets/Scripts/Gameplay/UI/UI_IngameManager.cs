using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class UI_IngameManager : MonoBehaviour
{
    private class TurretData
    {
        public TurretController TurretController { get; private set; }
        public GameObject TurretObject { get; private set; }
        public Vector3 WorldPosition { get; private set; }

        public TurretData(TurretController turretController, GameObject turretObject, Vector3 worldPosition)
        {
            TurretController = turretController;
            TurretObject = turretObject;
            WorldPosition = worldPosition;
        }
    }
    public static UI_IngameManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject uiUpgradePanel;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Transform CallWave;

    private TurretData turretData;
    private PlayerData playerData;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        playerData = PlayerData.instance;
        uiUpgradePanel.SetActive(false);
        UpdateMoneyUI(playerData.GetMoney());
    }

    #region UI Controls
    public void ShowUpgradePanel(TurretController turret, GameObject turretObject, Vector3 posWorldSpace)
    {
        // Cập nhật dữ liệu Turret
        turretData = new TurretData(turret, turretObject, posWorldSpace);

        // Hiển thị bảng nâng cấp
        uiUpgradePanel.SetActive(true);

        // Cập nhật vị trí bảng nâng cấp trên Canvas
        Vector2 canvasPosition = UtilityFuntion.ConvertWordSpaceToUI(posWorldSpace, GetComponent<Canvas>());
        uiUpgradePanel.GetComponent<RectTransform>().anchoredPosition = canvasPosition;
    }

    public void UpdateMoneyUI(int currentMoney)
    {
        moneyText.text = currentMoney.ToString();
    }

    public void HideUpgradePanel()
    {
        uiUpgradePanel.SetActive(false);
        turretData = null; // Reset dữ liệu khi ẩn panel
    }
    public void ShowUI_CallWave(Vector3 pos)
    {
        Vector2 posUi = UtilityFuntion.ConvertWordSpaceToUI(pos,GetComponent<Canvas>());

    }
    #endregion

    #region Button Events
    public void RemoveTurret()
    {
        if (turretData == null) return;

        // Hoàn tiền cho người chơi
        int refundAmount = Mathf.FloorToInt(turretData.TurretController.turretDataSO.cost * 0.75f);
        PlayerData.instance.ReceiveMoney(refundAmount);

        // Thay đổi layer của đối tượng (nếu cần)
        turretData.TurretObject.layer = 9;

        // Phát âm thanh và xóa turret
        BuildingSystem.instance.PlayRemoveSound();
        Destroy(turretData.TurretController.gameObject);

        // Ẩn panel nâng cấp
        HideUpgradePanel();
    }

    public void UpgradeTurret()
    {
        if (turretData == null) return;

        // Kiểm tra điều kiện nâng cấp
        int upgradeCost = turretData.TurretController.turretDataSO.upgradeCost;
        if (PlayerData.instance.GetMoney() < upgradeCost)
        {
            Debug.Log("Dont enough money to update");
            HideUpgradePanel();
            return;
        }

        // Nâng cấp turret
        turretData.TurretController.UpgradeTurret();
        BuildingSystem.instance.PlayUpgradeSound();

        // Ẩn panel nâng cấp
        HideUpgradePanel();
    }
    #endregion
}
