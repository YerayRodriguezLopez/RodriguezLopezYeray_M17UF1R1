using System;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public static event Action OnPlayerHurt;
    public float bulletSpeed = 10f;
    private Vector2 direction;
    private Rigidbody2D rb;
    private CanonBehaviour sourceCanon;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 shootDirection, CanonBehaviour canon)
    {
        direction = shootDirection.normalized;
        sourceCanon = canon;
        rb.linearVelocity = direction * bulletSpeed;
        UnityEngine.Debug.Log("Bullet fired with direction: " + direction + " and speed: " + bulletSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnPlayerHurt?.Invoke();
            gameObject.SetActive(false);
            UnityEngine.Debug.Log("Bullet hit the player!");
        }
        else
        {
            gameObject.SetActive(false);
            UnityEngine.Debug.Log("Bullet hit an obstacle!");
        }
    }

    private void OnDisable()
    {
        if (sourceCanon != null)
        {
            sourceCanon.ReturnBulletToPool(gameObject);
        }
    }
}