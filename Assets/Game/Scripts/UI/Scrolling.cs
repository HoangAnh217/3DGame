using UnityEngine;
using UnityEngine.EventSystems;

public class Scrolling : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Transform levelPageRect; // Tham chiếu đến RectTransform chứa UI
    private Vector2 startPosition;       // Vị trí bắt đầu của UI khi kéo
    private Vector2 dragStartPosition;   // Vị trí con chuột khi bắt đầu kéo
    private float dragThreshold = 50f;   // Ngưỡng tối thiểu để xác định là một thao tác kéo

    private void Start()
    {
        // Khởi tạo vị trí ban đầu
        startPosition = levelPageRect.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Lưu vị trí chuột khi bắt đầu kéo
        dragStartPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Tính khoảng cách chuột đã kéo
        float deltaX = eventData.position.x - dragStartPosition.x;

        // Di chuyển UI theo khoảng cách kéo chuột
        Vector2 newPosition = startPosition + new Vector2(deltaX, 0);
        levelPageRect.position = newPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Tính khoảng cách kéo
        float deltaX = eventData.position.x - dragStartPosition.x;

        // Nếu kéo vượt ngưỡng, chuyển trang
        if (Mathf.Abs(deltaX) > dragThreshold)
        {
            if (deltaX > 0)
                FindObjectOfType<PageLevelCtrl>().Previous();
            else
                FindObjectOfType<PageLevelCtrl>().Next();
        }

        // Reset lại vị trí ban đầu
        startPosition = levelPageRect.position;
    }
}
