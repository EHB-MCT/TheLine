// This script is responsible for starting the game and loading the first level after verifying the login status.

using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class StartGame : MonoBehaviour
{
    // Method to start the game process
    public void StartGameProcess()
    {
        // Check if the player is logged in
        if (Login.IsLoggedIn)
        {
            Debug.Log("Starting the game..."); // Log message to indicate the game is starting

            // Create a new LevelCompletionHandler instance and report that level 1 is starting
            LevelCompletionHandler levelCompletionHandler = gameObject.AddComponent<LevelCompletionHandler>();
            levelCompletionHandler.OnLevelComplete(1); // Inform the handler that level 1 is completed
            
            // Load the first level (Scene 2)
            SceneManager.LoadScene(2); // Scene index 2 represents the first game level
        }
        else
        {
            // Log a warning if the player is not logged in
            Debug.LogWarning("You must log in before starting the game.");
        }
    }
}
