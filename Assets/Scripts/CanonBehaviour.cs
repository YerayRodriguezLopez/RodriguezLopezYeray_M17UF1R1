using UnityEngine;
using System.Collections.Generic;

public class CanonBehaviour : MonoBehaviour
{
    // Create a stack of GameObjects to hold the projectiles
    private Stack<GameObject> bulletStack;
    // Create a public GameObject variable to hold the projectile prefab
    public GameObject Bullet;
    // Create a raycast hit variable
    private RaycastHit2D hit;
    private float shootDelay = 0.2f;
    private float speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        bulletStack = new Stack<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // Shoot a projectile 0.2 seconds after the player is detected
        hit = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity, LayerMask.GetMask("Player"));
        Debug.DrawRay(transform.position, Vector2.left * 10, Color.red);
        if (hit.collider != null)
        {
            shootDelay -= Time.deltaTime;
            if (shootDelay <= 0f)
            {
                Shoot();
                shootDelay = 0.2f;
            }
        }
    }

    private void Shoot()
    {
        GameObject bullet;
        if (bulletStack.Count > 0)
        {
            bullet = bulletStack.Pop();
            bullet.SetActive(true);
            bullet.transform.position = transform.position;
        }
        else
        {
            bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            bullet.AddComponent<BulletBehaviour>().canon = this;
        }
        bullet.GetComponent<Rigidbody2D>().linearVelocity = Vector2.left * speed;
    }
}
