/*
 * This script checks if there are more levels available in the game. 
 * If there are no more levels, it displays the Game Won popup.
 * It uses a singleton pattern for easy access from other scripts.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCheck : MonoBehaviour
{
    // Singleton for easy access from other scripts
    public static LevelCheck Instance;

    // Reference to the GameWonPopup (ensure this script is correctly assigned)
    public GameWonPopup gameWonPopup;

    void Awake()
    {
        // Ensure only one LevelCheck instance exists and make it accessible via the singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Check if there are more levels available
    public void CheckForMoreLevels()
    {
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // If there is no next scene
        if (nextSceneIndex >= UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
        {
            gameWonPopup.ShowGameWonPopup();  // If no more levels, show the GameWonPopup
        }
    }
}