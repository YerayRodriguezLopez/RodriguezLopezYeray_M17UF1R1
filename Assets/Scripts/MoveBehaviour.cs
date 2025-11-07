using System.Diagnostics;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Player))]

public class MoveBehaviour : MonoBehaviour
{
    private float speed = 5f;
    public Rigidbody2D rb;
    public Player player;

    public void Move(Vector2 direction)
    {
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocityY);
    }
    public void Jump()
    {
        //check if the player is on the ground
        if (Mathf.Abs(rb.linearVelocityY) < 0.01f || player.cheeseCount >= 5)
        {
            if (rb.gravityScale > 0)
                rb.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
            else
                rb.AddForce(new Vector2(0, -5f), ForceMode2D.Impulse);
            rb.gravityScale *= -1f;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
        }
    }
}
