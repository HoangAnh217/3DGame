using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    private PopupCanvas popupCanvas;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        popupCanvas = PopupCanvas.Instance;
    }
    public void GameOver()
    {
        Debug.Log("Game Over");
        popupCanvas.ShowLoseGameUI();
    }
    public void WinGame()
    {
        Debug.Log("Win");
        popupCanvas.ShowWinGameUI();
    }
}