using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    [Header("Dash Settings")]
    public int dashDamage = 30;
    public float dashForce = 35f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Dash Buff")]
    public bool isDashBuffActive = false;
    private float originalDashForce;

    [Header("Shooting Settings")]
    public bool canShoot = false;                 // Unlocked at Level 4
    public GameObject antibodyPrefab;
    public Transform firePoint;
    public float fireRate = 1f;                   // 1 shot / second

    // State
    public bool isDashing { get; private set; }
    private bool canDash = true;
    private float nextFireTime = 0f;

    // References
    private Rigidbody2D rb;
    private PlayerStats myStats;
    private PlayerMovement movement;

    void Awake()
    {
        originalDashForce = dashForce;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myStats = GetComponent<PlayerStats>();
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        HandleShooting();
    }

    // ================= DASH =================
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

        if (movement) movement.enabled = false;

        Vector2 dashDir = transform.right;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dashDir * dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        if (movement) movement.enabled = true;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // ================= SHOOTING =================
    void HandleShooting()
    {
        if (!canShoot) return;
        if (isDashing) return;

        if (Mouse.current.leftButton.isPressed && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (antibodyPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(
            antibodyPrefab,
            firePoint.position,
            firePoint.rotation
        );

        AntibodyProjectile proj = bullet.GetComponent<AntibodyProjectile>();
        if (proj != null)
        {
            proj.Init(firePoint.right);
        }
    }

    // ================= BUFF API (USED BY GLUCOSE) =================
    public void ApplyDashBuff(float multiplier, float duration)
    {
        if (isDashBuffActive) return;

        StartCoroutine(DashBuffRoutine(multiplier, duration));
    }

    IEnumerator DashBuffRoutine(float multiplier, float duration)
    {
        isDashBuffActive = true;
        dashForce *= multiplier;

        yield return new WaitForSeconds(duration);

        dashForce = originalDashForce;
        isDashBuffActive = false;
    }

    // ================= COLLISION =================
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        EnemyHealthAndXP enemy = collision.gameObject.GetComponent<EnemyHealthAndXP>();
        if (enemy == null) return;

        if (isDashing)
        {
            enemy.TakeDamage(dashDamage);
            BounceBack(collision.transform.position, 10f);
        }
        else
        {
            myStats.TakeDamage(enemy.damageToPlayer);
            BounceBack(collision.transform.position, 5f);
        }
    }

    void BounceBack(Vector3 hazardPos, float force)
    {
        Vector2 dir = (transform.position - hazardPos).normalized;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }
}
