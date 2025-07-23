using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitData : MonoBehaviour
{
    [SerializeField] private Transform environmentParent; // Chỗ để spawn environment
    [SerializeField] private int levelIndex = -1; // Dùng test nếu chưa có hệ thống load level

    void Start()
    {
       /* if (levelIndex < 0)
            levelIndex = PlayerPrefs.GetInt("CurrentLevel", 1);

        string path = $"Environments/Environment_Level{levelIndex}";
        GameObject prefab = Resources.Load<GameObject>(path);

        if (prefab != null)
        {
            GameObject env = Instantiate(prefab, environmentParent);
            env.name = $"Environment_Level{levelIndex}";
        }
        else
        {
            Debug.LogError($"Không tìm thấy prefab tại: {path}");
        }*/
    }
}
