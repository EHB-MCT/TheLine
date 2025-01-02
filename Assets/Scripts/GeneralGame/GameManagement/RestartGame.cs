/*
 * This script handles restarting the game by returning to the MainMenu scene. 
 * It resets the time scale, reloads the "MainMenu" scene, and resets the timer.
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    // Restart the game by returning to the MainMenu
    public void RestartGameProcess()
    {
        Time.timeScale = 1f; // Set the time scale back to normal
        SceneManager.LoadScene("MainMenu"); // Ensure the scene is named "MainMenu"
        Timer.Instance.ResetTimer(); // Reset the timer to 0 upon restart
    }
}