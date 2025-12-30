using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    public int dashDamage = 30;
    public float dashForce = 35f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    public bool isDashBuffActive = false;

    public GameObject antibodyPrefab;
    public Transform firePoint;
    public float fireRate = 1f;

    public bool isDashing { get; private set; }

    Rigidbody2D rb;
    PlayerMovement movement;
    PlayerStats stats;

    bool canDash = true;
    bool canShoot = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (Mouse.current.leftButton.isPressed && !isDashing)
        {
            TryShoot();
        }
    }

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

        if (movement)
            movement.enabled = false;

        Vector2 dashDir = transform.right;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dashDir * dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        if (movement)
            movement.enabled = true;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void TryShoot()
    {
        if (!canShoot || antibodyPrefab == null || firePoint == null)
            return;

        StartCoroutine(ShootCooldown());

        Vector2 dir = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - firePoint.position;
        dir.Normalize();

        GameObject bullet = Instantiate(antibodyPrefab, firePoint.position, Quaternion.identity);
        AntibodyProjectile proj = bullet.GetComponent<AntibodyProjectile>();

        if (proj)
            proj.Init(dir);
    }

    IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(1f / fireRate);
        canShoot = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy"))
            return;

        EnemyHealthAndXP enemy = collision.gameObject.GetComponent<EnemyHealthAndXP>();
        if (enemy == null)
            return;

        if (isDashing)
        {
            enemy.TakeDamage(dashDamage);
            BounceBack(collision.transform.position, 10f);
        }
        else
        {
            stats.TakeDamage(enemy.damageToPlayer);
            BounceBack(collision.transform.position, 5f);
        }
    }

    void BounceBack(Vector3 source, float force)
    {
        Vector2 dir = (transform.position - source).normalized;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }
}
