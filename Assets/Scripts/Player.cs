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

    [Header("Invincibility")]
    [SerializeField] private float invincibilityDuration = 1.5f;
    [SerializeField] private float flashInterval = 0.1f; // How fast player flashes
    private float invincibilityTimer = 0f;
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;

    [Header("Death Settings")]
    [SerializeField] private string gameOverSceneName = "GameOver"; // Scene to load on death
    [SerializeField] private float deathDelay = 1.5f; // Delay before loading game over scene (should be at least as long as death sound)

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _jb.Jump();
            // Play jump sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayJumpSound();
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
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        // Handle invincibility timer
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;

            // Flash sprite
            if (spriteRenderer != null)
            {
                float flash = Mathf.PingPong(Time.time / flashInterval, 1f);
                spriteRenderer.enabled = flash > 0.5f;
            }

            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
                // Make sure sprite is visible again
                if (spriteRenderer != null)
                    spriteRenderer.enabled = true;
            }
        }
    }

    public void Hurt()
    {
        // Check if player is invincible or already dead
        if (isInvincible || health <= 0) return;

        _inputActions.Player.Disable();
        health -= 1;

        // Play hurt sound FIRST before any animation
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayHurtSound();
            UnityEngine.Debug.Log("Playing hurt sound");
        }
        else
        {
            UnityEngine.Debug.LogWarning("AudioManager not found!");
        }

        // Activate invincibility
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;

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

        // Stop all movement
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;

        // Play death sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayDeathSound();

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

        // Reset velocity to zero
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;

        if (_rb.gravityScale < 0)
        {
            _rb.gravityScale *= -1f;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
        }

        _rb.transform.position = RespawnPoint;
        _ab.EndHurt();

        // Reset invincibility and make sprite visible
        isInvincible = false;
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

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