/*
 * This script checks if the mouse is hovering over a designated start point in the UI.
 * It uses a RectTransform for the start point and converts screen coordinates to local space.
 */

using UnityEngine;

public class StartPointHandler : MonoBehaviour
{
    public RectTransform startPoint; // Use a RectTransform for the start point
    private Canvas canvas;

    void Start()
    {
        canvas = startPoint.GetComponentInParent<Canvas>(); // Get the Canvas (needed for conversion)
    }

    // Check if the mouse is over the start point
    public bool IsMouseOverStartPoint()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out mousePos
        );

        // Check if the mouse is within the bounds of the RectTransform
        return RectTransformUtility.RectangleContainsScreenPoint(startPoint, Input.mousePosition, canvas.worldCamera);
    }
}