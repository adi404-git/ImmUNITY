using UnityEngine;
using UnityEngine.UI; // Required for Sliders
using TMPro;          // Required for Text

public class PlayerStats : MonoBehaviour
{
    [Header("1. Health System")]
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar; // Drag 'HealthBar' here

    [Header("2. Experience System")]
    public int currentLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;
    public float xpDifficultyMultiplier = 1.2f;
    
    public Slider xpBar;      // Drag 'XPBar' here
    public TextMeshProUGUI levelText; // Drag 'LevelText' here

    void Start()
    {
        currentHealth = maxHealth;
        
        // Initialize Bars
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

    // --- HEALTH ---
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        // Update UI
        if (healthBar) healthBar.value = currentHealth;

        if (currentHealth <= 0) Die();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        
        // Update UI
        if (healthBar) healthBar.value = currentHealth;
    }

    void Die()
    {
        Debug.Log("Game Over");
        gameObject.SetActive(false);
    }

    // --- XP ---
    public void GainXP(int amount)
    {
        currentXP += amount;
        
        // Check for Level Up
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }

        // Update UI
        if (xpBar) xpBar.value = currentXP;
    }

    void LevelUp()
    {
        currentXP -= xpToNextLevel;
        currentLevel++;
        
        // Make next level harder
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpDifficultyMultiplier);
        
        // Reset XP Bar Scaling for new level
        if (xpBar) xpBar.maxValue = xpToNextLevel;

        UpdateLevelText();
        Heal(maxHealth); // Full heal on level up!
        
        Debug.Log("Level Up! Open Card Menu Here.");
        // TODO: Call the 'LevelSystem' to open the card menu
    }

    void UpdateLevelText()
    {
        if (levelText) levelText.text = "Lvl " + currentLevel;
    }
}