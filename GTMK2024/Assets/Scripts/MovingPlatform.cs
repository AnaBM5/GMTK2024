using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveSpeed = 2f;       // Speed of platform movement
    public float moveDistance = 5f;    // Distance the platform will move from its start position
    public float playerMassThreshold = 1.5f; // Mass threshold for breaking the platform
    public float respawnDelay = 3f;    // Time in seconds before respawning the platform

    private float startX;
    private bool movingRight = true;
    private bool isBroken = false;
    private Vector3 respawnPosition;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider; // Reference to the BoxCollider2D

    void Start()
    {
        startX = transform.position.x;
        respawnPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>(); // Get the BoxCollider2D component

        // Freeze the platform in the Y-axis but allow movement in the X-axis
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
    }

    void Update()
    {
        if (isBroken)
        {
            return; // Platform is broken, skip movement
        }

        MovePlatform();
    }

    private void MovePlatform()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            if (transform.position.x >= startX + moveDistance)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            if (transform.position.x <= startX)
            {
                movingRight = true;
            }
        }
    }

     void OnCollisionStay2D(Collision2D collision)
    {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Debug.Log("Player collided with platform. Player mass: " + playerRb.mass);
                if (playerRb.mass >= playerMassThreshold)
                {
                    Debug.Log("Player mass exceeds threshold. Breaking platform.");
                    BreakPlatform();
                }
                else
                {
                    Debug.Log("Player mass does not exceed threshold.");
                }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = transform;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Remove the player from being a child of the platform
            collision.transform.parent = null;
        }
    }

    private void BreakPlatform()
    {
        isBroken = true;
        rb.velocity = Vector2.zero; // Stop platform movement
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Unfreeze Y-axis to allow falling

        // Disable the BoxCollider2D to prevent further interactions
        boxCollider.enabled = false;

        Invoke("RespawnPlatform", respawnDelay);
    }

    private void RespawnPlatform()
    {
        // Find all player objects that were children of the platform and detach them
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Player"))
            {
                child.parent = null;
            }
        }
        isBroken = false;

        // Reset the platform's velocity
        rb.velocity = Vector2.zero;

        // Reapply Y-axis constraint to stop falling
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

        // Reset the platform's position
        transform.position = respawnPosition;

        // Re-enable the BoxCollider2D for future interactions
        boxCollider.enabled = true;


        movingRight = true;
    }
}
