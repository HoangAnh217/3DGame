using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class UI_IngameManager : MonoBehaviour
{   
   
    public static UI_IngameManager Instance { get; private set; }


    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Transform CallWave;
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
    public void ShowUI_CallWave(Vector3 pos)
    {
        Vector2 posUi = UtilityFuntion.ConvertWordSpaceToUI(pos,GetComponent<Canvas>());

    }
    public void ShowUpgradePanel(TurretController turret, GameObject turretObject, Vector3 posWorldSpace)
    {
        upgradeTurret.ShowUpgradePanel(turret, turretObject, posWorldSpace);
    }
}
