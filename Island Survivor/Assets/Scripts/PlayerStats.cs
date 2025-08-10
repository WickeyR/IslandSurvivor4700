using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Max Values")]
    public float maxHealth = 100f;
    public float maxHunger = 100f;
    public float maxThirst = 100f;

    [Header("Depletion Rates (per second)")]
    public float hungerDepletionRate = 0.5f;
    public float thirstDepletionRate = 0.7f;
    public float healthDepletionRate = 1f; 

    [Header("UI Sliders")]
    public Slider healthBar;
    public Slider hungerBar;
    public Slider thirstBar;

    [Header("Damage Flash UI")]
    public Image damageFlashImage;
    [Range(0f,1f)] public float flashMaxAlpha = 0.5f;
    public float flashDuration = 0.1f;
    public float flashFadeTime = 0.5f;

    [Header("Respawn UI")]
    public GameObject respawnPanel;
    public Transform spawnPoint;

    private float currentHealth;
    private float currentHunger;
    private float currentThirst;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentThirst = maxThirst;
        UpdateUI();

        if (damageFlashImage != null)
        {
            Color c = damageFlashImage.color;
            c.a = 0f;
            damageFlashImage.color = c;
        }

        if (respawnPanel != null)
            respawnPanel.SetActive(false);
    }

    void Update()
    {
        if (isDead) return;

        // Hunger depletion
        currentHunger -= hungerDepletionRate * Time.deltaTime;
        currentHunger = Mathf.Max(currentHunger, 0f);

        // Thirst depletion
        currentThirst -= thirstDepletionRate * Time.deltaTime;
        currentThirst = Mathf.Max(currentThirst, 0f);

        // Damage from starvation or dehydration
        if (currentHunger <= 0f || currentThirst <= 0f)
            TakeDamage(healthDepletionRate * Time.deltaTime, false);

        UpdateUI();
    }

    public void TakeDamage(float amount, bool showFlash = true)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f);
        UpdateUI();

        if (showFlash && damageFlashImage != null)
            StartCoroutine(DamageFlash());

        if (currentHealth <= 0f)
            Die();
    }

    private IEnumerator DamageFlash()
    {
        Color c = damageFlashImage.color;
        c.a = flashMaxAlpha;
        damageFlashImage.color = c;

        yield return new WaitForSecondsRealtime(flashDuration);

        float elapsed = 0f;
        while (elapsed < flashFadeTime)
        {
            elapsed += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(flashMaxAlpha, 0f, elapsed / flashFadeTime);
            damageFlashImage.color = c;
            yield return null;
        }

        c.a = 0f;
        damageFlashImage.color = c;
    }

    private void Die()
    {
        isDead = true;
        var pc = GetComponent<PlayerController>();
        if (pc != null) pc.enabled = false;

        if (respawnPanel != null)
            respawnPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void OnRespawnButton()
    {
        Time.timeScale = 1f;

        if (respawnPanel != null)
            respawnPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentThirst = maxThirst;
        UpdateUI();

        if (spawnPoint != null)
            transform.position = spawnPoint.position;

        var pc = GetComponent<PlayerController>();
        if (pc != null) pc.enabled = true;

        isDead = false;
    }

    private void UpdateUI()
    {
        if (healthBar != null) healthBar.value = currentHealth / maxHealth;
        if (hungerBar != null) hungerBar.value = currentHunger / maxHunger;
        if (thirstBar != null) thirstBar.value = currentThirst / maxThirst;
    }

    public void Eat(float hungerAmount, float healthAmount = 0f)
    {
        currentHunger = Mathf.Min(currentHunger + hungerAmount, maxHunger);
        currentHealth = Mathf.Min(currentHealth + healthAmount, maxHealth);
        UpdateUI();
    }

    public void Drink(float thirstAmount)
    {
        currentThirst = Mathf.Min(currentThirst + thirstAmount, maxThirst);
        UpdateUI();
    }

    public void DrinkFull()
    {
        currentThirst = maxThirst;
        UpdateUI();
    }
}
