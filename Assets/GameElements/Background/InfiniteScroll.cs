using UnityEngine;

public class InfiniteScroll : MonoBehaviour
{
    [Header("Settings")]
    public float scrollSpeed = 5f;

    private float spriteWidth;

    void Start()
    {
        // 1. Auto-measure the width of the sprite so you don't have to guess
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
    }

    void Update()
    {
        // 2. Move the object to the left
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

        // 3. Check if we have gone completely off-screen to the left
        // We compare position to negative width (meaning it's fully past the left edge)
        if (transform.position.x < -spriteWidth)
        {
            // 4. Teleport to the right!
            // We add (2 * width) because we have 2 backgrounds. 
            // This snaps it exactly behind the second background.
            Vector3 newPos = transform.position;
            newPos.x += 2f * spriteWidth; 
            transform.position = newPos;
        }
    }
}