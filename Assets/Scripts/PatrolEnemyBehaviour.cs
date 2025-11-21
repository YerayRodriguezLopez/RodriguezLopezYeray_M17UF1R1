using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PatrolEnemyBehaviour : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float patrolSpeed = 2f;
    public float patrolDistance = 5f;
    private Vector2 startPosition;
    private bool movingRight = true;

    [Header("Chase Settings")]
    public float chaseSpeed = 4f;
    public float detectionRange = 8f;
    public LayerMask playerLayer;

    [Header("Platform Detection")]
    public Transform groundCheck;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundLayer;

    [Header("Obstacle Detection")]
    public float obstacleCheckDistance = 0.3f; // Reduced from 1f to walk closer
    public LayerMask obstacleLayer;

    [Header("Head Collision")]
    public Transform headCheck;
    public float headCheckRadius = 0.3f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isChasing = false;
    private Transform player;
    private Rigidbody2D playerRb;
    private bool isDead = false;
    private float flipCooldown = 0f;
    private const float FLIP_COOLDOWN_TIME = 0.5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;

        // Find the player in the scene
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerRb = playerObj.GetComponent<Rigidbody2D>();
        }
    }

    void OnEnable()
    {
        // Subscribe to hurt events to detect player respawn
        HurtBehaviourScript.OnPlayerHurt += OnPlayerRespawn;
        BulletBehaviour.OnPlayerHurt += OnPlayerRespawn;
    }

    void OnDisable()
    {
        // Unsubscribe from events
        HurtBehaviourScript.OnPlayerHurt -= OnPlayerRespawn;
        BulletBehaviour.OnPlayerHurt -= OnPlayerRespawn;
    }

    void OnPlayerRespawn()
    {
        // Reset enemy to starting position and state
        if (isDead)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        // Reset position
        transform.position = startPosition;

        // Reset state
        isDead = false;
        isChasing = false;
        movingRight = true;

        // Reset animator
        animator.SetBool("Dead", false);
        animator.SetBool("Chasing", false);
        animator.SetBool("Moving", false);

        // Re-enable the script and rigidbody
        rb.linearVelocity = Vector2.zero;
        this.enabled = true;

        // Make visible again
        gameObject.SetActive(true);
    }

    void Update()
    {
        // Check if player landed on head
        CheckHeadStomp();

        // Check if player is in detection range
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                if (!isChasing)
                {
                    isChasing = true;
                    animator.SetBool("Chasing", true);
                }
            }
            else if (distanceToPlayer > detectionRange * 1.5f) // Stop chasing if player gets far enough
            {
                if (isChasing)
                {
                    isChasing = false;
                    animator.SetBool("Chasing", false);
                }
            }
        }

        if (isChasing && player != null)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        // Update moving animation based on velocity
        bool isMoving = Mathf.Abs(rb.linearVelocityX) > 0.1f;
        animator.SetBool("Moving", isMoving);
    }

    void CheckHeadStomp()
    {
        if (player == null || playerRb == null) return;

        Vector2 checkPosition = headCheck != null ? headCheck.position : (Vector2)transform.position + Vector2.up * 0.5f;

        // Check if player is above and moving downward
        Collider2D hit = Physics2D.OverlapCircle(checkPosition, headCheckRadius, playerLayer);

        if (hit != null && playerRb.linearVelocityY < 0)
        {
            // Player stomped on enemy
            Die();

            // Bounce player upward
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocityX, 0);
            playerRb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        }
    }

    void Die()
    {
        UnityEngine.Debug.Log("Enemy died!");
        isDead = true;
        animator.SetBool("Dead", true);
        // Disable movement
        rb.linearVelocity = Vector2.zero;
        this.enabled = false;

        // Hide the enemy instead of destroying it
        Invoke("HideEnemy", 0.5f);
    }

    void HideEnemy()
    {
        gameObject.SetActive(false);
    }

    void Patrol()
    {
        // Check if there's ground ahead
        bool groundAhead = CheckGroundAhead();

        // Check for obstacles ahead
        bool obstacleAhead = CheckObstacleAhead();

        // Check if reached patrol limit
        bool reachedLimit = ReachedPatrolLimit();

        // If no ground ahead, reached patrol limit, or obstacle ahead, turn around
        if (!groundAhead || reachedLimit || obstacleAhead)
        {
            // Only flip if we're actually moving in that direction
            // This prevents flip loops
            float currentDirection = movingRight ? 1f : -1f;
            if (Mathf.Sign(rb.linearVelocityX) == currentDirection || Mathf.Abs(rb.linearVelocityX) < 0.01f)
            {
                movingRight = !movingRight;
                Flip();
            }
        }

        // Move in patrol direction
        float direction = movingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * patrolSpeed, rb.linearVelocityY);
    }

    void ChasePlayer()
    {
        // Check if there's ground ahead before chasing
        bool groundAhead = CheckGroundAhead();

        // Check for obstacles ahead
        bool obstacleAhead = CheckObstacleAhead();

        if (!groundAhead || obstacleAhead)
        {
            // Stop at edge or obstacle, don't chase
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);

            // If obstacle ahead during chase, turn back to patrol
            if (obstacleAhead)
            {
                isChasing = false;
                movingRight = !movingRight;
                Flip();
            }
            return;
        }

        // Calculate direction to player
        float directionToPlayer = Mathf.Sign(player.position.x - transform.position.x);

        // Move towards player
        rb.linearVelocity = new Vector2(directionToPlayer * chaseSpeed, rb.linearVelocityY);

        // Face the player
        if ((directionToPlayer > 0 && !movingRight) || (directionToPlayer < 0 && movingRight))
        {
            movingRight = !movingRight;
            Flip();
        }
    }

    bool CheckGroundAhead()
    {
        // Cast a ray downward from slightly ahead of the enemy
        Vector2 checkPosition = groundCheck != null ? groundCheck.position : transform.position;
        float checkDistance = movingRight ? 0.5f : -0.5f;
        Vector2 rayStart = new Vector2(checkPosition.x + checkDistance, checkPosition.y);

        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, groundCheckDistance, groundLayer);

        // Debug visualization
        Debug.DrawRay(rayStart, Vector2.down * groundCheckDistance, hit.collider != null ? Color.green : Color.red);

        return hit.collider != null;
    }

    bool CheckObstacleAhead()
    {
        // Cast a ray forward from the edge of the enemy to detect obstacles
        // Start the raycast from further forward (at the enemy's edge)
        float enemyHalfWidth = 0.5f; // Adjust based on your enemy's actual width
        Vector2 rayStart = new Vector2(
            transform.position.x + (movingRight ? enemyHalfWidth : -enemyHalfWidth),
            transform.position.y
        );

        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(rayStart, direction, obstacleCheckDistance, obstacleLayer);

        // Also check for HurtBehaviour component
        if (hit.collider != null)
        {
            HurtBehaviourScript hurtScript = hit.collider.GetComponent<HurtBehaviourScript>();
            if (hurtScript != null)
            {
                Debug.DrawRay(rayStart, direction * obstacleCheckDistance, Color.magenta);
                return true;
            }
        }

        Debug.DrawRay(rayStart, direction * obstacleCheckDistance, hit.collider != null ? Color.yellow : Color.blue);

        return hit.collider != null;
    }

    bool ReachedPatrolLimit()
    {
        float distanceFromStart = transform.position.x - startPosition.x;

        // Check if moving right and reached right limit
        if (movingRight && distanceFromStart >= patrolDistance)
        {
            return true;
        }
        // Check if moving left and reached left limit
        else if (!movingRight && distanceFromStart <= -patrolDistance)
        {
            return true;
        }

        return false;
    }

    void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void OnDrawGizmosSelected()
    {
        // Draw patrol range
        Vector2 pos = Application.isPlaying ? startPosition : (Vector2)transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector2(pos.x - patrolDistance, pos.y), new Vector2(pos.x + patrolDistance, pos.y));

        // Draw detection range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw head check area
        Vector2 headPos = headCheck != null ? headCheck.position : (Vector2)transform.position + Vector2.up * 0.5f;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(headPos, headCheckRadius);
    }
}