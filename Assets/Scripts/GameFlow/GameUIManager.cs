using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel; // UI Panel voor Game Over
    [SerializeField] private GameObject gameWonPanel;  // UI Panel voor Game Won

    void Start()
    {
        // Zorg dat beide popups verborgen zijn bij start
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Game Over Panel is not assigned in the Inspector!");
        }

        if (gameWonPanel != null)
        {
            gameWonPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Game Won Panel is not assigned in the Inspector!");
        }
    }

    // Toon de Game Over popup
    public void ShowGameOverPopup()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // Maak de Game Over popup zichtbaar
        }
    }

    // Toon de Game Won popup
    public void ShowGameWonPopup()
    {
        if (gameWonPanel != null)
        {
            gameWonPanel.SetActive(true); // Maak de Game Won popup zichtbaar
        }
    }

    public void StopGame()
    {
        Time.timeScale = 0f; // Stop de tijd
        Debug.Log("Game Stopped. Time is frozen.");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Zet de tijd terug naar normaal
        SceneManager.LoadScene("MainMenu"); // Zorg dat de scène "MainMenu" heet
        TimerScript.Instance.ResetTimer(); // Reset de timer naar 0 bij het herstarten
    }
}
