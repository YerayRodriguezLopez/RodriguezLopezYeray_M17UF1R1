using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;

    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "Game"; // Your main game scene
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    void Start()
    {
        // Make sure time is running
        Time.timeScale = 1f;

        // Setup button listeners
        if (retryButton != null)
            retryButton.onClick.AddListener(RetryGame);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    private void RetryGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}