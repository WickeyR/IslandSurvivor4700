using UnityEngine;
using UnityEngine.SceneManagement;

public class BoatEndingController : MonoBehaviour
{
    public static BoatEndingController Instance;

    public CanvasGroup fadeGroup;
    public float fadeInTime  = 1f;
    public float holdBlack   = 1f;
    public float postBlackDelay = 2f;

    public enum EndAction { LoadScene, RestartToScene, QuitApp }
    public EndAction endAction = EndAction.LoadScene;

    public string endSceneName = "Ending";
    public string restartSceneName = "Main";

    void Awake()
    {
        Instance = this;
        if (fadeGroup) fadeGroup.alpha = 0f;
    }

    public static void PlayEnding()
    {
        if (Instance) Instance.StartCoroutine(Instance.DoEnding());
    }

    System.Collections.IEnumerator DoEnding()
    {
        Time.timeScale = 1f;

        if (fadeGroup)
        {
            float t = 0f;
            while (t < fadeInTime)
            {
                t += Time.deltaTime;
                fadeGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeInTime);
                yield return null;
            }
            fadeGroup.alpha = 1f;
            yield return new WaitForSeconds(holdBlack);
        }

        if (postBlackDelay > 0f) yield return new WaitForSeconds(postBlackDelay);

        switch (endAction)
        {
            case EndAction.LoadScene:
                if (!string.IsNullOrEmpty(endSceneName))
                    SceneManager.LoadScene(endSceneName);
                break;
            case EndAction.RestartToScene:
                if (!string.IsNullOrEmpty(restartSceneName))
                    SceneManager.LoadScene(restartSceneName);
                break;
            case EndAction.QuitApp:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
                if (!string.IsNullOrEmpty(restartSceneName))
                    SceneManager.LoadScene(restartSceneName);
#else
                Application.Quit();
#endif
                break;
        }
    }
}
