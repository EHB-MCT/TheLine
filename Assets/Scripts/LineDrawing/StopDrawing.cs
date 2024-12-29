/*
 * This script stops the drawing process. 
 * It clears the drawn points, resets the line renderer, and clears the collider when drawing is stopped.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopDrawing : MonoBehaviour
{
    private LineDrawer lineDrawer;  // Reference to the LineDrawer component

    void Start()
    {
        // Get the LineDrawer component at the start
        lineDrawer = GetComponent<LineDrawer>();
    }

    // This method is called to stop the drawing process
    public void StopDrawingProcess()
    {
        // Set the drawing flag to false, indicating the drawing has stopped
        lineDrawer.isDrawing = false;

        // Clear the list of points and reset the line renderer
        lineDrawer.points.Clear();
        lineDrawer.lineRenderer.positionCount = 0;

        // Reset the collider when the drawing is stopped
        lineDrawer.edgeCollider.points = new Vector2[0];
    }
}