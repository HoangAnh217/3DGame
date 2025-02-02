using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeSence : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    private Animator animatorCamera;
    private void Start()
    {
        animatorCamera = mainCamera.GetComponent<Animator>();
    }
    #region button action
    public void NextPage()
    {
        if (mainCamera == null)
            return;

        animatorCamera.SetFloat("animate", 1);
    }
    public void PrevPage() 
    {
        if (mainCamera == null)
            return;
        animatorCamera.SetFloat("animate", 0);
    }
    #endregion
}
