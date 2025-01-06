using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void StartGameProcess()
    {
        if (Login.IsLoggedIn)
        {
            Debug.Log("Starting the game...");
            
            // Meld dat level 1 wordt gestart
            LevelCompletionHandler levelCompletionHandler = gameObject.AddComponent<LevelCompletionHandler>();
            levelCompletionHandler.OnLevelComplete(1); // Level 1 is de eerste scene met stats
            
            SceneManager.LoadScene(2); // Laad het eerste level
        }
        else
        {
            Debug.LogWarning("You must log in before starting the game.");
        }
    }
}
