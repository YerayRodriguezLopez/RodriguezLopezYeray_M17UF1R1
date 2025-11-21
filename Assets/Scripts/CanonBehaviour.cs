using UnityEngine;

public class CanonBehaviour : MonoBehaviour
{
<<<<<<< HEAD
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
=======
    [Header("Detection Settings")]
    public float detectionRange = 10f;
    public LayerMask playerLayer;
    public Transform raycastOrigin;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float bulletSpeed = 20f;

    [Header("Debug")]
    public bool showDebugRay = true;

    private float nextFireTime = 0f;
    private Transform player;

    void Enable()
    {
        if (raycastOrigin == null)
            raycastOrigin = transform;

        if (firePoint == null)
            firePoint = transform;
    }

    void Update()
    {
        DetectAndShoot();
    }

    void DetectAndShoot()
    {
        Vector3 direction = raycastOrigin.forward;
        RaycastHit hit;

        // Perform raycast
        if (Physics.Raycast(raycastOrigin.position, direction, out hit, detectionRange, playerLayer))
        {
            // Check if we hit the player
            if (hit.collider.CompareTag("Player"))
            {
                player = hit.collider.transform;

                // Debug ray (green when player detected)
                if (showDebugRay)
                    Debug.DrawRay(raycastOrigin.position, direction * hit.distance, Color.green);

                // Shoot at player if cooldown is ready
                if (Time.time >= nextFireTime)
                {
                    ShootAtPlayer();
                    nextFireTime = Time.time + 1f / fireRate;
                }
            }
        }
        else
        {
            // Debug ray (red when no player detected)
            if (showDebugRay)
                Debug.DrawRay(raycastOrigin.position, direction * detectionRange, Color.red);

            player = null;
        }
    }

    void ShootAtPlayer()
    {
        if (bulletPrefab == null || player == null)
            return;

        // Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Calculate direction to player
        Vector3 shootDirection = (player.position - firePoint.position).normalized;

        // Add velocity to bullet
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = shootDirection * bulletSpeed;
        }

        // Destroy bullet after 5 seconds
        Destroy(bullet, 5f);
    }

    void OnDrawGizmosSelected()
    {
        if (raycastOrigin == null)
            raycastOrigin = transform;

        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(raycastOrigin.position, raycastOrigin.forward * detectionRange);
>>>>>>> origin/Dev
    }
}
