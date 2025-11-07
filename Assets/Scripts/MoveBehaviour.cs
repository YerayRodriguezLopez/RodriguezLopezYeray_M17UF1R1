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
            //slowly flip the player scaling it over half a second
            Vector3 scale = transform.localScale;
            float flipDuration = 0.5f;
            float elapsedTime = 0f;
            while (elapsedTime < flipDuration)
            {
                float t = elapsedTime / flipDuration;
                transform.localScale = new Vector3(scale.x, Mathf.Lerp(scale.y, -scale.y, t), scale.z);
                elapsedTime += Time.deltaTime;
            }
        }
    }
}
