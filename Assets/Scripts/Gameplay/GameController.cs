using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // control flow game
    private int money = 200;
    public int wave = 1;
    //player data test
    public static GameController instance;
    private UI_IngameManager ingameManager;
    private void Awake()
    {
        instance = this;
        money = 200;
    }
    private void Start()
    {
        ingameManager = UI_IngameManager.instance;
    }
    public void ReceiveMoney(int _money)
    {
        this.money+= _money;
        ingameManager.UpdateMoney();
    } 
    public int GetMoney()
    {
        return money;
    }
}
