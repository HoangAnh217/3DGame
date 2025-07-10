using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerData : MonoBehaviour,IDameable
{   
    public static PlayerData instance;
    private MainCanvas ingameManager;
    private GameController gameController;

    private int money = 200;
    private int health; // Ví dụ về máu người chơi
    private int maxHealth = 100; // Ví dụ về máu người chơi


    [SerializeField] private Slider healthPlayerSlider;
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
        maxHealth = 100;
        health = maxHealth;
        //hpText = healthPlayerSlider.transform.Find("HpText").GetComponent<TextMeshProUGUI>();
        ingameManager = MainCanvas.Instance;
        gameController = GameController.Instance;

    }
    // Cập nhật tiền
    public void ReceiveMoney(int _money)
    {
        this.money += _money;
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
        UpdateHealthUI(); // Cập nhật UI máu
        if (health <= 0)
        {
            gameController.GameOver();
        }
    }
    private void UpdateHealthUI()
    {
        healthPlayerSlider.value = health;
        //healthPlayer.transform.Find("hpText").GetComponent;
    }
    public int GetHealth()
    {
        return health;
    }
}
