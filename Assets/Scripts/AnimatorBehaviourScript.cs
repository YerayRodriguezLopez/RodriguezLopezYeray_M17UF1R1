using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class AnimatorBehaviourScript : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        if (Mathf.Abs(rb.linearVelocityX) > 0.1f)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }
    public void GotHurt()
    {
        animator.SetBool("Hurt", true);
    }
    public void EndHurt()
    {
        animator.SetBool("Hurt", false);
    }
}
