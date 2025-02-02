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

    public IEnumerator LoadLevel()
    {
        transitionAnim.SetTrigger("Start");


        yield return new WaitForSeconds(0.6f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        asyncLoad.allowSceneActivation = false; // Tạm thời không kích hoạt scene ngay lập tức

        while (!asyncLoad.isDone)
        {
            // Kiểm tra nếu scene tải xong, kích hoạt khi fade in sẵn sàng
            if (asyncLoad.progress >= 0.9f)
            {
                // Kích hoạt scene khi fade in hoàn tất
                asyncLoad.allowSceneActivation = true;
            }

            yield return null; // Chờ cho frame tiếp theo
        }

        // Bắt đầu fade in
        transitionAnim.SetTrigger("End");
        // Đợi fade in hoàn tất
        yield return new WaitForSeconds(0.3f);
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
