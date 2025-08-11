using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 40f;
    public float destroyDelay = 2f;
    public bool IsDead { get; private set; }

    float currentHealth;
    Animator anim;
    NavMeshAgent agent;
    Collider[] colliders;
    HitFlash flash;

    [Header("Popup")]
    public DamagePopup popupPrefab;
    public Color popupColor = new Color(1f, 0.85f, 0.2f, 1f);

    void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        colliders = GetComponentsInChildren<Collider>();
        flash = GetComponent<HitFlash>();
    }

    public void TakeHit(float amount, Vector3 hitPoint)
    {
        if (IsDead) return;

        currentHealth -= amount;

        if (flash != null) flash.Flash();
        if (popupPrefab != null) DamagePopup.Spawn(popupPrefab, hitPoint, amount, popupColor);
        Hitmarker.Trigger();

        if (currentHealth <= 0f)
        {
            Die();
        }
        else
        {
            if (anim) anim.SetTrigger("Hit");
        }
    }

    void Die()
    {
        IsDead = true;
        if (agent) agent.enabled = false;
        if (colliders != null) foreach (var c in colliders) if (c) c.enabled = false;
        if (anim) anim.SetTrigger("Die");

        if (InventorySystem.Instance != null)
        {
            InventorySystem.Instance.AddToInventory("Bone");
            InventorySystem.Instance.RecalculateList();
        }

        Destroy(gameObject, destroyDelay);
    }
}