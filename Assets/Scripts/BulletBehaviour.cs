using System;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public static event Action OnPlayerHurt;
    public float bulletSpeed = 10f;
    public CanonBehaviour canonBehaviour;
    [SerializeField] private Rigidbody2D rb;

    private void Awake()
    {
        UnityEngine.Debug.Log("Bullet Awake called.");
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(bulletSpeed, 0);
        UnityEngine.Debug.Log("Bullet fired with speed: " + bulletSpeed);
    }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(bulletSpeed, 0);
        UnityEngine.Debug.Log("Bullet fired with speed: " + bulletSpeed);
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
        canonBehaviour.ReturnBulletToPool(gameObject);
    }
}
