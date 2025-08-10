using System.Collections;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    [ColorUsage(false, true)] public Color flashColor = new Color(10, 10, 10, 1);
    public float flashDuration = 0.08f;
    public float fadeDuration  = 0.18f;
    public Renderer[] renderers;

    MaterialPropertyBlock _block;
    static readonly int _EmissionColor = Shader.PropertyToID("_EmissionColor");

    void Awake()
    {
        _block = new MaterialPropertyBlock();
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(DoFlash());
    }

    IEnumerator DoFlash()
    {
        float t = 0f;
        while (t < flashDuration)
        {
            t += Time.deltaTime;
            SetEmission(Color.Lerp(flashColor, Color.black, t / flashDuration));
            yield return null;
        }
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            SetEmission(Color.Lerp(Color.black, Color.black, t / fadeDuration));
            yield return null;
        }
        SetEmission(Color.black);
    }

    void SetEmission(Color c)
    {
        if (renderers == null) return;
        for (int i = 0; i < renderers.Length; i++)
        {
            var r = renderers[i];
            if (!r) continue;
            r.GetPropertyBlock(_block);
            _block.SetColor(_EmissionColor, c);
            r.SetPropertyBlock(_block);
        }
    }
}