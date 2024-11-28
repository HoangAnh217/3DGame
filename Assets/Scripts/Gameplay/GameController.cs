using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // control flow game
    public int wave = 1;
    //player data test
    public static GameController instance;
    private UI_IngameManager ingameManager;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        ingameManager = UI_IngameManager.instance;
    }
    private int money = 5000;
    public void MinusAmountCost(int amountMinus)
    {
        money-=amountMinus;
        ingameManager.UpdateMoney();
    } 
    public int GetMoney()
    {
        return money;
    }
}
