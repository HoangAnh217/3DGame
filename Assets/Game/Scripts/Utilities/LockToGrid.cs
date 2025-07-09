using UnityEditor;
using UnityEngine;

// Lớp LockToGrid giúp căn chỉnh vị trí của đối tượng vào một lưới 3D
[ExecuteInEditMode] // Cho phép script hoạt động trong chế độ chỉnh sửa (Edit Mode)
public class LockToGrid : MonoBehaviour
{
    // Kích thước của một ô lưới (grid cell size)
    public int tileSize = 1;

    // Độ lệch (offset) tùy chỉnh để đặt vị trí khởi điểm của lưới
    public Vector3 tileOffset = Vector3.zero;

    // Hàm chạy mỗi frame (trong chế độ Play hoặc Edit Mode)
    void Update()
    {
        // Chỉ chạy trong chế độ chỉnh sửa (Edit Mode), không chạy khi đang Play
        if (!EditorApplication.isPlaying)
        {
            // Lấy vị trí hiện tại của đối tượng
            Vector3 currentPosition = transform.position;

            float snappedX = Mathf.Round(currentPosition.x / tileSize) * tileSize + tileOffset.x;
            float snappedZ = Mathf.Round(currentPosition.z / tileSize) * tileSize + tileOffset.z;
            float snappedY = tileOffset.y;

            // Tạo vị trí mới (snapped) với các giá trị đã làm tròn
            Vector3 snappedPosition = new Vector3(snappedX, snappedY, snappedZ);

            // Cập nhật vị trí của đối tượng theo vị trí đã căn chỉnh
            transform.position = snappedPosition;
        }
    }
}
