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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player play))
        {
            //only if collided from top
            //if (other.transform.position.y > transform.position.y + 0.5f)
                play.Hurt();
            
        }
    }
}
