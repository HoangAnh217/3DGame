using UnityEngine;
using UnityEditor;

public class ReplaceWithPrefab : MonoBehaviour
{
    [SerializeField] private GameObject prefab; // Kéo Prefab vào đây trong Inspector

    [MenuItem("Tools/Replace Selected With Prefab")]
    static void ReplaceSelectedWithPrefab()
    {
        ReplaceWithPrefab instance = FindObjectOfType<ReplaceWithPrefab>();
        if (instance == null || instance.prefab == null)
        {
            Debug.LogError("Hãy kéo Prefab hợp lệ vào ô Prefab trong Inspector.");
            return;
        }

        GameObject prefab = instance.prefab;

        // Lấy tất cả các đối tượng được chọn
        GameObject[] selectedObjects = Selection.gameObjects;
        foreach (GameObject obj in selectedObjects)
        {
            Vector3 position = obj.transform.position;
            Quaternion rotation = obj.transform.rotation;
            Vector3 scale = obj.transform.localScale;

            DestroyImmediate(obj);

            GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (newObject != null)
            {
                newObject.transform.position = position;
                newObject.transform.rotation = rotation;
                newObject.transform.localScale = scale;
            }
            else
            {
                Debug.LogError("Không thể khởi tạo Prefab: " + prefab.name);
            }
        }

        Debug.Log("Thay thế hoàn tất!");
    }
}
