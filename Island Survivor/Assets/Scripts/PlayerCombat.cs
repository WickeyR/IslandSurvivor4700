using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackRange = 2.5f;
    public float damagePerHit = 10f;
    public float attackCooldown = 0.5f;

    float _cooldown;

    void Update()
    {
        _cooldown -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && _cooldown <= 0f)
        {
            _cooldown = attackCooldown;
            TryAttack();
        }
    }

    void TryAttack()
    {
        Camera cam = Camera.main;
        if (!cam) return;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out var hit, attackRange, ~0, QueryTriggerInteraction.Ignore))
        {
            var eh = hit.collider.GetComponentInParent<EnemyHealth>();
            if (eh != null && !eh.IsDead)
            {
                eh.TakeHit(damagePerHit, hit.point);
            }
        }
    }
}