using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Laad de eerste scène (bijvoorbeeld het spel)
    public void StartGame()
    {
        Debug.Log("Starting the game...");
        SceneManager.LoadScene(2); // Laad scène met index 1
    }

    // Laad de leaderboard scène
    public void GoToLeaderboard()
    {
        Debug.Log("Going to the leaderboard...");
        SceneManager.LoadScene("Leaderboard"); // Zorg dat de scène "Leaderboard" heet
    }

    // Terugkeren naar het hoofdmenu
    public void BackToMainMenu()
    {
        Debug.Log("Returning to the main menu...");
        SceneManager.LoadScene("MainMenu"); // Zorg dat de scène "MainMenu" heet
    }
}