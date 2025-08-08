using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupCanvas : MonoBehaviour
{
    public static PopupCanvas Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject winGameUI;
    [SerializeField] private GameObject loseGameUI;
    [SerializeField] private GameObject settingUI;
    [SerializeField] private GameObject thankYouUI;
    [SerializeField] private GameObject pauseSettingUI;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại khi load scene mới
        }
        else
        {
            Destroy(gameObject); // Hủy bản trùng
        }
    }


    private void Start()
    {
        if (winGameUI != null) winGameUI.SetActive(false);
        if (loseGameUI != null) loseGameUI.SetActive(false);
        if (settingUI != null) settingUI.SetActive(false);
        if (thankYouUI != null) thankYouUI.SetActive(false);
        if (pauseSettingUI != null) pauseSettingUI.SetActive(false);
        // Gán sự kiện cho button thứ 2 trong WinGame UI (nếu có)
        AssignSecondButtonEvent();
        AssignLoseGameButtonEvents();
        AssignPauseSettingEvents();

    }

    public void ShowWinGameUI()
    {
        if (winGameUI != null)
            winGameUI.SetActive(true);
    }

    public void ShowLoseGameUI()
    {
        if (loseGameUI != null)
            loseGameUI.SetActive(true);
    }

    public void ActiveSettingUI()
    {
        if (settingUI != null)
            settingUI.SetActive(true);
    }
    private void AssignPauseSettingEvents()
    {
        if (pauseSettingUI == null) return;

        Button[] buttons = pauseSettingUI.GetComponentsInChildren<Button>(true);

        if (buttons.Length >= 4)
        {
            // Button 1: Option
            Button btnOption = buttons[0];
            btnOption.onClick.RemoveAllListeners();
            btnOption.onClick.AddListener(() =>
            {
                if (settingUI != null)
                    settingUI.SetActive(true);
            });

            // Button 2: Again (Restart level)
            Button btnAgain = buttons[1];
            btnAgain.onClick.RemoveAllListeners();
            btnAgain.onClick.AddListener(() =>
            {
                Time.timeScale = 1f; // Resume time before reloading
                int currentIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(currentIndex);
            });

            // Button 3: Continue
            Button btnContinue = buttons[2];
            btnContinue.onClick.RemoveAllListeners();
            btnContinue.onClick.AddListener(() =>
            {
                Time.timeScale = 1f; // Resume game
                pauseSettingUI.SetActive(false); // Hide pause UI
            });

            // Button 4: Menu
            Button btnToMenu = buttons[3];
            btnToMenu.onClick.RemoveAllListeners();
            btnToMenu.onClick.AddListener(() =>
            {
                Time.timeScale = 1f; // Resume time before loading menu
                SceneManager.LoadScene("MainMenu");
            });
        }
        else
        {
            Debug.LogWarning("PauseSetting UI không đủ 4 button để gán sự kiện.");
        }
    }

    private void AssignLoseGameButtonEvents()
    {
        if (loseGameUI == null) return;

        Button[] buttons = loseGameUI.GetComponentsInChildren<Button>(true);

        if (buttons.Length >= 2)
        {
            // Button 1: Trở về Menu
            Button firstButton = buttons[0];
            firstButton.onClick.RemoveAllListeners();
            firstButton.onClick.AddListener(() =>
            {
                if (loseGameUI != null) loseGameUI.SetActive(false);
                if (winGameUI != null) winGameUI.SetActive(false);
                if (settingUI != null) settingUI.SetActive(false);
                SceneManager.LoadScene("MainMenu"); // thay bằng tên scene menu
            });

            // Button 2: Chơi lại level hiện tại
            Button secondButton = buttons[1];
            secondButton.onClick.RemoveAllListeners();
            secondButton.onClick.AddListener(() =>
            {
                int currentIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(currentIndex); // Load lại scene hiện tại
            });
        }
        else
        {
            Debug.LogWarning("LoseGame UI không đủ 2 button để gán sự kiện.");
        }
    }

    private void AssignSecondButtonEvent()
    {
        if (winGameUI == null) return;

        // Lấy tất cả button con
        Button[] buttons = winGameUI.GetComponentsInChildren<Button>(true);

        if (buttons.Length >= 2)
        {
            Button firstButton = buttons[0];
            firstButton.onClick.RemoveAllListeners();
            firstButton.onClick.AddListener(() =>
            {
                // Ẩn toàn bộ UI trước khi load menu
                if (winGameUI != null) winGameUI.SetActive(false);
                if (loseGameUI != null) loseGameUI.SetActive(false);
                if (settingUI != null) settingUI.SetActive(false);

                // Load menu
                SceneManager.LoadScene("MainMenu"); // đổi "MainMenu" thành tên scene menu của bạn
            });
            Button secondButton = buttons[1]; // Lấy button thứ 2

            // Xóa sự kiện cũ
            secondButton.onClick.RemoveAllListeners();

            // Gán sự kiện mới
            secondButton.onClick.AddListener(() =>
            {
                int currentIndex = SceneManager.GetActiveScene().buildIndex;
                int nextIndex = currentIndex + 1;

                if (nextIndex < SceneManager.sceneCountInBuildSettings)
                {
                    SceneManager.LoadScene(nextIndex);
                    winGameUI.SetActive(false); // Ẩn UI sau khi chuyển scene
                    loseGameUI.SetActive(false); // Ẩn UI sau khi chuyển scene
                }
                else
                {
                    Debug.Log("Đã chơi hết tất cả level.");

                    // Ẩn UI thắng / thua nếu còn
                    if (winGameUI != null) winGameUI.SetActive(false);
                    if (loseGameUI != null) loseGameUI.SetActive(false);

                    // Hiện màn hình cảm ơn
                    if (thankYouUI != null) thankYouUI.SetActive(true);
                }

            });
        }
        else
        {
            Debug.LogWarning("WinGame UI không đủ 2 button để gán sự kiện.");
        }
    }
    public void ShowPauseUI()
    {
        Time.timeScale = 0f;
        pauseSettingUI?.SetActive(true);
    }

}
