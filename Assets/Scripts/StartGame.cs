using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Laad de eerste scène (bijvoorbeeld het spel)
    public void StartGameProcess()
    {
        Debug.Log("Starting the game...");
        SceneManager.LoadScene(2); // Laad scène met index 2 (2 is het 3e level in de Build Settings)
    }
}
