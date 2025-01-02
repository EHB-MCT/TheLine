/*
 * This script handles navigation to the leaderboard scene. 
 * When triggered, it loads the "Leaderboard" scene.
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToLeaderboard : MonoBehaviour
{
    // Load the leaderboard scene
    public void GoToLeaderboardProcess()
    {
        Debug.Log("Navigating to the leaderboard...");
        SceneManager.LoadScene("Leaderboard"); // Ensure the scene is named "Leaderboard"
    }
}
