using UnityEngine;

public class HurtBehaviourScript : MonoBehaviour
{
    private Rigidbody rb;
    private Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.Hurt();
        }
    }
}
