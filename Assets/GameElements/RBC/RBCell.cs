using UnityEngine;

public class RBCell : MonoBehaviour
{
    public float flowSpeed = 5f;
    public float destroyXPosition = -11f; // When to delete (Left side of screen)

    void Update()
    {
        // 1. Move constantly to the Left (The Current)
        transform.Translate(Vector3.left * flowSpeed * Time.deltaTime);

        // 2. Cleanup: If it goes too far left, destroy it
        if (transform.position.x < destroyXPosition)
        {
            Destroy(gameObject);
        }
    }
}