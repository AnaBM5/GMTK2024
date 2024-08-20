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

    void Start()
    {
        startX = transform.position.x;
        respawnPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();

        // Freeze the platform in the Y-axis but allow movement in the X-axis
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
    }

    void Update()
    {
        if (isBroken)
        {
            return; // Platform is broken, skip movement
        }

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
            else
            {
                Debug.Log("No Rigidbody2D found on player.");
            }
        }
    }

    private void BreakPlatform()
    {
        isBroken = true;
        rb.velocity = Vector2.zero; // Stop platform movement

        // Remove Y-axis constraint to allow falling
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        Invoke("RespawnPlatform", respawnDelay);
    }

    private void RespawnPlatform()
    {
        isBroken = false;

        // Reapply Y-axis constraint to stop falling
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

        transform.position = respawnPosition;
    }
}
