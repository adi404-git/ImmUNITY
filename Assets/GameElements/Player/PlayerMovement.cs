using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerMovement : MonoBehaviour
{
    [Header("Cell Settings")]
    public float moveSpeed = 1500f; // Higher because we use Force + Time.fixedDeltaTime
    public float maxSpeed = 5f;     // Speed limit cap
    
    [Header("Blood Flow Settings")]
    public float flowForce = 300f;  // The current pushing you Left

    // Variables to hold data
    private Rigidbody2D rb;
    private Camera cam;
    private Vector2 moveInput;      // Stores WASD direction
    private Vector2 mousePos;       // Stores Mouse position

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    // 2. NEW INPUT METHOD: This triggers automatically when you press WASD
    // (Requires PlayerInput component set to "Send Messages")
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        // 3. READ MOUSE (New System Way)
        // We do this in Update because it's visual aiming, not physics
        if (Mouse.current != null) // Safety check
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            mousePos = cam.ScreenToWorldPoint(screenPos);
        }
    }

    void FixedUpdate()
    {
        // 4. PHYSICS: Apply Swim Force
        // Multiply by fixedDeltaTime for smooth framerate-independent physics
        rb.AddForce(moveInput * moveSpeed * Time.fixedDeltaTime);

        // 5. PHYSICS: Apply Blood Flow (Constant Wind)
        rb.AddForce(Vector2.left * flowForce * Time.fixedDeltaTime);

        // 6. SPEED LIMITER
        // If we are swimming faster than the limit...
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            // ...Clamp (cut off) the speed to the max
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }

        // 7. ROTATION (Face the Mouse)
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 21 ; 
        //I added 21 here because of the offset from center in the image I used, as Gemini did not generate it exactly right to the centre. 
        rb.rotation = angle;
    }
}