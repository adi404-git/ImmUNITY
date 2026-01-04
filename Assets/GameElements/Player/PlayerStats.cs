using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
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
    SpriteRenderer sr;
    Color originalColor;

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
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        StartCoroutine(DamageFlash());
        if (healthBar)
            healthBar.value = currentHealth;

        if (currentHealth <= 0)
            Die();
        
        if (CombatLog.Instance)
        CombatLog.Instance.Log("-" + damage + " HP");

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
    GameManager.Instance.TriggerDefeat();
    gameObject.SetActive(false);
}


    public void GainXP(int amount)
    {
        currentXP += amount;

        if (currentXP >= xpToNextLevel)
            LevelUp();

        if (xpBar)
            xpBar.value = currentXP;
        if (CombatLog.Instance)
        CombatLog.Instance.Log("+" + amount + " XP");

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
        if (CombatLog.Instance)
        CombatLog.Instance.Log("Level Up!");

    }

    void UpdateLevelText()
    {
        if (levelText)
            levelText.text = "Lvl " + currentLevel;
    }
    IEnumerator DamageFlash()
{
    sr.color = new Color(1f, 0.6f, 0.6f);
    yield return new WaitForSeconds(0.1f);
    sr.color = originalColor;
}

}
