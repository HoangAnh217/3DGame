using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public Slider loadingSlider;

    void Start()
    {
        StartCoroutine(LoadMainSceneAsync());
    }

    IEnumerator LoadMainSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        operation.allowSceneActivation = false;

        float elapsedTime = 0f;

        while (!operation.isDone)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;

            if (operation.progress >= 0.9f && elapsedTime >= 0.7f)
            {
                yield return new WaitForSeconds(0.2f); // có thể giữ thêm chút để chuyển mượt
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
