using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupCanvas : MonoBehaviour
{

    public static PopupCanvas Instance { get; private set; }

    [SerializeField] private GameObject winGameUI;
    [SerializeField] private GameObject loseGameUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        winGameUI.SetActive(false);
        loseGameUI.SetActive(false);
    }
    public void ShowWinGameUI()
    {
        winGameUI.SetActive(true);
    }
    public void ShowLoseGameUI()
    {
        loseGameUI.SetActive(true);
    }

}
