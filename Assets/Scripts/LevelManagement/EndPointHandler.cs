/*
 * This script checks if a line, represented by a list of points, is touching a designated endpoint.
 * It uses a RectTransform for the endpoint and converts world points to screen space to perform the check.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointHandler : MonoBehaviour
{
    public RectTransform endPoint; // Use a RectTransform for the endpoint
    private Canvas canvas;

    void Start()
    {
        canvas = endPoint.GetComponentInParent<Canvas>(); // Get the Canvas (needed for conversion)
    }

    // Check if the line, represented by a list of points, is touching the endpoint
    public bool IsLineTouchingEndPoint(List<Vector3> points)
    {
        foreach (Vector3 point in points)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(point); // Convert world point to screen space

            // Check if the point is within the RectTransform bounds
            if (RectTransformUtility.RectangleContainsScreenPoint(endPoint, screenPoint, canvas.worldCamera))
            {
                return true;
            }
        }
        return false;
    }
}