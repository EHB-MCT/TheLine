/*
 * This script handles returning to the main menu. 
 * When triggered, it loads the "MainMenu" scene.
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    // Method to return to the main menu
    public void BackToMainMenuProcess()
    {
        Debug.Log("Returning to the main menu...");
        SceneManager.LoadScene("MainMenu"); // Ensure the scene is named "MainMenu"
    }
}