using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera mainCamera; // Camera bắn tia
    public float rayLength = 100f; // Độ dài của tia
    public LayerMask targetLayer;
    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        // Chuyển đổi tọa độ màn hình thành Ray
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        // Kiểm tra xem tia có va chạm với đối tượng nào không
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            // Log thông tin đối tượng trúng tia
            Debug.Log($"Hit: {hitInfo.collider.name}");

            // Gọi phương thức trên đối tượng bị bắn
            //hitInfo.collider.GetComponent<Renderer>().material.color = Color.red; // Ví dụ: đổi màu đối tượng
        }
    }
}
