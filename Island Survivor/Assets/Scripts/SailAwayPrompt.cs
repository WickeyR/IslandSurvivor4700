using UnityEngine;
using UnityEngine.UI;

public class SailAwayPrompt : MonoBehaviour
{
    public QuestMenu questMenu;
    public CanvasGroup promptGroup;
    public Button sailButton;
    public float fadeInTime = 0.2f;

    bool shown;

    void Awake()
    {
        if (promptGroup) { promptGroup.alpha = 0f; promptGroup.blocksRaycasts = false; promptGroup.interactable = false; }
        if (sailButton) sailButton.onClick.AddListener(OnSailClicked);
    }

    void Start()
    {
        if (!questMenu) questMenu = FindObjectOfType<QuestMenu>();
        if (questMenu) questMenu.OnObjectivesCompleteChanged += OnObjectivesChanged;
        if (questMenu && IsComplete()) Show(true);
    }

    void OnDestroy()
    {
        if (questMenu) questMenu.OnObjectivesCompleteChanged -= OnObjectivesChanged;
        if (sailButton) sailButton.onClick.RemoveListener(OnSailClicked);
    }

    void OnObjectivesChanged(bool complete)
    {
        if (complete) Show(true);
    }

    bool IsComplete()
    {
        return questMenu && questMenu.AllObjectivesComplete;
    }

    void Show(bool show)
    {
        shown = show;
        StopAllCoroutines();
        StartCoroutine(FadeTo(show ? 1f : 0f));
        if (promptGroup) { promptGroup.blocksRaycasts = show; promptGroup.interactable = show; }
        if (show) { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
    }

    System.Collections.IEnumerator FadeTo(float a)
    {
        if (!promptGroup) yield break;
        float s = promptGroup.alpha, t = 0f;
        float d = Mathf.Max(0.0001f, fadeInTime);
        while (t < d)
        {
            t += Time.unscaledDeltaTime;
            promptGroup.alpha = Mathf.Lerp(s, a, t / d);
            yield return null;
        }
        promptGroup.alpha = a;
    }

    void OnSailClicked()
    {
        Show(false);
        BoatEndingController.PlayEnding();
    }
}