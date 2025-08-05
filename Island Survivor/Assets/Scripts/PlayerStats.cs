using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Max Values")]
    public float maxHealth = 100f;
    public float maxHunger = 100f;

    [Header("Depletion Rates (per second)")]
    public float hungerDepletionRate = 0.5f;
    public float healthDepletionRate = 1f;

    [Header("UI")]
    public Slider healthBar;
    public Slider hungerBar;

    private float currentHealth;
    private float currentHunger;

    void Start()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        UpdateUI();
    }

    void Update()
    {
        currentHunger -= hungerDepletionRate * Time.deltaTime;
        currentHunger = Mathf.Max(currentHunger, 0f);

        if (currentHunger <= 0f)
        {
            currentHealth -= healthDepletionRate * Time.deltaTime;
            currentHealth = Mathf.Max(currentHealth, 0f);
        }

        UpdateUI();
    }

    public void Eat(float hungerAmount, float healthAmount = 0f)
    {
        currentHunger += hungerAmount;
        currentHunger = Mathf.Min(currentHunger, maxHunger);

        currentHealth += healthAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (healthBar != null)
            healthBar.value = currentHealth / maxHealth;
        if (hungerBar != null)
            hungerBar.value = currentHunger / maxHunger;
    }
}