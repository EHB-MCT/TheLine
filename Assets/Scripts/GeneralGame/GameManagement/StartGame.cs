using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{

    public void StartGameProcess()
    {
        Debug.Log("Starting the game...");
        SceneManager.LoadScene(2); // Laad het eerste level
    }
}
