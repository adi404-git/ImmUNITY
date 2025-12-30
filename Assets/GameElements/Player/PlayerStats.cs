using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar;

    public int currentLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;
    public float xpDifficultyMultiplier = 1.2f;

    public Slider xpBar;
    public TextMeshProUGUI levelText;

    public UpgradeManager upgradeManager;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (xpBar)
        {
            xpBar.maxValue = xpToNextLevel;
            xpBar.value = currentXP;
        }

        UpdateLevelText();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (healthBar)
            healthBar.value = currentHealth;

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        if (healthBar)
            healthBar.value = currentHealth;
    }

    void Die()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(false);
    }

    public void GainXP(int amount)
    {
        currentXP += amount;

        if (currentXP >= xpToNextLevel)
            LevelUp();

        if (xpBar)
            xpBar.value = currentXP;
    }

    void LevelUp()
    {
        currentXP -= xpToNextLevel;
        currentLevel++;

        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpDifficultyMultiplier);

        if (xpBar)
        {
            xpBar.maxValue = xpToNextLevel;
            xpBar.value = currentXP;
        }

        UpdateLevelText();
        Heal(maxHealth);

        if (upgradeManager)
            upgradeManager.OpenUpgradePanel();
    }

    void UpdateLevelText()
    {
        if (levelText)
            levelText.text = "Lvl " + currentLevel;
    }
}
