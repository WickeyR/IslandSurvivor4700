using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroSequence : MonoBehaviour
{
    [Serializable]
    public class Slide
    {
        [TextArea] public string text;
        public float fadeIn  = 0.6f;
        public float hold    = 2.5f;
        public float fadeOut = 0.6f;
        public AudioClip sfx;
        public float sfxDelay = 0f;
    }

    [Header("UI")]
    public CanvasGroup textGroup;
    public TextMeshProUGUI introText;

    [Header("Audio")]
    public AudioSource musicSource;
    public AudioSource sfxSource;   // optional second source for one-shots

    [Header("Sequence")]
    public List<Slide> slides = new List<Slide>();

    [Header("Flow")]
    public string nextSceneName = "Main";
    public bool allowSkip = true;
    public float skipAvailableAfter = 1.0f;   // seconds
    public KeyCode skipKey = KeyCode.Space;

    void Start()
    {
        if (textGroup) textGroup.alpha = 0f;
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        float elapsed = 0f;
        foreach (var slide in slides)
        {
            if (introText) introText.text = slide.text;
            yield return StartCoroutine(Fade(textGroup, 0f, 1f, slide.fadeIn));
            float t = 0f;

            if (slide.sfx && sfxSource)
                StartCoroutine(PlaySfxDelayed(slide.sfx, slide.sfxDelay));

            while (t < slide.hold)
            {
                t += Time.deltaTime;
                elapsed += Time.deltaTime;
                if (allowSkip && elapsed >= skipAvailableAfter && Input.GetKeyDown(skipKey))
                {
                    LoadNext();
                    yield break;
                }
                yield return null;
            }

            yield return StartCoroutine(Fade(textGroup, 1f, 0f, slide.fadeOut));
        }

        LoadNext();
    }

    IEnumerator Fade(CanvasGroup g, float from, float to, float dur)
    {
        if (!g || dur <= 0f)
        {
            if (g) g.alpha = to;
            yield break;
        }
        float t = 0f;
        g.alpha = from;
        while (t < dur)
        {
            t += Time.deltaTime;
            g.alpha = Mathf.Lerp(from, to, t / dur);
            yield return null;
        }
        g.alpha = to;
    }

    IEnumerator PlaySfxDelayed(AudioClip clip, float delay)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);
        if (sfxSource && clip) sfxSource.PlayOneShot(clip);
    }

    void LoadNext()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }
}
