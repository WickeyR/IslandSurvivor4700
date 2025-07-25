using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int spawnCount = 5;
    public float spawnRadius = 20f;

    private List<GameObject> spawned = new();
    private DayNightCycle cycle;

    void Start()
    {
        cycle = FindObjectOfType<DayNightCycle>();
        cycle.OnNightStart += SpawnEnemies;
        cycle.OnDayStart   += DespawnEnemies;
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 pos = transform.position + new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                0,
                Random.Range(-spawnRadius, spawnRadius)
            );
            if (NavMesh.SamplePosition(pos, out var hit, spawnRadius, NavMesh.AllAreas))
                spawned.Add(Instantiate(enemyPrefab, hit.position, Quaternion.identity));
        }
    }

    void DespawnEnemies()
    {
        foreach (var e in spawned) if (e) Destroy(e);
        spawned.Clear();
    }

    void OnDestroy()
    {
        cycle.OnNightStart -= SpawnEnemies;
        cycle.OnDayStart   -= DespawnEnemies;
    }
}