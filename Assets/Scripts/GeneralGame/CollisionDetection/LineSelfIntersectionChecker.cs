/*
 * This script checks if a line drawn using the attached LineDrawer component intersects itself. 
 * If it does, the game is stopped using the StopGame script.
 */

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineDrawer))]
public class LineSelfIntersectionChecker : MonoBehaviour
{
    private LineDrawer lineDrawer;
    private bool isIntersected = false;

    [SerializeField] private StopGame stopGame; // Reference to the StopGame script

    void Start()
    {
        // Retrieve the LineDrawer component from the GameObject and verify its existence.
        lineDrawer = GetComponent<LineDrawer>();
        if (lineDrawer == null)
        {
            Debug.LogError("LineDrawer component is missing on this GameObject.");
        }

        // Verify that the StopGame script is properly assigned in the Inspector.
        if (stopGame == null)
        {
            Debug.LogError("StopGame script is not assigned in the Inspector!");
        }
    }

    void Update()
    {
        // Only check if the line is not already self-intersecting and the LineDrawer component is available.
        if (lineDrawer != null && !isIntersected)
        {
            List<Vector3> points = lineDrawer.GetPoints();

            // Check if the line intersects itself.
            if (CheckForSelfIntersection(points))
            {
                Debug.Log("GAME OVER! The line has self-intersected.");
                isIntersected = true;

                // Stop the game using the StopGame script.
                if (stopGame != null)
                {
                    stopGame.StopGameProcess(lineDrawer);
                }
            }
        }
    }

    // Check if the line intersects itself based on a list of points.
    public bool CheckForSelfIntersection(List<Vector3> points)
    {
        if (points.Count < 2)
        {
            return false; // Not enough points to cause a self-intersection.
        }

        Vector3 lastPoint = points[points.Count - 1];

        // Compare the last point with previous points in the list.
        for (int i = 0; i < points.Count - 1; i++)
        {
            if (Vector3.Distance(lastPoint, points[i]) < 0.1f) // Use a threshold for precision.
            {
                return true;
            }
        }

        return false;
    }

    // Reset the intersection status to allow a new line to be drawn without issues.
    public void ResetIntersection()
    {
        isIntersected = false;
    }
}