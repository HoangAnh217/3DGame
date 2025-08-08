using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class ButtonAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Button yourButton;
    private TextMeshProUGUI textMeshProUGUI;
    private TextMeshPro textMeshPro;
    private Image image;
    private Color colorOrigin;

    [SerializeField] private int sceneIndex;

    [Header("Info")]
    [SerializeField] private bool hasText = true;
    [SerializeField] private bool hasRotate = true;

    [Header("Sound Settings")]
    [SerializeField] private bool playHoverSound = true;
    [SerializeField] private bool playClickSound = true;
    [SerializeField] private string hoverSFXName = "ButtonHover"; // tên clip SFX trong AudioManager
    [SerializeField] private string clickSFXName = "ButtonClick"; // tên clip SFX trong AudioManager

    private void Start()
    {
        yourButton = GetComponent<Button>();
        if (hasText)
        {
            textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
            if (textMeshProUGUI == null)
                textMeshPro = GetComponentInChildren<TextMeshPro>();
        }

        image = GetComponent<Image>();
        colorOrigin = image.color;
    }

    private void OnValidate()
    {
        yourButton = GetComponent<Button>();
        if (hasText)
        {
            textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
            if (textMeshProUGUI == null)
                textMeshPro = GetComponentInChildren<TextMeshPro>();
        }

        image = GetComponent<Image>();
        colorOrigin = image.color;

        if (hasText)
        {
            if (textMeshProUGUI != null)
            {
                textMeshProUGUI.color = image.color;
             //   textMeshProUGUI.text = gameObject.name;
            }
            else if (textMeshPro != null)
            {
                textMeshPro.color = image.color;
                textMeshPro.text = gameObject.name;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        yourButton.transform.DOScale(Vector3.one * 1.1f, 0.2f);
        if (hasRotate)
            transform.DOLocalRotate(new Vector3(0, 0, 2), 0.2f);

        image.color = Color.green;
        if (hasText)
        {
            if (textMeshProUGUI != null)
                textMeshProUGUI.color = Color.green;
            else if (textMeshPro != null)
                textMeshPro.color = Color.green;
        }

        // Phát âm thanh hover nếu bật
        if (playHoverSound && AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("HoverButton",0.4f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        yourButton.transform.DOScale(Vector3.one, 0.2f);
        if (hasRotate)
            transform.DOLocalRotate(Vector3.zero, 0.2f);

        image.color = colorOrigin;
        if (hasText)
        {
            if (textMeshProUGUI != null)
                textMeshProUGUI.color = colorOrigin;
            else if (textMeshPro != null)
                textMeshPro.color = colorOrigin;
        }
        /*if (playHoverSound && AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("HoverButton");*/
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 scale = transform.localScale;
        yourButton.transform.DOScale(scale * 1.1f, 0.1f).OnComplete(() =>
        {
            yourButton.transform.DOScale(scale, 0.1f);
        });

        // Phát âm thanh click nếu bật
        if (playClickSound && AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Click");
    }

    public void LoadSence()
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneController scenceController = SceneController.instance;
            if (scenceController == null)
                SceneManager.LoadScene(sceneIndex);
            else
                StartCoroutine(scenceController.LoadSceneWithFade(sceneIndex));
        }
        else
        {
            Debug.LogError("Scene index is invalid or not set!");
        }
    }
}
