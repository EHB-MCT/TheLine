using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    // Singleton for easy access from other scripts
    public static LoadNextScene Instance;

    void Awake()
    {
        // Ensure only one LoadNextScene instance exists and make it accessible via the singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadNextSceneProcess()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Haal verstreken tijd op voor dit level
        float elapsedTime = Timer.Instance.GetElapsedTime();
        Debug.Log($"Time elapsed for level {currentSceneIndex}: {elapsedTime:F2} seconds");

        // Controleer en update hoogste level en sla tijd op in LeaderboardManager
        LeaderboardManager.Instance.UpdateHighestLevel(currentSceneIndex, elapsedTime);

        // Meld dat het huidige level voltooid is
        LevelCompletionHandler levelCompletionHandler = gameObject.AddComponent<LevelCompletionHandler>();
        levelCompletionHandler.OnLevelComplete(currentSceneIndex);

        // Check of het volgende level beschikbaar is
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Loading Next Level: " + nextSceneIndex);
            SceneManager.LoadScene(nextSceneIndex); // Laad het volgende level
        }
        else
        {
            Debug.Log("No more levels in Build Settings. Checking for additional levels...");
            LevelCheck.Instance.CheckForMoreLevels();
        }
    }

}
