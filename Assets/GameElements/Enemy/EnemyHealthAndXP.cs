using UnityEngine;

public class EnemyHealthAndXP : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100; // Bacterium = 100, Rhinovirus = 30
    public int xpReward = 20;   // How much XP player gets
    public int damageToPlayer = 10; // How much it hurts if player bumps it

    private int currentHealth;
    private SpriteRenderer sprite;
    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;
        sprite = GetComponent<SpriteRenderer>();
        if (sprite) originalColor = sprite.color;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        // Visual Feedback (Flash White)
        StartCoroutine(FlashDamage());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 1. Give XP to Player
        PlayerStats player = FindObjectOfType<PlayerStats>();
        if (player != null)
        {
            player.GainXP(xpReward);
        }

        // 2. Destroy this enemy
        Debug.Log($"{gameObject.name} Died.");
        Destroy(gameObject);
    }

    // --- COLLISION LOGIC (If Player walks into us) ---
    void OnCollisionEnter2D(Collision2D collision)
    {
        // If we hit the Player, damage them (unless they are Dashing, handled in PlayerCombat)
        if (collision.gameObject.CompareTag("Player"))
        {
            // We let the PlayerCombat script handle the "Dash vs Hit" logic 
            // to avoid double-counting damage.
        }
    }

    System.Collections.IEnumerator FlashDamage()
    {
        if (sprite) sprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        if (sprite) sprite.color = originalColor;
    }
}