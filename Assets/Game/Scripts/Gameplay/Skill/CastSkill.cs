using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.ParticleSystem;
using UnityEngine.VFX;
using UnityEngine.UI;

public class CastSkill : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] protected GameObject skill;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float timeCountDown = 10f;
    [SerializeField] private UnityEngine.UI.Image skillCountDown;
    [SerializeField] private GameObject skillFrameImage;
    protected float timer = 0f;
    protected bool isCasting = false;
    protected virtual void Start()
    {
       //    skillCountDown = transform.GetChild(0).GetComponent<Image>();
        skillFrameImage.SetActive(false);
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        skillCountDown.fillAmount = timer / timeCountDown;
    }
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (timer >= 0)
        {
            Debug.Log("cant do ");
            return;
        }
        isCasting = true;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (!isCasting)
            return;
        if (TryGetHitPosition(eventData, out RaycastHit hit))
        {
            skill.transform.position = hit.point+Vector3.up*0.3f;
        } else
        {
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            Vector3 rayStart = ray.origin;
            Vector3 rayDirection = ray.direction;

            // Kiểm tra nếu tia không song song với mặt phẳng y = 0
            if (Mathf.Abs(rayDirection.y) > Mathf.Epsilon)
            {
                float t = -rayStart.y / rayDirection.y;
                Vector3 hitPoint = rayStart + rayDirection * t;
                skill.transform.position = hitPoint + Vector3.up * 0.3f;
            }
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        timer = timeCountDown;
        skillCountDown.fillAmount = 1;
        isCasting = false;

    }
    protected virtual bool TryGetHitPosition(PointerEventData eventData, out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        return Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        skillFrameImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        skillFrameImage.SetActive(false);
    }
}
