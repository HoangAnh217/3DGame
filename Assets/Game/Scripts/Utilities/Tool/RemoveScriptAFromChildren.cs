using UnityEngine;

public class RemoveScriptAFromChildren : MonoBehaviour
{
    void Start()
    {
        // Lấy toàn bộ script A trong các object con
        LockToGrid[] components = GetComponentsInChildren<LockToGrid>(true); // true để bao gồm cả object đang inactive

        foreach (LockToGrid comp in components)
        {
            DestroyImmediate(comp); // Xóa script A khỏi object ngay lập tức
            Debug.Log($"Removed A from: {comp.gameObject.name}");
        }
    }
}
