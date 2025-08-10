using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public TextMeshPro text;
    public float lifetime = 0.8f;
    public float riseSpeed = 1.2f;
    public float scaleIn  = 0.08f;
    public float scaleOut = 0.15f;

    float _age;
    Camera _cam;

    public static void Spawn(DamagePopup prefab, Vector3 worldPos, float amount, Color color)
    {
        var p = Instantiate(prefab, worldPos, Quaternion.identity);
        p.text.text  = Mathf.RoundToInt(amount).ToString();
        p.text.color = color;
    }

    void Awake()
    {
        _cam = Camera.main;
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        _age += Time.deltaTime;
        if (_cam)
        {
            transform.forward = (_cam.transform.position - transform.position).normalized * -1f;
        }

        transform.position += Vector3.up * riseSpeed * Time.deltaTime;

        if (_age < scaleIn)
        {
            float k = _age / scaleIn;
            transform.localScale = Vector3.LerpUnclamped(Vector3.zero, Vector3.one, Mathf.SmoothStep(0,1,k));
        }
        else if (_age > lifetime - scaleOut)
        {
            float k = Mathf.InverseLerp(lifetime, lifetime - scaleOut, _age);
            transform.localScale = Vector3.LerpUnclamped(Vector3.one, Vector3.one * 0.6f, Mathf.SmoothStep(0,1,k));
            var c = text.color; c.a = Mathf.Lerp(1f, 0f, k); text.color = c;
        }

        if (_age >= lifetime) Destroy(gameObject);
    }
}