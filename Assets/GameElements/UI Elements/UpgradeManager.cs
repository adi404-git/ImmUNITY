using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeCanvas;

    public PlayerStats playerStats;
    public PlayerMovement playerMovement;
    public PlayerCombat playerCombat;

    bool choosing = false;

    void Start()
    {
        upgradeCanvas.SetActive(false);
    }

    public void OpenUpgradePanel()
    {
        if (choosing) return;

        choosing = true;
        Time.timeScale = 0f;
        upgradeCanvas.SetActive(true);
    }
    
    // ---------- UPGRADE CARDS ----------

    public void UpgradeMobility()
    {
        // +Move Speed, -Max Health
        playerMovement.moveSpeed *= 1.15f;

        playerStats.maxHealth = Mathf.Max(
            Mathf.RoundToInt(playerStats.maxHealth * 0.9f),
            20
        );

        if (playerStats.currentHealth > playerStats.maxHealth)
            playerStats.currentHealth = playerStats.maxHealth;

        Close();
    }

    public void UpgradeVitality()
    {
        // +Max Health, -Move Speed
        playerStats.maxHealth = Mathf.RoundToInt(playerStats.maxHealth * 1.2f);
        playerStats.currentHealth = playerStats.maxHealth;

        playerMovement.moveSpeed *= 0.9f;

        Close();
    }

    public void UpgradePower()
    {
        // +Dash Force, -Move Speed
        playerCombat.dashForce *= 1.15f;
        playerMovement.moveSpeed *= 0.9f;

        Close();
    }

    void Close()
    {
        upgradeCanvas.SetActive(false);
        Time.timeScale = 1f;
        choosing = false;
    }
}
