using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel; // Het Game Over-paneel
    [SerializeField] private Text currentAttemptInfoText; // Tekst voor huidige poging
    [SerializeField] private Text highestLevelInfoText; // Tekst voor hoogste niveau en tijd

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

        if (currentAttemptInfoText == null)
        {
            Debug.LogError("Current Attempt Info Text is not assigned in the Inspector!");
        }

        if (highestLevelInfoText == null)
        {
            Debug.LogError("Highest Level Info Text is not assigned in the Inspector!");
        }
    }

    public void ShowGameOverPopup(int achievedLevel, float achievedTime)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            string username = LeaderboardManager.Instance?.Username ?? "Player";

            // Format huidige poging
            int achievedMinutes = Mathf.FloorToInt(achievedTime / 60);
            int achievedSeconds = Mathf.FloorToInt(achievedTime % 60);
            int achievedMilliseconds = Mathf.FloorToInt((achievedTime * 1000) % 1000);

            currentAttemptInfoText.text = $"{username} died after reaching Level {achievedLevel} in " +
                                          $"{achievedMinutes:00}:{achievedSeconds:00}.{achievedMilliseconds:000}";

            // Format hoogste niveau
            int highestMinutes = LeaderboardManager.Instance.MinutesForHighestLevel;
            int highestSeconds = LeaderboardManager.Instance.SecondsForHighestLevel;
            int highestMilliseconds = LeaderboardManager.Instance.MillisecondsForHighestLevel;

            highestLevelInfoText.text = $"Highest Level ever reached Level {LeaderboardManager.Instance.HighestLevelReached} in " +
                                        $"{highestMinutes:00}:{highestSeconds:00}.{highestMilliseconds:000}";

            Debug.Log($"Game Over Popup - Current Attempt: Level {achievedLevel}, Time {achievedMinutes:00}:{achievedSeconds:00}.{achievedMilliseconds:000}");
            Debug.Log($"Game Over Popup - Highest Level: Level {LeaderboardManager.Instance.HighestLevelReached}, Time {highestMinutes:00}:{highestSeconds:00}.{highestMilliseconds:000}");
        }
        else
        {
            Debug.LogError("GameOverPopup: Game Over panel is not assigned.");
        }
    }
}