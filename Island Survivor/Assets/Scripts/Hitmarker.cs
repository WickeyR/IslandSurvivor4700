using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Hitmarker : MonoBehaviour
{
    public static Hitmarker Instance;
    public Image image;
    public float pulseScale = 1.25f;
    public float pulseTime  = 0.08f;
    public float fadeTime   = 0.15f;

    Vector3 _baseScale;
    Color   _baseColor;

    void Awake()
    {
        Instance = this;
        _baseScale = image.rectTransform.localScale;
        _baseColor = image.color;
        var c = _baseColor; c.a = 0f; image.color = c;
    }

    public static void Trigger()
    {
        if (Instance) Instance.StartCoroutine(Instance.Pulse());
    }

    IEnumerator Pulse()
    {
        float t = 0f;
        while (t < pulseTime)
        {
            t += Time.unscaledDeltaTime;
            float k = t / pulseTime;
            image.rectTransform.localScale = Vector3.Lerp(_baseScale, _baseScale * pulseScale, k);
            var c = _baseColor; c.a = Mathf.Lerp(0f, 1f, k);
            image.color = c;
            yield return null;
        }
        t = 0f;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            float k = t / fadeTime;
            image.rectTransform.localScale = Vector3.Lerp(_baseScale * pulseScale, _baseScale, k);
            var c = _baseColor; c.a = Mathf.Lerp(1f, 0f, k);
            image.color = c;
            yield return null;
        }
        image.rectTransform.localScale = _baseScale;
        var c2 = _baseColor; c2.a = 0f; image.color = c2;
    }
}