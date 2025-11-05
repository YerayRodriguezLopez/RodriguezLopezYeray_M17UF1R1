using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MoveBehaviour))]

public class Player : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    private AnimatorBehaviourScript _ab;
    private MoveBehaviour _mb;
    private Rigidbody2D _rb;
    private InputSystem_Actions _inputActions;
    public Vector2 _dir;
    public int health = 3;
    [SerializeField] private GameObject damageObj;
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _mb.Jump();
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        _dir = context.ReadValue<Vector2>();
        if (_dir.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (_dir.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _mb = GetComponent<MoveBehaviour>();
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.SetCallbacks(this);
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        _mb.Move(_dir);
    }
    // When the player collides with an obstacle get knocked back and a little bit up
    public void Hurt()
    {
        Vector2 knockback = new Vector2(-_dir.x * 5f, 5f);
        _rb.AddForce(knockback, ForceMode2D.Impulse);
        health -= 1;
        _ab.HurtAnimation();
        if (health <= 0)
        {
            // Handle player death (e.g., reload scene, show game over screen, etc.)
            Debug.Log("Player has died.");
        }
    }
}
