using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MoveBehaviour))]

public class Player : MonoBehaviour, InputSystem_Actions.IPlayerActions
{

    private MoveBehaviour _mb;
    private Rigidbody2D _rb;
    private InputSystem_Actions _inputActions;
    public Vector2 _dir;
    public void OnJump(InputAction.CallbackContext context)
    {
        //do only if on the ground
        if (!Mathf.Approximately(_rb.linearVelocityY, 0f))
        {
            if (context.performed)
            {
                _mb.Jump();
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            }
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
}
