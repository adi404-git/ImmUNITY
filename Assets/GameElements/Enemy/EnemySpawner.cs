using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;

    [Tooltip("Spawn interval at Level 1")]
    public float baseSpawnIntervalMin = 2.5f;
    public float baseSpawnIntervalMax = 4f;

    [Tooltip("How much spawn interval decreases per level")]
    public float spawnIntervalDecreasePerLevel = 0.2f;

    [Tooltip("Hard limit so spawns never get too fast")]
    public float minimumSpawnInterval = 0.8f;

    [Header("Position Settings")]
    public float spawnX = 12f;
    public float spawnYMin = -4f;
    public float spawnYMax = 4f;

    // Reference to player level
    private PlayerStats playerStats;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            float level = playerStats != null ? playerStats.currentLevel : 1;

            float minInterval =
                baseSpawnIntervalMin - (level - 1) * spawnIntervalDecreasePerLevel;
            float maxInterval =
                baseSpawnIntervalMax - (level - 1) * spawnIntervalDecreasePerLevel;

            minInterval = Mathf.Max(minInterval, minimumSpawnInterval);
            maxInterval = Mathf.Max(maxInterval, minimumSpawnInterval);

            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        float randomY = Random.Range(spawnYMin, spawnYMax);
        Vector3 spawnPos = new Vector3(spawnX, randomY, 0f);

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    // Visual Debug
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            new Vector3(spawnX, spawnYMin, 0),
            new Vector3(spawnX, spawnYMax, 0)
        );
    }
}
