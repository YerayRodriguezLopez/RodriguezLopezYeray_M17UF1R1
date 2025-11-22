using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CanonBehaviour : MonoBehaviour
{
    //Detects player with a Raycast2D and shoots them
    public float detectionRange = 5f;
    public float detectionDelay = 0.5f; // Half second delay before shooting
    public float shootCooldown = 0.5f; // Cooldown between shots
    private Stopwatch detectionTimer = new Stopwatch();
    private Stopwatch shootTimer = new Stopwatch();
    private bool playerDetected = false;
    private bool canShoot = true;
    public Transform shootPoint;
    public GameObject bullet;
    public Stack<GameObject> bulletsPool = new Stack<GameObject>();

    private void Start()
    {
        detectionTimer.Start();
        shootTimer.Start();
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, detectionRange);
        //UnityEngine.Debug.DrawRay(transform.position, transform.right * detectionRange, Color.red);

        bool playerInSight = hit.collider != null && hit.collider.gameObject.layer == 6;

        // Start detection timer when player first spotted
        if (playerInSight && !playerDetected && canShoot)
        {
            playerDetected = true;
            detectionTimer.Restart();
            //UnityEngine.Debug.Log("Player detected! Preparing to shoot...");
        }

        // Once detection started, shoot after delay regardless of player position
        if (playerDetected && detectionTimer.ElapsedMilliseconds >= (detectionDelay * 1000))
        {
            Shoot();
            playerDetected = false;
            canShoot = false;
            shootTimer.Restart();
            //UnityEngine.Debug.Log("Canon shot a bullet!");
        }

        // Reset shoot cooldown
        if (!canShoot && shootTimer.ElapsedMilliseconds >= (shootCooldown * 1000))
        {
            canShoot = true;
        }
    }

    private void Shoot()
    {
        if (bullet == null)
        {
            //UnityEngine.Debug.LogError("Bullet prefab is not assigned to the cannon!");
            return;
        }

        BulletBehaviour bb;

        if (bulletsPool.Count > 0)
        {
            GameObject bulletObj = bulletsPool.Pop();
            bulletObj.transform.position = shootPoint.position;
            bulletObj.SetActive(true);
            bb = bulletObj.GetComponent<BulletBehaviour>();
        }
        else
        {
            GameObject bulletObj = Instantiate(bullet, shootPoint.position, Quaternion.identity);
            bb = bulletObj.GetComponent<BulletBehaviour>();
        }

        if (bb == null)
        {
            //UnityEngine.Debug.LogError("BulletBehaviour component is missing from the bullet prefab!");
            return;
        }

        // Initialize the bullet with the cannon's facing direction
        bb.Initialize(transform.right, this);
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletsPool.Push(bullet);
    }
}