using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject objectPrefab;    // Drag your RBC Prefab here
    public float spawnIntervalMin = 1f; // Fastest spawn rate
    public float spawnIntervalMax = 3f; // Slowest spawn rate
    
    [Header("Position Settings")]
    public float spawnX = 12f;         // Just off-screen to the Right
    public float spawnYMin = -4f;      // Bottom wall limit
    public float spawnYMax = 4f;       // Top wall limit

    void Start()
    {
        // Start the infinite spawning loop
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true) // Run forever
        {
            // 1. Calculate a random wait time
            float waitTime = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(waitTime);

            SpawnObject();
        }
    }

    void SpawnObject()
    {
        // 2. Pick a random height (Y)
        float randomY = Random.Range(spawnYMin, spawnYMax);
        Vector3 spawnPos = new Vector3(spawnX, randomY, 0);

        // 3. Instantiate (Create) the object
        Instantiate(objectPrefab, spawnPos, Quaternion.identity);
    }

    // Visual Debug to see where the spawn line is
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(spawnX, spawnYMin, 0), new Vector3(spawnX, spawnYMax, 0));
    }
}