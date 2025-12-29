using UnityEngine;

public class ProteinPickup : MonoBehaviour
{
    public float flowSpeed = 2f;
    public float rotationSpeed = 90f;

    [Range(0f, 1f)]
    public float healPercent = 0.2f;

    public float destroyX = -15f;

    void Update()
    {
        transform.Translate(Vector2.left * flowSpeed * Time.deltaTime, Space.World);
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerStats stats = other.GetComponent<PlayerStats>();
        if (stats != null)
        {
            float healAmount = stats.maxHealth * healPercent;
            stats.currentHealth = Mathf.Min(stats.currentHealth + (int)healAmount, stats.maxHealth);

        }

        Destroy(gameObject);
    }
}
