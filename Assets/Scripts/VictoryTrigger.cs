using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryTrigger : MonoBehaviour
{
    [SerializeField] private string victorySceneName = "Victory";
    [SerializeField] private int currentLevel = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            // Play victory sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayVictorySound();

            // Save stats for victory screen
            PlayerPrefs.SetInt("CheeseCollected", player.CheeseCount);
            PlayerPrefs.SetInt("LevelCompleted", currentLevel);
            PlayerPrefs.Save();

            // Load victory scene
            SceneManager.LoadScene(victorySceneName);
        }
    }
}