/*
 * This script manages the drawing mechanics of the line. It handles the start and end points of the drawing, 
 * checks if the line is being drawn, and updates the line as the user moves the mouse.
 * Additionally, it checks if the line reaches the end point and triggers loading the next scene when the drawing is complete.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMechanicsManager : MonoBehaviour
{
    public RectTransform startPoint;  // The start point of the drawing
    public RectTransform endPoint;    // The end point of the drawing

    private StartPointHandler startPointHandler;  // Handler for the start point
    private EndPointHandler endPointHandler;      // Handler for the end point
    private LineDrawer lineDrawer;                // LineDrawer component to manage line drawing
    private StartDrawing startDrawing;            // Component to start the drawing process
    private UpdateLine updateLine;                // Component to update the line as it's being drawn

    private bool isDrawing;  // Flag to track whether drawing is in progress
    private Camera mainCamera;  // The main camera to convert mouse position to world space

    void Start()
    {
        mainCamera = Camera.main;  // Get the main camera
        startPointHandler = gameObject.AddComponent<StartPointHandler>();  // Add and get the StartPointHandler component
        endPointHandler = gameObject.AddComponent<EndPointHandler>();      // Add and get the EndPointHandler component
        lineDrawer = GetComponent<LineDrawer>();  // Get the LineDrawer component
        startDrawing = GetComponent<StartDrawing>();  // Get the StartDrawing component
        updateLine = GetComponent<UpdateLine>();  // Get the UpdateLine component

        // Assign the start and end points to the respective handlers
        startPointHandler.startPoint = startPoint;
        endPointHandler.endPoint = endPoint;
    }

    void Update()
    {
        // Stop drawing if the game is paused
        if (Time.timeScale == 0f)
            return;

        // Start drawing when the mouse button is pressed down and the cursor is over the start point
        if (Input.GetMouseButtonDown(0))
        {
            if (startPointHandler.IsMouseOverStartPoint())  // Check if the mouse is over the start point
            {
                isDrawing = true;
                startDrawing.StartDrawingProcess(startPoint.position);  // Start drawing from the start point
            }
        }

        // While the mouse button is held down, update the line as the mouse moves
        if (Input.GetMouseButton(0) && isDrawing)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 2f;  // Set the correct z-value for the 2D line
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);  // Convert mouse position to world space
            updateLine.UpdateLineProcess(worldPos);  // Update the line position as the mouse moves
        }

        // Stop drawing when the mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
        }

        // Check if the line touches the end point and trigger the next scene
        if (isDrawing && endPointHandler.IsLineTouchingEndPoint(lineDrawer.GetPoints()))
        {
            LoadNextScene.Instance.LoadNextSceneProcess();  // Load the next scene using the singleton

            isDrawing = false;
        }
    }
}