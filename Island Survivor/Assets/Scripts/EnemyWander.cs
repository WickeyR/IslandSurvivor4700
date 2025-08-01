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

    private Transform    player;
    private NavMeshAgent agent;
    private Animator     anim;
    private float        timer;

    private enum State { Wander, Chase }
    private State current = State.Wander;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim  = GetComponent<Animator>();
        agent.updatePosition = true;
        agent.updateRotation = true;

        if (playerObject != null)
            player = playerObject.transform;
        else
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) player = go.transform;
        }
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

        Vector3 horizVel = Vector3.ProjectOnPlane(agent.velocity, Vector3.up);
        anim.SetFloat("Speed", horizVel.magnitude);
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
        float angle = Vector3.Angle(transform.forward, toPlayer);
        if (angle > viewAngle * 0.5f) return false;
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
        Vector3 rightDir = Quaternion.Euler(0, viewAngle * 0.5f, 0) * transform.forward;
        Vector3 leftDir  = Quaternion.Euler(0, -viewAngle * 0.5f, 0) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + rightDir * chaseRadius);
        Gizmos.DrawLine(transform.position, transform.position + leftDir  * chaseRadius);
    }
}
