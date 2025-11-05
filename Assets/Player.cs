using System.Diagnostics;
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
    private bool goingRight = true;
    private bool hitted = false;
    private Stopwatch stopwatch;
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
            goingRight = true;
        }
        else if (_dir.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            goingRight = false;
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
        // Move the player based on input and if not hitted
        if (hitted)
        {
            if (stopwatch == null)
            {
                stopwatch = Stopwatch.StartNew();
            }
            else if (stopwatch.ElapsedMilliseconds >= 500)
            {
                hitted = false;
                stopwatch.Stop();
                stopwatch = null;
            }
        }
        else
        {
            _mb.Move(_dir);
        }
    }
    // When the player collides with an obstacle get knocked back and a little bit up
    public void Hurt()
    {
        health--;
        hitted = true;
        Vector2 knockbackDir = goingRight ? Vector2.left : Vector2.right;
        _rb.linearVelocity = knockbackDir * 5f + Vector2.up * 5f;
        if (health <= 0)
        {
            // Restart the level
            //UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}
