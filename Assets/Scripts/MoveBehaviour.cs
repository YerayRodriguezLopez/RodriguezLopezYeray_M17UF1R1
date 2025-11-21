using System.Diagnostics;
using UnityEngine;

public class MoveBehaviour : MonoBehaviour
{
    private float speed = 5f;
    public Rigidbody2D rb;

    public void Move(Vector2 direction)
    {
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocityY);
    }
}
