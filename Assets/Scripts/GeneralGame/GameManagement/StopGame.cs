using UnityEngine;

public class StopGame : MonoBehaviour
{
    [SerializeField] private GameOverPopup gameOverPopup; // Reference to the GameOverPopup

    // Stop het spel door de tijd te bevriezen
    public void StopGameProcess(LineDrawer lineDrawer)
    {
        Time.timeScale = 0f; // Freeze time

        // Toon het Game Over popup
        if (gameOverPopup != null)
        {
            gameOverPopup.ShowGameOverPopup(); // Toon het Game Over popup
        }
        else
        {
            Debug.LogError("GameOverPopup component not assigned in the Inspector!");
        }

        // Stop de tekenfunctionaliteit
        StopDrawing stopDrawing = lineDrawer.GetComponent<StopDrawing>();
        if (stopDrawing != null)
        {
            stopDrawing.StopDrawingProcess(); // Stop de tekeningen
        }
        else
        {
            Debug.LogError("StopDrawing component not found on LineDrawer.");
        }
    }
}

