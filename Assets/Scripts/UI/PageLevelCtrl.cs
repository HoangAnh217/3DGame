using DanielLochner.Assets.SimpleScrollSnap;
using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class PageLevelCtrl : MonoBehaviour
{
    [SerializeField] private SimpleScrollSnap simpleScroll;   
    

    [SerializeField] private int maxPage;
    [SerializeField] private int unlockPage = 2;
    [SerializeField] private Material material;
    [SerializeField] private Transform[] panel;

    private TextMeshProUGUI titleText;
    private TextMeshProUGUI introduceText;
    [Header("AnimateScaleObj")]
    private Tween currentTween;
    [SerializeField] private float duration;
    [SerializeField] private GameObject titleObj;
    [SerializeField] private GameObject introduceObj;
    [SerializeField] private GameObject buttonSelect;
    [Header("Title and Introduce Text")]
    [SerializeField] private List<string> titlesString; // Danh sách các title cho từng trang.
    [SerializeField] private List<string> introduceTextString;
    private void Start()
    {   
        titleText = titleObj.GetComponentInChildren<TextMeshProUGUI>();
        introduceText = introduceObj.GetComponentInChildren<TextMeshProUGUI>();
        for (int i = unlockPage; i < panel.Length; i++)
        {
            panel[i].GetChild(0).gameObject.SetActive(true);
            panel[i].GetComponent<RawImage>().material = material;
        }
    }
    #region button
    public void Next()
    {
        // press button next will call next() and then call ActWhenEndDrag(int centeredPanel)
        if (simpleScroll.CenteredPanel ==maxPage-1)
        {
            return;
        }
        titleObj.transform.DOScale(Vector3.zero, duration / 4);
        introduceObj.transform.DOScale(Vector3.zero, duration / 4);
        currentTween = buttonSelect.transform.DOScale(Vector3.zero, duration / 4).OnComplete(() =>{
        });
    }

    public void Previous()
    {
        if (simpleScroll.CenteredPanel == 0)
        {   
            return;
        }
        titleObj.transform.DOScale(Vector3.zero, duration / 4);
        introduceObj.transform.DOScale(Vector3.zero, duration / 4);
        currentTween = buttonSelect.transform.DOScale(Vector3.zero, duration / 4);
    }
    public void Return()
    {

    }
    public void Select()
    {
        SceneController.instance.SelectLevel(1);
    }
    #endregion
    private void AnimateObject(int index)
    {
        if (currentTween.IsPlaying())
        {
            currentTween.OnComplete(() =>
            {
                Animate(index);
            });
        }
        else
        {
            Animate(index);
        }
    }

    private void Animate(int index)
    {
        titleText.text = titlesString[index];
        introduceText.text = introduceTextString[index];

        titleObj.transform.DOScale(Vector3.one, duration / 2);
        introduceObj.transform.DOScale(Vector3.one, duration / 2);
        buttonSelect.transform.DOScale(Vector3.one, duration / 2).OnComplete(() =>
        {
            currentTween = null;
        });
    }

    public void ActWhenBeginDrag()
    {

        titleObj.transform.DOScale(Vector3.zero, duration / 2);
        introduceObj.transform.DOScale(Vector3.zero, duration / 2);
        currentTween = buttonSelect.transform.DOScale(Vector3.zero, duration / 2);
    }
    public void ActWhenEndDrag(int centeredPanel)
    {
        Debug.Log(centeredPanel);
        AnimateObject(centeredPanel);
    }
}
