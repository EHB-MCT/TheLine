using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineDrawer))]
public class LineSelfIntersectionChecker : MonoBehaviour
{
    private LineDrawer lineDrawer;
    private bool isIntersected = false;

    void Start()
    {
        lineDrawer = GetComponent<LineDrawer>();
        if (lineDrawer == null)
        {
            Debug.LogError("LineDrawer component missing on this GameObject.");
        }
    }

    public void Update()
    {
        if (lineDrawer != null && !isIntersected)
        {
            List<Vector3> points = lineDrawer.GetPoints();

            if (CheckForSelfIntersection(points))
            {
                Debug.Log("GAME LOST! Line self-intersected.");
                lineDrawer.StopDrawing(); // Stop drawing immediately
                isIntersected = true; // Mark the line as intersected
                EndGame(); // Trigger end game logic
            }
        }
    }

    public bool CheckForSelfIntersection(List<Vector3> points)
    {
        if (points.Count < 2)
        {
            return false; // Too few points to check
        }

        Vector3 lastPoint = points[points.Count - 1];

        // Check if the last point intersects with any previous point
        for (int i = 0; i < points.Count - 1; i++)
        {
            if (Vector3.Distance(lastPoint, points[i]) < 0.1f) // Tolerance of 0.1
            {
                return true; // Intersection detected
            }
        }

        return false;
    }

    public void ResetIntersection()
    {
        isIntersected = false; // Allow drawing a new line again
    }

    void EndGame()
    {
        // Here you can add any game over logic, such as disabling further input
        // and showing a game over UI or restarting the level.
        Debug.Log("GAME OVER - You lost!");
        // Example: Load a Game Over scene (this requires scene management setup)
        // SceneManager.LoadScene("GameOverScene");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Line")) // If the line intersects with itself
        {
            Debug.Log("Lijn raakt zichzelf! GAME LOST.");
            lineDrawer.StopDrawing(); // Stop drawing
            EndGame(); // Trigger the game over logic
        }
    }
}
