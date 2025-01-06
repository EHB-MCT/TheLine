// This script is responsible for stopping the game, freezing the time, and displaying the game over popup.

using UnityEngine;

public class StopGame : MonoBehaviour
{
    // Reference to the GameOverPopup for displaying the game over screen
    [SerializeField] private GameOverPopup gameOverPopup;

    // Method to stop the game process
    public void StopGameProcess(LineDrawer lineDrawer, float timeElapsed)
    {
        // Freeze time to stop the game
        Time.timeScale = 0f;

        // Get the last completed level and the time for that level
        int lastCompletedLevel = LeaderboardManager.Instance.LastCompletedLevel;
        float timeForLastCompletedLevel = LeaderboardManager.Instance.TimeForLastCompletedLevel;

        // Log the game over details
        Debug.Log($"Game over! Last completed level: {lastCompletedLevel}, Time: {timeForLastCompletedLevel} seconds.");

        // Update the database with the latest level and time
        LeaderboardManager.Instance.StartCoroutine(
            LeaderboardManager.Instance.UpdateHighestLevelAndTimeInDatabase(timeForLastCompletedLevel, lastCompletedLevel)
        );

        // Show the game over popup with the current attempt details
        if (gameOverPopup != null)
        {
            gameOverPopup.ShowGameOverPopup(lastCompletedLevel, timeForLastCompletedLevel);
        }

        // Stop the drawing activity
        StopDrawing stopDrawing = lineDrawer.GetComponent<StopDrawing>();
        stopDrawing?.StopDrawingProcess(); // Ensure the drawing is stopped
    }
}
