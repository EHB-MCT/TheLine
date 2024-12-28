using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel; // UI Panel voor Game Over

    void Start()
    {
        // Zorg dat de Game Over popup verborgen is bij het starten
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Game Over Panel is not assigned in the Inspector!");
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
}


