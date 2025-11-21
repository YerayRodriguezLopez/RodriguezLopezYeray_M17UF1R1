using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CanonBehaviour : MonoBehaviour
{
    //Detects player with a Raycast2D and shoots them
    public float detectionRange = 5f;
    private Stopwatch Stopwatch = new Stopwatch();
    public Transform shootPoint;
    public GameObject bullet;
    public Stack<GameObject> bulletsPool = new Stack<GameObject>();
    private void Start()
    {
        Stopwatch.Start();
    }
    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, detectionRange);
        UnityEngine.Debug.DrawRay(transform.position, transform.right * detectionRange, Color.red);
        if (hit.collider != null && hit.collider.gameObject.layer == 6 && Stopwatch.ElapsedMilliseconds >= 500)
        {
            UnityEngine.Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
            //Debug.Log("Player detected by canon!");
            Stopwatch.Restart();
            Shoot();
            UnityEngine.Debug.Log("Canon shot a bullet!");
        }
    }
    private void Shoot()
    {
        BulletBehaviour bb;
        if(bulletsPool.Count > 0)
        {
            bb = bulletsPool.Pop().GetComponent<BulletBehaviour>();
            bb.transform.position = shootPoint.position;
            bb.gameObject.SetActive(true);
        }
        else
        {
            bb = Instantiate(bullet, shootPoint.position, Quaternion.identity).GetComponent<BulletBehaviour>();
        }
    }
    public void ReturnBulletToPool(GameObject bullet)
    {
        bulletsPool.Push(bullet);
        bullet.SetActive(false);
    }
}
