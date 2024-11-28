using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem instance;

    public GridManager[] gridManager;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        gridManager = transform.GetChild(0).GetComponentsInChildren<GridManager>();
    }
    public void ShowGrid()
    {
        foreach (GridManager grid in gridManager)
        {
            grid.HideAndShowGrid(true);
        }
    }
}
