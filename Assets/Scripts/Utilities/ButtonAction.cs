using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class ButtonAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Button yourButton;
    private TextMeshProUGUI textMeshProUGUI;
    private TextMeshPro textMeshPro;
    private Image image;
    private Color colorOrigin;
    [SerializeField] private int sceneIndex;
    [Header("infor")]
    [SerializeField] private bool hasText = true;
    [SerializeField] private bool hasRotate = true;
    [SerializeField] private Animator trasitionSenece;

    private void Start()
    {
        yourButton = GetComponent<Button>();
        if (hasText)
        {
            // Kiểm tra TextMeshProUGUI
            textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
            // Nếu không phải TextMeshProUGUI, kiểm tra TextMeshPro
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

        // Đồng bộ màu và nội dung cho Text
        if (hasText)
        {
            if (textMeshProUGUI != null)
            {
                textMeshProUGUI.color = image.color;
                textMeshProUGUI.text = gameObject.name;
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
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 scale = transform.localScale;
        yourButton.transform.DOScale(transform.localScale * 1.1f, 0.1f).OnComplete(() =>
        {
            yourButton.transform.DOScale(scale, 0.1f);
        });
    }

    public void LoadSence()
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
           // SceneManager.LoadScene(sceneIndex);
            SceneController scenceController  = SceneController.instance;
            if (scenceController == null)
            {
                Debug.Log("asdasd");
                SceneManager.LoadScene(sceneIndex);
            }
            else
            StartCoroutine(scenceController.LoadLevel(sceneIndex));
        }
        else
        {
            Debug.LogError("Scene index is invalid or not set!");
        }
    }
/*    private IEnumerator LoadLevel()
    {
        trasitionSenece.SetTrigger("Start");


        yield return new WaitForSeconds(0.6f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false; 

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null; 
        }

        trasitionSenece.SetTrigger("End");
        yield return new WaitForSeconds(0.3f);
    }*/
}
