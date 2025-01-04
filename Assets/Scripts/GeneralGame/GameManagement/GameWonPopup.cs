/*
 * This script manages the Game Won popup UI. 
 * It ensures the popup is hidden when the game starts and provides a method to display it.
 */

using UnityEngine;

public class GameWonPopup : MonoBehaviour
{
    [SerializeField] private GameObject gameWonPanel;  // Reference to the Game Won UI Panel

    void Start()
    {
        // Ensure the Game Won popup is hidden when the game starts
        if (gameWonPanel != null)
        {
            gameWonPanel.SetActive(false); // Hide the Game Won popup at the start
        }
        else
        {
            Debug.LogError("Game Won Panel is not assigned in the Inspector!");
        }
    }

    // Display the Game Won popup
    public void ShowGameWonPopup()
    {
        if (gameWonPanel != null)
        {
            gameWonPanel.SetActive(true); // Show the Game Won popup
        }
    }
}