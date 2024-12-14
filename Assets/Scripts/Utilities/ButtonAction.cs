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
    private TextMeshProUGUI text;
    private Image image;
    private Color colorOrigin;
    [SerializeField] private int scene;
    [Header("infor")]
    [SerializeField] private bool hasText = true;
    [SerializeField] private bool hasRotate = true;

    private void Start()
    {   
        yourButton = GetComponent<Button>();
        if (hasText)
            text = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponent<Image>();
        colorOrigin = image.color;
    }
    private void OnValidate()
    {
        yourButton = GetComponent<Button>();
        if (hasText)
            text = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponent<Image>();
        colorOrigin = image.color;
        if (hasText)
        {
            text.color = image.color;
            text.text = gameObject.name;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        yourButton.transform.DOScale(Vector3.one * 1.1f, 0.2f);
        if (hasRotate)
            transform.DORotate(new Vector3(0, 0, 2), 0.2f);
        image.color = Color.green;
        if (hasText)
            text.color = Color.green;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        yourButton.transform.DOScale(Vector3.one, 0.2f);
        if (hasRotate)
            transform.DORotate(Vector3.zero, 0.2f);
        image.color = colorOrigin;
        if (hasText)
            text.color = colorOrigin;
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
        SceneManager.LoadScene(scene);
    }
}


