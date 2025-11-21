using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MoveBehaviour))]
public class Player : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    private AnimatorBehaviourScript _ab;
    private MoveBehaviour _mb;
    private JumpBehaviour _jb;
    private Rigidbody2D _rb;
    private InputSystem_Actions _inputActions;
    private UIManager _uiManager;

    public Vector2 _dir;
    private int health = 3;
    [SerializeField] private GameObject damageObj;
    public int CheeseCount = 0;
    public Animator Animator;
    public Vector2 RespawnPoint;

    [Header("Death Settings")]
    [SerializeField] private string gameOverSceneName = "GameOver"; // Scene to load on death
    [SerializeField] private float deathDelay = 2f; // Delay before loading game over scene

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _jb.Jump();
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

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _mb = GetComponent<MoveBehaviour>();
        _ab = GetComponent<AnimatorBehaviourScript>();
        _jb = GetComponent<JumpBehaviour>();
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.SetCallbacks(this);
        Animator = GetComponent<Animator>();
        RespawnPoint = transform.position;

        // Find UIManager in the scene
        _uiManager = FindObjectOfType<UIManager>();
        if (_uiManager == null)
        {
            UnityEngine.Debug.LogWarning("UIManager not found in scene!");
        }
    }

    private void Start()
    {
        // Initialize UI with current health
        if (_uiManager != null)
        {
            _uiManager.SetHealth(health);
        }
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        HurtBehaviourScript.OnPlayerHurt += Hurt;
        BulletBehaviour.OnPlayerHurt += Hurt;
        PatrolEnemyBehaviour.OnPlayerHurt += Hurt;
        DoorBehaviourScript.OnPlayerDoor += EnterDoor;
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
        HurtBehaviourScript.OnPlayerHurt -= Hurt;
        BulletBehaviour.OnPlayerHurt -= Hurt;
        PatrolEnemyBehaviour.OnPlayerHurt -= Hurt;
        DoorBehaviourScript.OnPlayerDoor -= EnterDoor;
    }

    void Update()
    {
        _mb.Move(_dir);
    }

    public void Hurt()
    {
        if (health <= 0) return; // Already dead

        _inputActions.Player.Disable();
        health -= 1;

        // Update UI
        if (_uiManager != null)
        {
            _uiManager.TakeDamage(1);
        }

        _rb.linearVelocity = new Vector2(0, 0);
        _ab.GotHurt();
        _rb.linearVelocity = new Vector2(0, 0);

        // Check if dead
        if (health <= 0)
        {
            Die();
        }
        else
        {
            _inputActions.Player.Enable();
        }
    }

    private void Die()
    {
        // Disable player input
        _inputActions.Player.Disable();

        // Optional: Play death animation
        if (_ab != null)
        {
            // You might want to add a death animation trigger here
            // _ab.PlayDeathAnimation();
        }

        // Load game over scene after delay
        Invoke(nameof(LoadGameOverScene), deathDelay);
    }

    private void LoadGameOverScene()
    {
        Time.timeScale = 1f; // Make sure time is running
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameOverSceneName);
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

    public void EnterDoor(DoorBehaviourScript NextDoor, int exitDirection, int level)
    {
        RespawnPoint = NextDoor.transform.position + new Vector3(1.5f * exitDirection, 0);
        _rb.transform.position = RespawnPoint;

        // Update level display in UI
        if (_uiManager != null)
        {
            _uiManager.UpdateLevelText(level);
        }
    }

    // Public methods for health management
    public void Heal(int amount = 1)
    {
        health = Mathf.Clamp(health + amount, 0, 3);
        if (_uiManager != null)
        {
            _uiManager.Heal(amount);
        }
    }

    public void SetHealth(int newHealth)
    {
        health = Mathf.Clamp(newHealth, 0, 3);
        if (_uiManager != null)
        {
            _uiManager.SetHealth(health);
        }
    }

    public int GetHealth()
    {
        return health;
    }
}