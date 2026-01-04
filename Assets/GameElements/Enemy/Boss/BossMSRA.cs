using UnityEngine;

public class BossMRSA : MonoBehaviour
{
    public int shieldHitsRequired = 3;
    public int maxHealth = 200;

    int currentShieldHits;
    int currentHealth;
    bool shieldBroken;

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void InitializeBoss()
    {
        shieldBroken = false;
        currentShieldHits = shieldHitsRequired;
        currentHealth = maxHealth;

        if (anim)
            anim.Play("Shielded");
    }

    public void OnDashHit()
    {
        if (shieldBroken)
            return;

        currentShieldHits--;

        if (currentShieldHits <= 0)
            BreakShield();
        
        if (CombatLog.Instance)
         CombatLog.Instance.Log("Shield hit (" + currentShieldHits + " left)");

    }

    void BreakShield()
    {
        shieldBroken = true;

        if (anim)
            anim.Play("Broken");

        transform.localScale *= 0.8f;
        if (CombatLog.Instance)
        CombatLog.Instance.Log("Shield broken!");

    }

    public void TakeDamage(int damage)
    {
        if (!shieldBroken)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        GameManager.Instance.SetState(GameManager.GameState.Victory);
        Destroy(gameObject);
    }
}
