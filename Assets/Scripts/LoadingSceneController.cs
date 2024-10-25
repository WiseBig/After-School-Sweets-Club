using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    [SerializeField]
    Image loadingbar;

    static string nextScene;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadingSceneProcess());
    }
    IEnumerator LoadingSceneProcess()
    {
        AsyncOperation ap = SceneManager.LoadSceneAsync(nextScene);
        ap.allowSceneActivation = false;

        float timer = 0f;
        while (!ap.isDone)
        {
            yield return null;

            if(ap.progress < 0.9f)
            {
                loadingbar.fillAmount = ap.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                loadingbar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if(loadingbar.fillAmount >= 1f)
                {
                    ap.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
