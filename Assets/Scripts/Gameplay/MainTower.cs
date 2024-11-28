using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainTower : MonoBehaviour,IDameable
{   
    public static MainTower Instance { get; private set; }
    [SerializeField] private float health = 100f;
    [SerializeField] private Slider slider;
    private void Awake()
    {
        Instance = this;
    }
    public void ReceiveDamage(float dame)
    {
        // throw new System.NotImplementedException();
        health -= dame;
        slider.value = health;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
