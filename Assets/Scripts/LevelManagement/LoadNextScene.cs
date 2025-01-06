// This script handles loading the next scene, updating the leaderboard with the player's progress, and checking for additional levels.

using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    // Singleton instance for easy access from other scripts
    public static LoadNextScene Instance;

    void Awake()
    {
        // Ensure only one instance of LoadNextScene exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // Method to load the next scene in the game
    public void LoadNextSceneProcess()
    {
        // Get the current scene index and calculate the next scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Get the elapsed time for the current level from the Timer
        float elapsedTime = Timer.Instance.GetElapsedTime();
        Debug.Log($"Time elapsed for level {currentSceneIndex}: {elapsedTime:F2} seconds");

        // Check and update the highest level and time in the LeaderboardManager
        LeaderboardManager.Instance.UpdateHighestLevel(currentSceneIndex, elapsedTime);

        // Notify that the current level is completed and update stats
        LevelCompletionHandler levelCompletionHandler = gameObject.AddComponent<LevelCompletionHandler>();
        levelCompletionHandler.OnLevelComplete(currentSceneIndex);

        // Check if the next scene exists and load it
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Loading Next Level: " + nextSceneIndex);
            SceneManager.LoadScene(nextSceneIndex); // Load the next level
        }
        else
        {
            // If no more levels exist in the build settings, check for additional levels
            Debug.Log("No more levels in Build Settings. Checking for additional levels...");
            LevelCheck.Instance.CheckForMoreLevels();
        }
    }
}
