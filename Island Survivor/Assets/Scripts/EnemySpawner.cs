using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject playerObject;   
    public int        spawnCount  = 5;
    public float      spawnRadius = 20f;

    private List<GameObject> spawned = new List<GameObject>();
    private DayNightCycle    cycle;

    void Start()
    {
        if (playerObject == null)
            playerObject = GameObject.FindGameObjectWithTag("Player");

        cycle = FindObjectOfType<DayNightCycle>();
        cycle.OnNightStart += SpawnEnemies;
        cycle.OnDayStart  += DespawnEnemies;
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 rnd = new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                0,
                Random.Range(-spawnRadius, spawnRadius)
            );
            Vector3 pos = transform.position + rnd;
            if (NavMesh.SamplePosition(pos, out var hit, spawnRadius, NavMesh.AllAreas))
            {
                var enemy = Instantiate(enemyPrefab, hit.position, Quaternion.identity);
                var wander = enemy.GetComponent<EnemyWander>();
                if (wander != null)
                    wander.playerObject = playerObject;
                spawned.Add(enemy);
            }
        }
    }

    void DespawnEnemies()
    {
        foreach (var e in spawned)
            if (e) Destroy(e);
        spawned.Clear();
    }

    void OnDestroy()
    {
        if (cycle != null)
        {
            cycle.OnNightStart -= SpawnEnemies;
            cycle.OnDayStart  -= DespawnEnemies;
        }
    }
}