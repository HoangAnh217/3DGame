using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;
using System.Collections;

public class DropdownUI : MonoBehaviour
{
    [Header("UI References")]
    public Button dropdownButton;        // Nút mở/tắt danh sách
    public GameObject checklistPanel;    // Panel chứa các lựa chọn
    public TMP_Text selectedLanguageText; // Text hiển thị ngôn ngữ đã chọn

    [Header("Language Buttons")]
    public Button btnEnglish;
    public Button btnVietnamese;
    public Button btnJapanese;

    private void Start()
    {
        // Ẩn panel khi bắt đầu
        checklistPanel.SetActive(false);

        // Sự kiện click nút dropdown
        dropdownButton.onClick.AddListener(ToggleChecklist);

        // Gán sự kiện click từng nút ngôn ngữ
        btnEnglish.onClick.AddListener(() => SelectLanguage("English", "en"));
        btnVietnamese.onClick.AddListener(() => SelectLanguage("Tiếng Việt", "vi"));
        //btnJapanese.onClick.AddListener(() => SelectLanguage("日本語", "ja"));
    }

    private void ToggleChecklist()
    {
        bool isActive = checklistPanel.activeSelf;
        checklistPanel.SetActive(!isActive);

        Debug.Log("asdasd");

    }

    private void SelectLanguage(string displayName, string langCode)
    {
        if (selectedLanguageText != null)
            selectedLanguageText.text = displayName;

        checklistPanel.SetActive(false);

        // Gọi đổi ngôn ngữ Unity Localization
        StartCoroutine(SetLanguage(langCode));

        Debug.Log("Language changed to: " + displayName + " (" + langCode + ")");
    }

    private IEnumerator SetLanguage(string languageCode)
    {
        // Chờ Localization load xong
        yield return LocalizationSettings.InitializationOperation;

        // Tìm locale theo mã ngôn ngữ
        var locales = LocalizationSettings.AvailableLocales.Locales;
        foreach (var locale in locales)
        {
            if (locale.Identifier.Code == languageCode)
            {
                LocalizationSettings.SelectedLocale = locale;
                break;
            }
        }
    }
}
