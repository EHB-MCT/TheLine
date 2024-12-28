using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToLeaderboard : MonoBehaviour
{
    // Laad de leaderboard scène
    public void GoToLeaderboardProcess()
    {
        Debug.Log("Going to the leaderboard...");
        SceneManager.LoadScene("Leaderboard"); // Zorg dat de scène "Leaderboard" heet
    }
}

