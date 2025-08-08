using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [Header("Audio Sliders")]
    public Slider bgmSlider; // Slider BGM
    public Slider sfxSlider; // Slider SFX

    [Header("Language")]
    public TMP_Dropdown languageDropdown;

    [Header("Buttons")]
    public Button applyButton;
    public Button closeButton;
    [Header("Volume Text")]
    public TextMeshProUGUI bgmValueText; // Text hiển thị BGM volume
    public TextMeshProUGUI sfxValueText; // Text hiển thị SFX volume
    private void Start()
    {
        // Load dữ liệu lưu trước đó
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
      //  languageDropdown.value = PlayerPrefs.GetInt("LanguageIndex", 0);

        // Gán giá trị ngay cho AudioManager
        AudioManager.Instance.SetVolumeBGM(bgmSlider.value);
        AudioManager.Instance.SetVolumeSFX(sfxSlider.value);

        // Gắn sự kiện khi kéo slider
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        // Gắn sự kiện khi đổi ngôn ngữ
      //  languageDropdown.onValueChanged.AddListener(OnLanguageChanged);

        // Gắn sự kiện cho nút
        applyButton.onClick.AddListener(OnApplyClicked);
        closeButton.onClick.AddListener(OnCloseClicked);
    }

    private void OnBGMVolumeChanged(float value)
    {
        AudioManager.Instance.SetVolumeBGM(value);
        bgmValueText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    private void OnSFXVolumeChanged(float value)
    {
        AudioManager.Instance.SetVolumeSFX(value);
        sfxValueText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    private void OnLanguageChanged(int index)
    {
        Debug.Log("Preview Language: " + languageDropdown.options[index].text);
    }

    private void OnApplyClicked()
    {
        // Lưu âm lượng
        PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);

        // Lưu ngôn ngữ
    //    PlayerPrefs.SetInt("LanguageIndex", languageDropdown.value);

        PlayerPrefs.Save();

        Debug.Log("Settings applied and saved.");
        gameObject.SetActive(false);
    }

    private void OnCloseClicked()
    {
        AudioManager.Instance.SetVolumeBGM(PlayerPrefs.GetFloat("BGMVolume", 1f));
        AudioManager.Instance.SetVolumeSFX(PlayerPrefs.GetFloat("SFXVolume", 1f));
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        gameObject.SetActive(false);
    }
}
