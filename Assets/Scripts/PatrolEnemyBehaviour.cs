using UnityEngine;
using System;

public class PatrolEnemyBehaviour : MonoBehaviour
{
    [Header("Movement Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Settings")]
    [SerializeField] private float reachDistance = 0.8f;

    private Rigidbody2D rb;
    private MoveBehaviour mb;
    private SpriteRenderer spriteRenderer;
    private Transform targetPoint;
    private bool movingToB = true;
    public static event Action OnPlayerHurt;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mb = GetComponent<MoveBehaviour>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPoint = pointB;

    }
    public void Update()
    {
        Patrol();
    }
    private void Patrol()
    {
        if (Vector2.Distance(transform.position, targetPoint.position) <= reachDistance)
        {
            movingToB = !movingToB;
            targetPoint = movingToB ? pointB : pointA;
        }
        Vector2 direction = (targetPoint.position - transform.position).normalized;
        mb.Move(direction);
        // Flip sprite based on movement direction
        if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            OnPlayerHurt?.Invoke();
    }
}