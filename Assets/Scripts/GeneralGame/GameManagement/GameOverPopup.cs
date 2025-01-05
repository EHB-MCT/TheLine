using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel; // Het Game Over-paneel
    [SerializeField] private Text infoText; // Eén tekstcomponent voor zowel level als tijd

    void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // Zorg dat het paneel verborgen is bij start
        }
        else
        {
            Debug.LogError("Game Over Panel is not assigned in the Inspector!");
        }

        if (infoText == null)
        {
            Debug.LogError("Info Text is not assigned in the Inspector!");
        }
    }

    public void ShowGameOverPopup(int achievedLevel, float achievedTime, int bestLevel, float bestTime)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            string username = PlayerManager.Instance?.Username ?? "Player";

            // Format huidige poging
            int achievedMinutes = Mathf.FloorToInt(achievedTime / 60);
            int achievedSeconds = Mathf.FloorToInt(achievedTime % 60);
            int achievedMilliseconds = Mathf.FloorToInt((achievedTime * 1000) % 1000);

            // Format beste poging
            int bestMinutes = Mathf.FloorToInt(bestTime / 60);
            int bestSeconds = Mathf.FloorToInt(bestTime % 60);
            int bestMilliseconds = Mathf.FloorToInt((bestTime * 1000) % 1000);

            infoText.text = $"{username} died after reaching Level {achievedLevel} in " +
                            $"{achievedMinutes:00}:{achievedSeconds:00}.{achievedMilliseconds:000}\n" +
                            $"Best Attempt: Level {bestLevel} in " +
                            $"{bestMinutes:00}:{bestSeconds:00}.{bestMilliseconds:000}";

            Debug.Log($"Game Over Popup - Current: Level {achievedLevel}, Time {achievedMinutes:00}:{achievedSeconds:00}.{achievedMilliseconds:000}, " +
                    $"Best: Level {bestLevel}, Time {bestMinutes:00}:{bestSeconds:00}.{bestMilliseconds:000}");
        }
        else
        {
            Debug.LogError("GameOverPopup: Game Over panel is not assigned.");
        }
    }

}
