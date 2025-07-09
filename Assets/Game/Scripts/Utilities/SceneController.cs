using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    [SerializeField] private Animator transitionAnim;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Không phá hủy object này khi chuyển scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectLevel(int level)
    {
        StartCoroutine(SelectLevelPlay(level));
    }

    public IEnumerator LoadLevel(int scenceIndex)
    {
        // Bắt đầu fade out
        transitionAnim.SetTrigger("Start");
        yield return new WaitForSeconds(0.6f); // Đợi fade out hoàn tất

        // Bắt đầu tải scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scenceIndex);
        asyncLoad.allowSceneActivation = false; // Tạm thời không kích hoạt scene ngay lập tức

        while (!asyncLoad.isDone)
        {
            
            // Đợi fade in hoàn tất trước khi kích hoạt scene
            asyncLoad.allowSceneActivation = true;
            transitionAnim.SetTrigger("End");
            yield return new WaitForSeconds(0.3f); // Đợi fade in hoàn tất

            // Kích hoạt scene

            yield return null; // Chờ cho frame tiếp theo
        }

        Debug.Log("Scene loaded and activated.");
    }
    public IEnumerator SelectLevelPlay(int level)
    {
        transitionAnim.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        int nextSceneIndex = level;

        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadSceneAsync(nextSceneIndex);

        transitionAnim.SetTrigger("End");
    }
}
