/*
 * This script manages the Game Over popup UI. 
 * It ensures the popup is hidden when the game starts and provides a method to display it.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel; // Reference to the Game Over UI Panel

    void Start()
    {
        // Ensure the Game Over popup is hidden when the game starts
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // Hide the Game Over popup at the start
        }
        else
        {
            Debug.LogError("Game Over Panel is not assigned in the Inspector!");
        }
    }

    // Display the Game Over popup
    public void ShowGameOverPopup()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // Show the Game Over popup
        }
    }
}