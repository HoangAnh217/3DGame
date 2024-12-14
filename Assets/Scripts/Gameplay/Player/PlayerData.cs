using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerData : MonoBehaviour,IDameable
{   

    private int money = 200;
    private int health; // Ví dụ về máu người chơi
    private int maxHealth = 5000; // Ví dụ về máu người chơi

    // Dùng để truy cập giá trị tiền và máu
    public static PlayerData instance;

    private UI_IngameManager ingameManager;

    [SerializeField] private Slider healthPlayer;
    private TextMeshProUGUI hpText;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Đảm bảo chỉ có 1 instance
        }
    }
    private void Start()
    {
        health = maxHealth;
        hpText = healthPlayer.transform.Find("HpText").GetComponent<TextMeshProUGUI>();
        ingameManager = UI_IngameManager.Instance;

    }
    // Cập nhật tiền
    public void ReceiveMoney(int _money)
    {
        this.money += _money;
        UnityEngine.Debug.Log("masda");
        ingameManager.UpdateMoneyUI(money);
    }

    public int GetMoney()
    {
        return money;
    }

    // Cập nhật máu
    public void ReceiveDamage(int damage)
    {
        this.health -= damage;
        UpdateHealthUI(health,maxHealth); // Cập nhật UI máu
    }
    public void UpdateHealthUI(int _health, int maxHealth)
    {
        healthPlayer.value = 100*_health / maxHealth;
        //healthPlayer.transform.Find("hpText").GetComponent;
        hpText.text = health.ToString();
        
    }
    public int GetHealth()
    {
        return health;
    }
    // Các hàm khác để quản lý tiền và máu người chơi nếu cần
}
