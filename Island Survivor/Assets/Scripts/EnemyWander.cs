using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyWander : MonoBehaviour
{
    [Header("Wander Settings")]
    public float wanderRadius   = 10f;
    public float wanderInterval = 5f;

    [Header("Chase Settings")]
    public GameObject playerObject;
    public float      chaseRadius  = 8f;
    public float      viewAngle    = 120f;
    public LayerMask  obstacleMask;

    [Header("Attack Settings")]
    [Tooltip("How close the enemy must be to hit you")]
    public float attackRange    = 1.5f;
    [Tooltip("Damage dealt each hit")]
    public float damagePerHit   = 10f;
    [Tooltip("Seconds between hits")]
    public float attackInterval = 1f;

    private Transform     player;
    private PlayerStats   playerStats;
    private NavMeshAgent  agent;
    private Animator      anim;
    private float         timer;
    private float         attackTimer;

    private enum State { Wander, Chase }
    private State current = State.Wander;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim  = GetComponent<Animator>();
        agent.updatePosition = true;
        agent.updateRotation = true;

        // find player transform
        if (playerObject != null)
            player = playerObject.transform;
        else
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) player = go.transform;
        }

        // cache their PlayerStats component
        if (player != null)
            playerStats = player.GetComponent<PlayerStats>();
    }

    void OnEnable()
    {
        timer = wanderInterval;
    }

    void Update()
    {
        if (player != null && CanSeePlayer())
            current = State.Chase;
        else if (current == State.Chase
              && player != null
              && Vector3.Distance(transform.position, player.position) > chaseRadius * 1.2f)
        {
            current = State.Wander;
            timer   = wanderInterval;
        }

        switch (current)
        {
            case State.Chase:
                agent.SetDestination(player.position);
                TryAttackPlayer();
                break;

            case State.Wander:
                timer += Time.deltaTime;
                if (timer >= wanderInterval)
                {
                    timer = 0f;
                    Vector3 newPos = RandomNavmeshLocation(wanderRadius);
                    if (Vector3.Distance(transform.position, newPos) > 0.1f)
                        agent.SetDestination(newPos);
                }
                break;
        }

        // update run/walk animation speed
        Vector3 horizVel = Vector3.ProjectOnPlane(agent.velocity, Vector3.up);
        anim.SetFloat("Speed", horizVel.magnitude);
    }

    private void TryAttackPlayer()
    {
        if (playerStats == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= attackRange)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                attackTimer = 0f;
                playerStats.TakeDamage(damagePerHit);
            }
        }
        else
        {
            // reset timer if out of range
            attackTimer = 0f;
        }
    }

    Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDir = Random.insideUnitSphere * radius + transform.position;
        if (NavMesh.SamplePosition(randomDir, out var hit, radius, NavMesh.AllAreas))
            return hit.position;
        return transform.position;
    }

    bool CanSeePlayer()
    {
        Vector3 eyePos   = transform.position + Vector3.up * 1.5f;
        Vector3 toPlayer = player.position - eyePos;
        float   dist     = toPlayer.magnitude;
        if (dist > chaseRadius) return false;
        if (Vector3.Angle(transform.forward, toPlayer) > viewAngle * 0.5f) return false;
        if (Physics.Raycast(eyePos, toPlayer.normalized, out var hit, chaseRadius, ~obstacleMask))
            return hit.transform == player;
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
