/*
 * This script handles the initialization of the drawing process. 
 * It starts the line drawing, sets up the starting point, clears previous data, 
 * and resets necessary components like the collider and intersection checker.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDrawing : MonoBehaviour
{
    private LineDrawer lineDrawer;  // Reference to the LineDrawer component
    private LineSelfIntersectionChecker intersectionChecker;  // Reference to the intersection checker component

    void Start()
    {
        // Get the necessary components at the start
        lineDrawer = GetComponent<LineDrawer>();
        intersectionChecker = GetComponent<LineSelfIntersectionChecker>();
    }

    // This method is called to begin the drawing process from a specific start position
    public void StartDrawingProcess(Vector3 startPosition)
    {
        // Set the drawing flag to true, indicating the start of the drawing process
        lineDrawer.isDrawing = true;
        lineDrawer.isLineStarted = true;
        
        // Clear any previously drawn points and reset the line renderer
        lineDrawer.points.Clear();
        lineDrawer.lineRenderer.positionCount = 0;

        // Convert the start position from screen space to world space, and ensure z is set to 0 for 2D
        Vector3 worldStartPosition = Camera.main.ScreenToWorldPoint(startPosition);
        worldStartPosition.z = 0; // Ensure the z-coordinate is 0 for 2D drawing

        // Add the start point to the list of points and set it as the first position in the line renderer
        lineDrawer.points.Add(worldStartPosition);  
        lineDrawer.lineRenderer.positionCount = lineDrawer.points.Count;
        lineDrawer.lineRenderer.SetPosition(0, worldStartPosition);

        // Reset the collider to ensure it starts fresh
        lineDrawer.edgeCollider.points = new Vector2[0];

        // Reset the intersection status to detect any future line intersections
        intersectionChecker.ResetIntersection();
    }
}