using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] private Animator transitionAnim;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectLevel(int level)
    {
        StartCoroutine(LoadSceneWithFade(level));
    }

    public IEnumerator LoadSceneWithFade(int sceneIndex)
    {
        // Bắt đầu fade out
        transitionAnim.SetTrigger("Start");
        yield return new WaitForSeconds(0.6f);

        // Load scene bất đồng bộ
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false;

        // Chờ load gần xong
        Debug.Log("1");
        while (asyncLoad.progress >=1)
        {
            yield return null;
        }

        // Đợi thêm nếu muốn giữ loading một chút
        yield return new WaitForSeconds(0.2f);
        Debug.Log("2");
        // Cho phép kích hoạt scene mới
        asyncLoad.allowSceneActivation = true;
        Debug.Log("3");
        // Đợi scene hoàn toàn được kích hoạt
        /*while (!asyncLoad.isDone)
        {
            yield return null;
        }*/

        // Fade in sau khi scene đã load

        Debug.Log("4");

        transitionAnim.SetTrigger("End");
    }
}
