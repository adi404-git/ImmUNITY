using UnityEngine;

public class AntibodyProjectile : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 10;
    public float lifetime = 3f;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    // Called immediately after Instantiate
    public void Init(Vector2 direction)
    {
        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameManager.Instance.IsPlaying())
            return;

        if (other.CompareTag("Enemy"))
        {
            EnemyHealthAndXP enemy = other.GetComponent<EnemyHealthAndXP>();
            if (enemy != null)
                enemy.TakeDamage(damage);

            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
