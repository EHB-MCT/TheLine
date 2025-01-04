using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void StartGameProcess()
    {
        if (Login.IsLoggedIn)
        {
            Debug.Log("Starting the game...");
            SceneManager.LoadScene(2); // Laad het eerste level
        }
        else
        {
            Debug.LogWarning("You must log in before starting the game.");
        }
    }
}
