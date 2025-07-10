using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void GameOver()
    {
        // Xử lý logic khi game over
        Debug.Log("Game Over");
        // Có thể thêm UI hoặc các hành động khác ở đây
    }
    public void WinGame()
    {
        Debug.Log("Win");
    }
}