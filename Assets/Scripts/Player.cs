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
    private int health = 3;
    [SerializeField] private GameObject damageObj;
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
        _ab = GetComponent<AnimatorBehaviourScript>();
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.SetCallbacks(this);
        Animator = GetComponent<Animator>();
        RespawnPoint = transform.position;
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        HurtBehaviourScript.OnPlayerHurt += Hurt;
        BulletBehaviour.OnPlayerHurt += Hurt;
        DoorBehaviourScript.OnPlayerDoor += EnterDoor;
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
        HurtBehaviourScript.OnPlayerHurt -= Hurt;
        BulletBehaviour.OnPlayerHurt -= Hurt;
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
        _inputActions.Player.Disable();
        health -= 1;
        _rb.linearVelocity = new Vector2(0, 0);
        _ab.GotHurt();
        _rb.linearVelocity = new Vector2(0, 0);
        _inputActions.Player.Enable();
    }
    public void ResetPlayerAfterDeath()
    {
        _inputActions.Player.Disable();
        if (_rb.gravityScale < 0)
        {
            _rb.gravityScale *= -1f;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
        }
        _rb.transform.position = RespawnPoint;
        _ab.EndHurt();
        _inputActions.Player.Enable();
    }
    public void EnterDoor(DoorBehaviourScript NextDoor, int exitDirection)
    {
        RespawnPoint = NextDoor.transform.position + new Vector3(1.5f * exitDirection, 0);
        _rb.transform.position = RespawnPoint;
    }
}
