using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_IngameManager : MonoBehaviour
{   
    public static UI_IngameManager instance { get; private set; }

    private GameController gameController;

    [SerializeField] private GridManager[] grids;
    private bool gridEnable = false;
    //thong in ui
    [SerializeField] private GameObject uiUpgrade;
    [SerializeField] private TextMeshProUGUI currentMoney;
    // thong tin cua turret
    private GameObject turret;
    private Vector2Int point;
    private GridManager grid;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        gameController = GameController.instance;
        UpdateMoney();
    }
    public void ShowGrid()
    {
        foreach (GridManager grid in grids)
        {
            grid.HideAndShowGrid(!gridEnable);
        }
        gridEnable = !gridEnable;
    }
    public void ShowUI_Upgrade(GameObject _turret, Vector2Int a, GridManager _grid)
    {   
        uiUpgrade.SetActive(true);
        turret = _turret;
        point = a;
        grid = _grid;
    }
    public void UpdateMoney()
    {
        currentMoney.text = gameController.GetMoney().ToString();
    }
    #region button event
    public void RemoveTurret()
    {
        grid.SetArr(point, false);
        uiUpgrade.gameObject.SetActive(false);
        Destroy(turret);
    }
    public void Upgrade()
    {
        turret.GetComponent<TurretController>().UpgradeTurret();
        uiUpgrade.gameObject.SetActive(false);
    }
    #endregion
}
