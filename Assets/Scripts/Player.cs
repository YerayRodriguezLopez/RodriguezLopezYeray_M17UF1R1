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
    private int deaths;
    [SerializeField] private GameObject damageObj;
    private Stopwatch stopwatch;
    public int CheeseCount = 0;
    public Animator Animator;
    public Vector2 RespawnPoint;
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
        Animator = GetComponent<Animator>();
        RespawnPoint = transform.position;
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        HurtBehaviourScript.OnPlayerHurt += Hurt;
        DoorBehaviourScript.OnPlayerDoor += EnterDoor;
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
        HurtBehaviourScript.OnPlayerHurt -= Hurt;
        DoorBehaviourScript.OnPlayerDoor -= EnterDoor;
    }

    // Update is called once per frame
    void Update()
    {
        _mb.Move(_dir);
    }
    // When the player collides with an obstacle get knocked back and a little bit up
    public void Hurt()
    {
        if (_rb.gravityScale < 0)
        {
            _rb.linearVelocityY = 0;
            _rb.gravityScale
        }
        _rb.transform.position = RespawnPoint;
    }
    public void EnterDoor()
    {
        // Move and set the respawn point to the next door if the current one is odd, if even move to the previous door using the game manager and update the DoorTouched
        GameManager.instance.DoorTouched++;
        if (GameManager.instance.DoorTouched % 2 == 0)
        {
            RespawnPoint = GameManager.instance.doorsList[GameManager.instance.DoorTouched - 2].transform.position;
            RespawnPoint.x += 1f;
            transform.position = RespawnPoint;
        }
        else
        {
            RespawnPoint = GameManager.instance.doorsList[GameManager.instance.DoorTouched].transform.position;
            RespawnPoint.x += 1f;
            transform.position = RespawnPoint;
        }
    }
}
