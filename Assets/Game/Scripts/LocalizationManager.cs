using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizationManager : MonoBehaviour
{
    void Start()
    {
        // Chỉ đặt mặc định nếu chưa lưu ngôn ngữ
        if (!PlayerPrefs.HasKey("SelectedLanguage"))
        {
            SetLanguage("en"); // en = English
        }
        else
        {
            // Load lại ngôn ngữ đã lưu
            string code = PlayerPrefs.GetString("SelectedLanguage");
            SetLanguage(code);
        }
    }

    public void SetLanguage(string localeCode)
    {
        Locale locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);
        LocalizationSettings.SelectedLocale = locale;

        PlayerPrefs.SetString("SelectedLanguage", localeCode);
        PlayerPrefs.Save();
    }
}
