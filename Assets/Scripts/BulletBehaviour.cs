using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BulletBehaviour : MonoBehaviour
{
    public CanonBehaviour canon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // when the bullet hits something, it should be deactivated and returned to the canon's stack
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
