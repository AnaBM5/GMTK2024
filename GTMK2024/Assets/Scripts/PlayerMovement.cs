using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseMoveSpeed = 5f;
    public float baseJumpForce = 10f;
    public float scaleChangeSpeed = 0.1f; // How much the scale changes per click
    //public float currentCameraSize = 5f; // Base size for the camera
    public CameraController playerCamera; // Reference to the player's camera
    public Transform groundCheck;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;
    // public Transform wallCheck; // 
    // public float wallCheckDistance = 0.1f; // 
    // public LayerMask wallLayer; // 
    public Transform headCheck; // Used to check if there's something above the player
    public float headCheckDistance = 0.1f; // Distance to check above the player's head
    public LayerMask obstacleLayer; // Layer for obstacles above the player

    [Header("Change values:")]
    [SerializeField] private float cameraScaleChange = 0.8f;
    [SerializeField] private float gravityScaleChange = 0.5f;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    // private bool isTouchingWall; // 
    private Vector3 initialScale;
    private float currentMass;
    private float currentGravityScale;
    private float currentMoveSpeed;
    private float currentJumpForce;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale;
        currentMass = rb.mass;
        currentGravityScale = rb.gravityScale;

        // Initialize current speed and force
        currentMoveSpeed = baseMoveSpeed;
        currentJumpForce = baseJumpForce;

        // Set the initial camera size
        //playerCamera.orthographicSize = currentCameraSize;
    }

    void Update()
    {
        Move();
        CheckIfGrounded();
        // CheckIfTouchingWall(); // 
        Jump();
        HandleScaleChange();
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        
        //if(!CheckIfTouchingWall(moveInput))
        rb.velocity = new Vector2(moveInput * currentMoveSpeed, rb.velocity.y);
    }

    void CheckIfGrounded()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        /*Prevent ground detection when touching a wall
         if (isTouchingWall)
         {
             isGrounded = false;
        }*/
    }

    bool CheckIfTouchingWall(float moveInput)
    {
        Vector2 directionCheck;
         if (moveInput > 0f)
             directionCheck = Vector2.right;
         else if( moveInput < 0f)
             directionCheck = Vector2.left;
         else
             return true;
         
         return Physics2D.Raycast(groundCheck.position, directionCheck, groundCheckDistance, groundLayer);
     }

    void Jump()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
        }
    }

    void HandleScaleChange()
    {
        if (Input.GetMouseButtonDown(1)) // Right mouse button makes player bigger
        {
            if (ChangeScale(scaleChangeSpeed))
                ChangeProperties(1);
        }
        else if (Input.GetMouseButtonDown(0)) // Left mouse button makes player smaller
        {
            if(ChangeScale(-scaleChangeSpeed))
                ChangeProperties(-1);
        }
    }

    bool ChangeScale(float scaleDelta)
    {
        // Check if there's something above the player before increasing the scale
        bool canGrow = !Physics2D.Raycast(headCheck.position, Vector2.up, headCheckDistance, obstacleLayer);

        Vector3 localScale = transform.localScale;
        
        // Only increase scale if there's nothing above the player or new scale is too small 
        if (scaleDelta > 0 && !canGrow
            ||  scaleDelta < 0 && localScale.x<= 0.1f)
        {
            return false; // Prevents from affecting other elements
        }

        // Update the scale
        Vector3 newScale = localScale + new Vector3(scaleDelta, scaleDelta, 0);


        // Ensure the scale does not go below a certain threshold
        newScale = new Vector3(
            Mathf.Max(newScale.x, initialScale.x * 0.1f),
            Mathf.Max(newScale.y, initialScale.y * 0.1f),
            newScale.z
        );
        transform.localScale = newScale;
        
        return true;
    }

    private void ChangeProperties(int sizeMultiplier)
    {
        // Update the mass based on the new scale
        currentMass+= gravityScaleChange* sizeMultiplier; // Scale the mass based on the area
        rb.mass = currentMass;

        // Adjust gravity based on the new scale
        currentGravityScale += gravityScaleChange* sizeMultiplier;
        rb.gravityScale = currentGravityScale;

        // Adjust the jump force only when the player is larger
        currentJumpForce -= 2f* sizeMultiplier;

        // Adjust the camera size proportionally to the player's size
        playerCamera.ChangeCameraScale(sizeMultiplier);
        //playerCamera.orthographicSize = currentCameraSize + cameraScaleChange * sizeMultiplier;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        // Gizmos.color = Color.blue; // 
        // Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * wallCheckDistance); // 
        Gizmos.color = Color.green;
        Gizmos.DrawLine(headCheck.position, headCheck.position + Vector3.up * headCheckDistance);
    }
}
