using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    [Header("1. Dash Attack Settings")]
    public float dashForce = 35f;       // Stronger impulse for ramming
    public float dashDuration = 0.2f;   // Short burst
    public float dashCooldown = 1.2f;
    
    // Public flag: Other scripts (like Enemy) can check this to see if they got hit!
    public bool isDashing { get; private set; } 
    private bool canDash = true;

    [Header("2. Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    // Components
    private Rigidbody2D rb;
    private TrailRenderer dashTrail;
    private PlayerMovement controller; // Ref to movement script

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashTrail = GetComponent<TrailRenderer>();
        controller = GetComponent<PlayerMovement>();
        
        currentHealth = maxHealth;
        if(dashTrail) dashTrail.emitting = false;
        isDashing = false;
    }

    // --- INPUT LISTENER ---
    // Make sure Input Actions has "Dash" mapped to Spacebar
    void OnDash(InputValue value)
    {
        if (value.isPressed && canDash)
        {
            StartCoroutine(PerformDashAttack());
        }
    }

    // --- THE DASH ATTACK ---
    IEnumerator PerformDashAttack()
    {
        canDash = false;
        isDashing = true;

        // 1. Disable Normal Movement (Optional, prevents fighting the dash)
        if (controller != null) controller.enabled = false;

        // 2. Visuals ON
        if(dashTrail) dashTrail.emitting = true;

        // 3. Apply Force (Ramming Speed)
        // We dash in the direction the MOUSE is looking (Aim Direction)
        // This is better for attacking than WASD direction.
        Vector2 aimDir = transform.right; 
        
        rb.linearVelocity = Vector2.zero; // Reset current drift
        rb.AddForce(aimDir * dashForce, ForceMode2D.Impulse);

        // 4. Wait
        yield return new WaitForSeconds(dashDuration);

        // 5. Cleanup
        if(dashTrail) dashTrail.emitting = false;
        isDashing = false;
        
        // Re-enable control
        if (controller != null) controller.enabled = true;

        // 6. Cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // --- HEALTH SYSTEM ---
    public void TakeDamage(int damage)
    {
        // Invincible while dashing? (Optional design choice)
        // if (isDashing) return; 

        currentHealth -= damage;
        Debug.Log("Health: " + currentHealth);
        if (currentHealth <= 0) gameObject.SetActive(false); // Die
    }
}