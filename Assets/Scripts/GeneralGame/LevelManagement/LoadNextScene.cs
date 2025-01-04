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

        // Log het huidige level
        Debug.Log("Current Level: " + currentSceneIndex);

        // Controleer en update hoogste level in PlayerManager
        PlayerManager.Instance.UpdateHighestLevel(currentSceneIndex);

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
