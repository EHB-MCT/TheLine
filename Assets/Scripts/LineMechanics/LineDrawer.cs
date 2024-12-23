using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(EdgeCollider2D))]
public class LineDrawer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider; // EdgeCollider voor detectie
    private List<Vector3> points;
    private bool isDrawing = false;
    private bool isLineStarted = false;
    private LineSelfIntersectionChecker intersectionChecker;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        intersectionChecker = GetComponent<LineSelfIntersectionChecker>();

        if (lineRenderer == null || edgeCollider == null)
        {
            Debug.LogError("Required components missing on this GameObject.");
            return;
        }

        points = new List<Vector3>();

        // Configureer de LineRenderer
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.useWorldSpace = true;

        edgeCollider.isTrigger = true; // Zorg dat de collider een trigger is
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDrawing && IsMouseOverStartPoint())
        {
            StartDrawing(Vector3.zero); // Begin op het startpunt
            intersectionChecker.ResetIntersection(); // Reset de intersectiestatus
        }

        if (Input.GetMouseButton(0) && isDrawing)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f; // Afstand van de camera
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            UpdateLine(worldPos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDrawing)
            {
                // Stop met tekenen als er een intersectie is
                if (intersectionChecker.CheckForSelfIntersection(points))
                {
                    Debug.Log("Line intersected, clearing line.");
                    StopDrawing(); // Stop drawing when the line intersects itself
                    // Don't allow further drawing until the mouse is released and the player returns to start point
                }
            }
        }
    }

    private bool IsMouseOverStartPoint()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // Afstand van de camera
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // Controleer of de muis binnen een straal van 0.5 van het startpunt is
        return Vector3.Distance(worldPos, Vector3.zero) <= 0.5f; // Startpunt (0,0)
    }

    public void StartDrawing(Vector3 startPosition)
    {
        isDrawing = true;
        isLineStarted = true;
        points.Clear();
        lineRenderer.positionCount = 0;

        points.Add(startPosition);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(0, startPosition);

        // Reset de collider
        edgeCollider.points = new Vector2[0]; // Reset de collider voordat je nieuwe lijn start
    }

    public void UpdateLine(Vector3 position)
    {
        if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], position) > 0.1f)
        {
            points.Add(position);
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPosition(points.Count - 1, position);

            // Update de collider met de nieuwe lijn
            edgeCollider.points = ConvertToVector2Array(points);
        }
    }

    public void StopDrawing()
    {
        isDrawing = false;
        points.Clear();
        lineRenderer.positionCount = 0;

        // Reset de collider wanneer de lijn wordt gestopt
        edgeCollider.points = new Vector2[0];
    }

    public List<Vector3> GetPoints()
    {
        return points;
    }

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