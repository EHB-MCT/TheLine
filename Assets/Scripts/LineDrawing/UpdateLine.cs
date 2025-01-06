/*
 * This script updates the drawn line by adding new points to the line and updating the line renderer.
 * It also updates the collider to match the new line as it is drawn.
 */

using System.Collections.Generic;
using UnityEngine;

public class UpdateLine : MonoBehaviour
{
    private LineDrawer lineDrawer;  // Reference to the LineDrawer component

    void Start()
    {
        // Get the LineDrawer component at the start
        lineDrawer = GetComponent<LineDrawer>();
    }

    // This method is called to update the drawn line with new points
    public void UpdateLineProcess(Vector3 position)
    {
        // Check if the line has been started and if the new point is sufficiently far from the last one
        if (lineDrawer.points.Count == 0 || Vector3.Distance(lineDrawer.points[lineDrawer.points.Count - 1], position) > 0.1f)
        {
            // Add the new position to the points list
            lineDrawer.points.Add(position);

            // Update the line renderer with the new point
            lineDrawer.lineRenderer.positionCount = lineDrawer.points.Count;
            lineDrawer.lineRenderer.SetPosition(lineDrawer.points.Count - 1, position);

            // Update the collider to match the new line
            lineDrawer.edgeCollider.points = ConvertToVector2Array(lineDrawer.points);
        }
    }

    // Convert the list of Vector3 points to an array of Vector2 points for the collider
    private Vector2[] ConvertToVector2Array(List<Vector3> points)
    {
        Vector2[] vector2Array = new Vector2[points.Count];
        for (int i = 0; i < points.Count; i++)
        {
            vector2Array[i] = new Vector2(points[i].x, points[i].y);
        }
        return vector2Array;
    }
}