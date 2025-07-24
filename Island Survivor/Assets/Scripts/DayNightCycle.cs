using UnityEngine;
using System;

public class DayNightCycle : MonoBehaviour
{
    [Header("Cycle Settings")] [Tooltip("Duration of full day/night (seconds)")]
    public float cycleDuration = 600f;

    [Header("Lighting")] [Tooltip("Directional light representing sun/moon")]
    public Light sunLight;

    [Header("Skyboxes")] public Material daySkybox;
    public Material nightSkybox;

    [Header("Color Gradients (optional)")] public Gradient ambientColor;
    public Gradient directionalColor;

    [Range(0f, 1f)] public float timeOfDay = 0f;
    public int dayCount = 0;

    public event Action OnDayStart;
    public event Action OnNightStart;

    bool isNight = false;

    void Start()
    {
        RenderSettings.sun = sunLight;
        RenderSettings.skybox = daySkybox;
        DynamicGI.UpdateEnvironment();
        OnDayStart?.Invoke();
    }

    void Update()
    {
        // Advance time
        timeOfDay += Time.deltaTime / cycleDuration;
        if (timeOfDay >= 1f)
        {
            timeOfDay = 0f;
            dayCount++;
            OnDayStart?.Invoke();
        }

        float sunAngle = timeOfDay * 360f - 90f;
        sunLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        RenderSettings.ambientLight = ambientColor.Evaluate(timeOfDay);
        sunLight.color = directionalColor.Evaluate(timeOfDay);

        bool nextIsNight = (sunAngle < 0f || sunAngle > 180f);
        if (nextIsNight && !isNight)
        {
            isNight = true;
            RenderSettings.skybox = nightSkybox;
            DynamicGI.UpdateEnvironment();
            OnNightStart?.Invoke();
        }
        else if (!nextIsNight && isNight)
        {
            isNight = false;
            RenderSettings.skybox = daySkybox;
            DynamicGI.UpdateEnvironment();
            OnDayStart?.Invoke();
        }
    }
}
