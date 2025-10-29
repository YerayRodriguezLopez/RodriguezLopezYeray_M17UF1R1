using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

public class MoveBehaviour : MonoBehaviour
{
    private float speed = 5f;
    public Rigidbody2D rb;

    public void Move(Vector2 direction)
    {
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocityY);
    }
    public void Jump()
    {
        if (rb.gravityScale > 0)
            rb.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
        else
            rb.AddForce(new Vector2(0, -5f), ForceMode2D.Impulse);
        rb.gravityScale *= -1f;
    }
}
