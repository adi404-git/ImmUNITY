using UnityEngine;

public class EnemyProgressionManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public GameObject virusSpawner;

    void Start()
    {
        if (virusSpawner)
            virusSpawner.SetActive(false);
    }

    void Update()
    {
        if (playerStats.currentLevel >= 4 && virusSpawner && !virusSpawner.activeSelf)
        {
            virusSpawner.SetActive(true);
        }
    }
}
