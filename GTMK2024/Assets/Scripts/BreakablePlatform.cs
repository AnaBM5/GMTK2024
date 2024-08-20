using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    public float weightThreshold = 1.5f; // Minimum mass required to break the platform
    public float fallSpeedThreshold = 5f; // Minimum fall speed required to break the platform
    public float respawnDelay = 3f; // Time before the platform respawns

    private Rigidbody2D rb;
    private Vector3 respawnPosition;
    private bool isBroken = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        respawnPosition = transform.position;
        
        // Ensure the platform has constraints if necessary (e.g., if you want to freeze rotation)
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        rb.gravityScale = 0; // Ensure gravity is initially off

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBroken)
        {
            return; // Platform is broken, ignore further collisions
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                float playerMass = playerRb.mass;
                float playerFallSpeed = Mathf.Abs(playerRb.velocity.y);

                if (playerMass >= weightThreshold && playerFallSpeed >= fallSpeedThreshold)
                {
                    Debug.Log("Platform breaking due to weight and speed.");
                    BreakPlatform();
                }
                else
                {
                    Debug.Log("Platform not breaking. Mass or fall speed not sufficient.");
                }
            }
            else
            {
                
            }
        }
    }

    private void BreakPlatform()
    {
        isBroken = true;

        // Disable the collider to prevent further collisions
        GetComponent<Collider2D>().enabled = false;
        
        // Remove platform's Rigidbody2D constraints to allow falling
        rb.constraints = RigidbodyConstraints2D.None;

        // Apply gravity to let the platform fall
        rb.gravityScale = 1;

        // Respawn the platform after a delay
        Invoke("RespawnPlatform", respawnDelay);
    }

    private void RespawnPlatform()
    {
        isBroken = false;

        // Re-enable the collider
        GetComponent<Collider2D>().enabled = true;

        // Reset the platform's position and constraints
        transform.position = respawnPosition;
        rb.velocity = Vector2.zero; // Stop any ongoing motion
        rb.gravityScale = 0; // Reset gravity scale if necessary
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;; // Apply constraints if needed
    }
}
