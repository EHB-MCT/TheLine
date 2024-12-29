/*
 * This script stops the game by freezing time, displaying the Game Over popup, 
 * and halting the line drawing process.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopGame : MonoBehaviour
{
    [SerializeField] private GameOverPopup gameOverPopup; // Reference to the GameOverPopup

    // Stop the game by freezing time
    public void StopGameProcess(LineDrawer lineDrawer)
    {
        Time.timeScale = 0f; // Freeze time
        Debug.Log("Game Stopped. Time is frozen.");

        // Call the ShowGameOverPopup method to display the Game Over popup
        if (gameOverPopup != null)
        {
            gameOverPopup.ShowGameOverPopup(); // Show the Game Over popup
        }
        else
        {
            Debug.LogError("GameOverPopup component not assigned in the Inspector!");
        }

        // Ensure the StopDrawingProcess method is called via the correct component
        StopDrawing stopDrawing = lineDrawer.GetComponent<StopDrawing>();
        if (stopDrawing != null)
        {
            stopDrawing.StopDrawingProcess(); // Stop drawing the lines
        }
        else
        {
            Debug.LogError("StopDrawing component not found on LineDrawer.");
        }
    }
}