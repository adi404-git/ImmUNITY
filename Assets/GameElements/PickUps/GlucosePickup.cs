using UnityEngine;
using System.Collections;

public class GlucosePickup : MonoBehaviour
{
    public float flowSpeed = 2.5f;
    public float rotationSpeed = 120f;

    public float dashMultiplier = 1.4f;
    public float buffDuration = 6f;

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

        PlayerCombat combat = other.GetComponent<PlayerCombat>();
        if (combat != null)
        {
            StartCoroutine(ApplyDashBuff(combat));
        }

        Destroy(gameObject);
    }

    IEnumerator ApplyDashBuff(PlayerCombat combat)
{
    if (combat.isDashBuffActive)
        yield break;

    combat.isDashBuffActive = true;

    float originalDashForce = combat.dashForce;
    combat.dashForce *= dashMultiplier;

    yield return new WaitForSeconds(buffDuration);

    if (combat != null)
        combat.dashForce = originalDashForce;

    combat.isDashBuffActive = false;
}

}
