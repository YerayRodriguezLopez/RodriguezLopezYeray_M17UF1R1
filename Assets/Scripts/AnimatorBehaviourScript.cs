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

    public void Update()
    {
        animator.Set
    }
    public void HurtAnimation()
    {
        animator.SetBool("Hurt", true);
    }
}
