using UnityEngine;

public class StopGame : MonoBehaviour
{
    [SerializeField] private GameOverPopup gameOverPopup; // Reference to the GameOverPopup

    // Stop het spel door de tijd te bevriezen
    public void StopGameProcess(LineDrawer lineDrawer, float timeElapsed)
    {
        Time.timeScale = 0f;

        int achievedLevel = PlayerManager.Instance.HighestLevelReached;
        float timeForHighestLevel = timeElapsed;

        Debug.Log($"Sending data to database: Level={achievedLevel}, Time={timeForHighestLevel} seconds");

        // Update de database
        PlayerManager.Instance.StartCoroutine(
            PlayerManager.Instance.UpdateHighestLevelAndTimeInDatabase(timeForHighestLevel, achievedLevel)
        );

        // Toon de popup met data
        PlayerManager.Instance.StartCoroutine(
            PlayerManager.Instance.GetPlayerStatsFromDatabase((playerStats) =>
            {
                if (gameOverPopup != null)
                {
                    int bestLevel = playerStats?.HighestLevelReached ?? 0;
                    float bestTime = playerStats != null
                        ? (playerStats.Minutes * 60 + playerStats.Seconds + playerStats.Milliseconds / 1000f)
                        : 0;

                    gameOverPopup.ShowGameOverPopup(achievedLevel, timeForHighestLevel, bestLevel, bestTime);
                }
            })
        );

        StopDrawing stopDrawing = lineDrawer.GetComponent<StopDrawing>();
        stopDrawing?.StopDrawingProcess();
    }
}
