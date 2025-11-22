using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class VictoryManager : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;

    [Header("Scene Names")]
    [SerializeField] private string gameScene = "VVVVV"; // Next level or restart
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    void Start()
    {
        // Make sure time is running
        Time.timeScale = 1f;

        // Setup button listeners
        if (restartButton != null)
            restartButton.onClick.AddListener(ContinueToNextLevel);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }


    private void ContinueToNextLevel()
    {
        SceneManager.LoadScene(gameScene);
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