using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyWander : MonoBehaviour
{
    [Header("Wander Settings")]
    public float wanderRadius   = 10f;
    public float wanderInterval = 5f;

    // Components
    private NavMeshAgent agent;
    private Animator      anim;
    private float         timer;

    void Awake()
    {
        // cache components
        agent = GetComponent<NavMeshAgent>();
        anim  = GetComponent<Animator>();

        // ensure agent moves your transform
        agent.updatePosition = true;
        agent.updateRotation = true;
    }

    void OnEnable()
    {
        timer = wanderInterval;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= wanderInterval)
        {
            timer = 0f;
            Vector3 newPos = RandomNavmeshLocation(wanderRadius);

            if (Vector3.Distance(transform.position, newPos) < 0.1f)
            {
                Debug.LogWarning($"[EnemyWander] SamplePosition returned same spot for {name}");
            }
            else
            {
                Debug.DrawLine(transform.position, newPos, Color.red, wanderInterval);
                agent.SetDestination(newPos);
            }
        }

    
        Vector3 horizontalVel = Vector3.ProjectOnPlane(agent.velocity, Vector3.up);
        anim.SetFloat("Speed", horizontalVel.magnitude);
    }

    Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDir = Random.insideUnitSphere * radius + transform.position;
        if (NavMesh.SamplePosition(randomDir, out var hit, radius, NavMesh.AllAreas))
            return hit.position;

        return transform.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
