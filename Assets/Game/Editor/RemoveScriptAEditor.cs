using UnityEditor;
using UnityEngine;

public class RemoveScriptAEditor : EditorWindow
{
    [MenuItem("Tools/Remove Script A From Children")]
    static void RemoveScriptAFromSelected()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogWarning("Vui lòng chọn một GameObject.");
            return;
        }

        GameObject selected = Selection.activeGameObject;
        LockToGrid[] components = selected.GetComponentsInChildren<LockToGrid>(true); // true = bao gồm cả inactive

        int count = 0;
        foreach (LockToGrid comp in components)
        {
            Undo.DestroyObjectImmediate(comp); // Hỗ trợ undo
            count++;
        }

        Debug.Log($"Đã xóa {count} script A khỏi {selected.name} và các con.");
    }
}
