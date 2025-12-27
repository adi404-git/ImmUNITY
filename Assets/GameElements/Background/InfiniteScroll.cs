using UnityEngine;

public class InfiniteScroll : MonoBehaviour
{
    [Header("Settings")]
    public float scrollSpeed = 2f;
    public int totalImages = 3; // We are using 3 images to cover wide screens

    private float imageWidth;

    void Start()
    {
        // 1. Measure the image automatically
        // If your PPU is 100, a 2048px image will have a width of 20.48 units
        imageWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // 2. Move to the Left
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

        // 3. The Reset Check
        // If the object has moved completely off-screen to the left...
        // (We check against the image width. If position < -width, it's gone)
        if (transform.position.x < -imageWidth)
        {
            // 4. Teleport to the back of the line
            // We calculate how far we need to jump: Width * Total Number of Images
            Vector3 jumpVector = Vector3.right * (imageWidth * totalImages);
            
            // Add that jump to the current position to snap it perfectly to the end
            transform.position += jumpVector;
        }
    }
}