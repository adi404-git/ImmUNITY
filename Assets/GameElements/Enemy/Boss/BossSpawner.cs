using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public BossMRSA boss;
    public int spawnLevel = 7;

    PlayerStats playerStats;
    bool spawned;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();

        if (boss)
            boss.gameObject.SetActive(false);
    }

    void Update()
    {
        if (spawned || playerStats == null || boss == null)
            return;

        if (playerStats.currentLevel >= spawnLevel)
        {
            boss.gameObject.SetActive(true);
            boss.InitializeBoss();
            spawned = true;
        }
    }
}
