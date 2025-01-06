// This script handles the display of a game over popup, showing the current attempt's information and the highest level reached.

using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel; // The Game Over panel to display after the game ends
    [SerializeField] private Text currentAttemptInfoText; // Text element to show current attempt details (level and time)
    [SerializeField] private Text highestLevelInfoText; // Text element to show highest level reached and time

    void Start()
    {
        // Hide the Game Over panel at the start if it's assigned
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // Make the panel invisible initially
        }
        else
        {
            Debug.LogError("Game Over Panel is not assigned in the Inspector!"); // Log error if the panel is not assigned
        }

        // Verify if the text elements are assigned in the Inspector
        if (currentAttemptInfoText == null)
        {
            Debug.LogError("Current Attempt Info Text is not assigned in the Inspector!");
        }

        if (highestLevelInfoText == null)
        {
            Debug.LogError("Highest Level Info Text is not assigned in the Inspector!");
        }
    }

    // Method to display the game over popup with current level and time
    public void ShowGameOverPopup(int achievedLevel, float achievedTime)
    {
        // Ensure the game over panel is assigned
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // Show the Game Over panel

            string username = LeaderboardManager.Instance?.Username ?? "Player"; // Get the username or default to "Player" if not available

            // Format the current attempt time in minutes, seconds, and milliseconds
            int achievedMinutes = Mathf.FloorToInt(achievedTime / 60);
            int achievedSeconds = Mathf.FloorToInt(achievedTime % 60);
            int achievedMilliseconds = Mathf.FloorToInt((achievedTime * 1000) % 1000);

            // Display the current attempt information (level and time)
            currentAttemptInfoText.text = $"{username} died after reaching Level {achievedLevel} in " +
                                          $"{achievedMinutes:00}:{achievedSeconds:00}.{achievedMilliseconds:000}";

            // Get and format the highest level and time information
            int highestMinutes = LeaderboardManager.Instance.MinutesForHighestLevel;
            int highestSeconds = LeaderboardManager.Instance.SecondsForHighestLevel;
            int highestMilliseconds = LeaderboardManager.Instance.MillisecondsForHighestLevel;

            // Display the highest level information
            highestLevelInfoText.text = $"Highest Level ever reached Level {LeaderboardManager.Instance.HighestLevelReached} in " +
                                        $"{highestMinutes:00}:{highestSeconds:00}.{highestMilliseconds:000}";

            // Log the details for debugging
            Debug.Log($"Game Over Popup - Current Attempt: Level {achievedLevel}, Time {achievedMinutes:00}:{achievedSeconds:00}.{achievedMilliseconds:000}");
            Debug.Log($"Game Over Popup - Highest Level: Level {LeaderboardManager.Instance.HighestLevelReached}, Time {highestMinutes:00}:{highestSeconds:00}.{highestMilliseconds:000}");
        }
        else
        {
            Debug.LogError("GameOverPopup: Game Over panel is not assigned."); // Log error if panel is not assigned
        }
    }
}
