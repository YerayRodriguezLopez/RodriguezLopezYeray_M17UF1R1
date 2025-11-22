using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("HUD Elements")]
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Hearts (3 hearts)")]
    [SerializeField] private Image[] heartBackgrounds = new Image[3];
    [SerializeField] private Image[] heartBorders = new Image[3];
    [SerializeField] private Image[] hearts = new Image[3];

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private InputSystem_Actions inputActions;
    private bool isPaused = false;
    private int currentHealth = 3;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    void Start()
    {
        // Initialize UI
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        // Setup button listeners
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartLevel);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);

        UpdateHealthDisplay();
    }

    void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Cancel.performed += OnPausePerformed;
    }

    void OnDisable()
    {
        inputActions.UI.Cancel.performed -= OnPausePerformed;
        inputActions.UI.Disable();
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    // Level Display
    public void UpdateLevelText(int levelNumber)
    {
        if (levelText != null)
            levelText.text = "Level " + levelNumber;
    }

    // Health System
    public void SetHealth(int health)
    {
        currentHealth = Mathf.Clamp(health, 0, 3);
        UpdateHealthDisplay();
    }

    public void TakeDamage(int damage = 1)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, 3);
        UpdateHealthDisplay();
    }

    public void Heal(int amount = 1)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, 3);
        UpdateHealthDisplay();
    }

    private void UpdateHealthDisplay()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
            {
                // Show/hide heart based on current health
                hearts[i].enabled = i < currentHealth;

                // Optional: dim the background/border when heart is empty
                if (heartBackgrounds[i] != null)
                {
                    Color bgColor = heartBackgrounds[i].color;
                    bgColor.a = i < currentHealth ? 1f : 0.3f;
                    heartBackgrounds[i].color = bgColor;
                }

                if (heartBorders[i] != null)
                {
                    Color borderColor = heartBorders[i].color;
                    borderColor.a = i < currentHealth ? 1f : 0.3f;
                    heartBorders[i].color = borderColor;
                }
            }
        }
    }

    // Pause Menu Functions
    public void PauseGame()
    {
        isPaused = true;
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void RestartLevel()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f;
        // Replace "MainMenu" with your actual main menu scene name
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}