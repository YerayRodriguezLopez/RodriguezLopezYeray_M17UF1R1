using UnityEngine;

public class SceneMusicStarter : MonoBehaviour
{
    [SerializeField] private bool isGameScene = true; // True for game, false for menu

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            if (isGameScene)
                AudioManager.Instance.PlayGameMusic();
            else
                AudioManager.Instance.PlayMenuMusic();
        }
    }
}