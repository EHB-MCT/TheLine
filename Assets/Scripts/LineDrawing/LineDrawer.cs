/*
 * This script handles the drawing of a line using the LineRenderer and EdgeCollider2D components.
 * It allows the user to draw a line with the mouse, update it as they move the mouse, and stop drawing when necessary.
 * It also checks for self-intersections and stops the drawing if an intersection is detected.
 */

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(EdgeCollider2D))]
public class LineDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCollider;
    public List<Vector3> points;  // List to store the points of the line
    public bool isDrawing = false;  // Flag to check if drawing is in progress
    public bool isLineStarted = false;

    private StartDrawing startDrawing;  // Reference to StartDrawing component
    private UpdateLine updateLine;  // Reference to UpdateLine component
    private StopDrawing stopDrawing;  // Reference to StopDrawing component
    private LineSelfIntersectionChecker intersectionChecker;  // Reference to self-intersection checker

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        startDrawing = GetComponent<StartDrawing>();
        updateLine = GetComponent<UpdateLine>();
        stopDrawing = GetComponent<StopDrawing>();  // Ensure proper reference to StopDrawing
        intersectionChecker = GetComponent<LineSelfIntersectionChecker>();

        // Check if required components are present
        if (lineRenderer == null || edgeCollider == null)
        {
            Debug.LogError("Required components missing on this GameObject.");
            return;
        }

        points = new List<Vector3>();  // Initialize the list of points

        // Set up LineRenderer properties
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.useWorldSpace = true;

        // Set the EdgeCollider to be a trigger
        edgeCollider.isTrigger = true;
    }

    void Update()
    {
        // Do not update if time is paused or drawing is not enabled
        if (Time.timeScale == 0f || !isDrawing)
            return;

        // Start drawing when the mouse button is pressed and drawing is not already in progress
        if (Input.GetMouseButtonDown(0) && !isDrawing)
        {
            startDrawing.StartDrawingProcess(Vector3.zero);  // Start at the initial point
        }

        // Update the line position while the mouse button is held down
        if (Input.GetMouseButton(0) && isDrawing)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 2f;  // Set the distance from the camera
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            updateLine.UpdateLineProcess(worldPos);  // Update the line with the current mouse position
        }

        // Stop drawing when the mouse button is released and check for intersections
        if (Input.GetMouseButtonUp(0))
        {
            if (isDrawing)
            {
                // If the line intersects itself, stop drawing and clear the line
                if (intersectionChecker.CheckForSelfIntersection(points))
                {
                    Debug.Log("Line intersected, clearing line.");
                    stopDrawing.StopDrawingProcess();  // Correct method call to stop drawing
                }
            }
        }
    }

    // Method to access the list of points
    public List<Vector3> GetPoints()
    {
        return points;
    }
}