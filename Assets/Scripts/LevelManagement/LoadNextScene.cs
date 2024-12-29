/*
 * This script loads the next scene in the game.
 * If there are no more scenes, it checks for more levels using the LevelCheck singleton.
 * It uses a singleton pattern for easy access from other scripts.
 */

using System.Collections;
using System.Collections.Generic;
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

    // Method to load the next scene
    public void LoadNextSceneProcess()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene exists in the Build Settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex); // Load the next scene
        }
        else
        {
            LevelCheck.Instance.CheckForMoreLevels();  // Call LevelCheck to see if more levels exist
        }
    }
}