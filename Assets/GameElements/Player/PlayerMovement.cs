using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerMovement : MonoBehaviour
{
   
    public float moveSpeed = 1500f; 
    public float maxSpeed = 5f;    
    
   
    public float flowForce = 300f;  

    
    private Rigidbody2D rb;
    private Camera cam;
    private Vector2 moveInput;     
    private Vector2 mousePos;     

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
       
        if (Mouse.current != null) 
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            mousePos = cam.ScreenToWorldPoint(screenPos);
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.IsPlaying())
         return;
       
        rb.AddForce(moveInput * moveSpeed * Time.fixedDeltaTime);

       
        rb.AddForce(Vector2.left * flowForce * Time.fixedDeltaTime);

       
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
           
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }

      
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 21 ; 
        
        rb.rotation = angle;
    }
}