using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTurret : MonoBehaviour
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
    [SerializeField] private GameObject uiUpgradePanel;
    private TurretData turretData;
    private void Start()
    {
        uiUpgradePanel.SetActive(false);

    }
    public void ShowUpgradePanel(TurretController turret, GameObject turretObject, Vector3 posWorldSpace)
    {
        turretData = new TurretData(turret, turretObject, posWorldSpace);

        // Hiển thị bảng nâng cấp
        uiUpgradePanel.SetActive(true);

        // Cập nhật vị trí bảng nâng cấp trên Canvas
        Vector2 canvasPosition = UtilityFuntion.ConvertWordSpaceToUI(posWorldSpace, GetComponentInParent<Canvas>());
        uiUpgradePanel.GetComponent<RectTransform>().anchoredPosition = canvasPosition;
    }
    public void HideUpgradePanel()
    {
        uiUpgradePanel.SetActive(false);
        turretData = null; // Reset dữ liệu khi ẩn panel
    }
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
    public void UpgradeTurretUI()
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
}
