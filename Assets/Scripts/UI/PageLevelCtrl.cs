using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PageLevelCtrl : MonoBehaviour
{
    [SerializeField] private List<GameObject> objs  = new List<GameObject>();
    [SerializeField] private GameObject holderObjBlock;
    private static int level;

    [SerializeField] private Sprite[] sprs;
    private void Start()
    {
        LoadUI_Level();
    }
    private void OnValidate()
    {
        LoadUI_Level();
    }
    public void LoadUI_Level()
    {
        level = PlayerPrefs.GetInt("Level");
        
        for (int i = 0; i < objs.Count; i++)
        {   


            if (i >= level)
            {
                Color a = Color.gray;
                objs[i].GetComponent<Image>().color = a;
                objs[i].GetComponentInChildren<TextMeshProUGUI>().color = a;
                Transform tras = objs[i].transform.Find("Star");
                tras.gameObject.SetActive(false);

                // block sign from mouse
                //holderObjBlock.transform.GetChild(i).gameObject.SetActive();

            } else
            {
                string b = "Score" + (i+1).ToString() ;
                Transform tras = objs[i].transform.Find("Star");
                int star = PlayerPrefs.GetInt(b);
                foreach (Transform t in tras)
                {
                    if (star>0)
                    {
                        return;
                    }
                    t.GetComponent<Image>().sprite = sprs[1];
                    star--;
                }
                holderObjBlock.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        Debug.Log(level);
    }
}/*
[CustomEditor(typeof(PageLevelCtrl))]
public class PlayerDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Vẽ giao diện mặc định của Inspector
        DrawDefaultInspector();

        // Lấy tham chiếu đến PlayerData
        PageLevelCtrl playerData = (PageLevelCtrl)target;

        // Tạo nút "Load Data" trong Inspector
        if (GUILayout.Button("Load Data"))
        {
            // Gọi phương thức LoadData() khi nhấn nút
            playerData.LoadUI_Level();
        }
    }
}*/
