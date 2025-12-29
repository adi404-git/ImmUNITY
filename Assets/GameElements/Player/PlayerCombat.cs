using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Settings")]
    public int dashDamage = 30; // 4 hits to kill a 100 HP enemy
    public float dashForce = 35f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public bool isDashBuffActive = false;

    // State
    public bool isDashing { get; private set; }
    private bool canDash = true;

    // References
    private Rigidbody2D rb;
    private PlayerStats myStats; // Link to our new Health script
    private PlayerMovement movement; // Link to movement to pause it while dashing

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myStats = GetComponent<PlayerStats>();
        movement = GetComponent<PlayerMovement>();
    }

    // --- INPUT (Spacebar) ---
    void OnDash(InputValue value)
    {
        if (value.isPressed && canDash)
        {
            StartCoroutine(PerformDash());
        }
    }

    IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;
        
        // Optional: Disable normal movement for a moment
        if (movement) movement.enabled = false;

        // Dash towards Mouse (Aim)
        // Vector2 dashDir = (Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position).normalized;
        // OR Dash Forward (Sprite Direction)
        Vector2 dashDir = transform.right; 

        rb.linearVelocity = Vector2.zero; // Reset drift
        rb.AddForce(dashDir * dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(dashDuration);

        if (movement) movement.enabled = true;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // --- MASTER COLLISION LOGIC ---
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Is it an Enemy?
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealthAndXP enemy = collision.gameObject.GetComponent<EnemyHealthAndXP>();

            if (isDashing)
            {
                // ATTACK: We hit them while dashing -> They take damage
                if (enemy != null)
                {
                    enemy.TakeDamage(dashDamage);
                    
                    // Bounce off slightly
                    BounceBack(collision.transform.position, 10f);
                }
            }
            else
            {
                // OUCH: We bumped into them normally -> We take damage
                if (enemy != null && myStats != null)
                {
                    myStats.TakeDamage(enemy.damageToPlayer);
                    
                    // Get knocked back
                    BounceBack(collision.transform.position, 5f);
                }
            }
        }
    }

    void BounceBack(Vector3 hazardPos, float force)
    {
        Vector2 dir = (transform.position - hazardPos).normalized;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }
}