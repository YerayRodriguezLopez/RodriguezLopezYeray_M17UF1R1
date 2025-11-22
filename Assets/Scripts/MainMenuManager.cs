using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;

    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "VVVVV"; // Your main game scene

    void Start()
    {
        // Make sure time is running
        Time.timeScale = 1f;

        // Show main menu, hide others
        ShowMainMenu();

        // Setup button listeners
        if (playButton != null)
            playButton.onClick.AddListener(PlayGame);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    private void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    private void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
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