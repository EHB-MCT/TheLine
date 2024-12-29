/*
 * This script handles starting the game by loading the first scene (e.g., the game scene). 
 * It loads the scene with the specified index in the Build Settings.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Load the first scene (e.g., the game scene)
    public void StartGameProcess()
    {
        Debug.Log("Starting the game...");
        SceneManager.LoadScene(2); // Load the scene with index 2 (which is the 3rd level in the Build Settings)
    }
}