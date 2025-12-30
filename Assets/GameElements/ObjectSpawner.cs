using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab;   
    public float spawnIntervalMin = 1f; 
    public float spawnIntervalMax = 3f; 
    
    public float spawnX = 12f;        
    public float spawnYMin = -4f;     
    public float spawnYMax = 4f;     

    void Start()
    {
      
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
           
            float waitTime = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(waitTime);

            SpawnObject();
        }
    }

    void SpawnObject()
    {
       
        float randomY = Random.Range(spawnYMin, spawnYMax);
        Vector3 spawnPos = new Vector3(spawnX, randomY, 0);

       
        Instantiate(objectPrefab, spawnPos, Quaternion.identity);
    }

   
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(spawnX, spawnYMin, 0), new Vector3(spawnX, spawnYMax, 0));
    }
}