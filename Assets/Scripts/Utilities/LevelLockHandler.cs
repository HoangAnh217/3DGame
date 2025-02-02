using UnityEngine;
using UnityEngine.UI;

public class LevelLockHandler : MonoBehaviour
{
    [SerializeField] private bool isLocked = true; // Trạng thái khóa của level
    [SerializeField] private Material lockedMaterial; // Material màu đen
    [SerializeField] private Image lockIconPrefab; // Prefab hình ổ khóa

    private Material[][] originalMaterials; // Lưu tất cả các material gốc
    private GameObject lockIconInstance;

    void Start()
    {
        if (isLocked)
        {
            ApplyLockedState();
        }
    }

    private void ApplyLockedState()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length][]; // Khởi tạo mảng lưu materials gốc

        for (int i = 0; i < renderers.Length; i++)
        {
            // Lưu tất cả các material gốc trong renderer
            originalMaterials[i] = renderers[i].materials;

            // Thay tất cả material trong renderer thành lockedMaterial
            Material[] lockedMaterials = new Material[renderers[i].materials.Length];
            for (int j = 0; j < lockedMaterials.Length; j++)
            {
                lockedMaterials[j] = lockedMaterial;
            }
            renderers[i].materials = lockedMaterials;
        }

        // Thêm hình ổ khóa vào đối tượng cha
        /*if (lockIconPrefab != null)
        {
            lockIconInstance = //Instantiate(lockIconPrefab, transform);
            lockIconInstance.transform.localPosition = new Vector3(0, 2, 0); // Điều chỉnh vị trí hiển thị ổ khóa
        }*/
    }

    public void UnlockLevel()
    {
        if (isLocked)
        {
            isLocked = false;

            // Khôi phục tất cả material gốc
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].materials = originalMaterials[i];
            }

            // Xóa hình ổ khóa
            if (lockIconInstance != null)
            {
                Destroy(lockIconInstance);
            }
        }
    }
}
