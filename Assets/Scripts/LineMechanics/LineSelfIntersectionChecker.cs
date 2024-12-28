using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineDrawer))]
public class LineSelfIntersectionChecker : MonoBehaviour
{
    private LineDrawer lineDrawer;
    private bool isIntersected = false;

    [SerializeField] private StopGame stopGame; // Verwijzing naar het StopGame-script

    void Start()
    {
        lineDrawer = GetComponent<LineDrawer>();
        if (lineDrawer == null)
        {
            Debug.LogError("LineDrawer component missing on this GameObject.");
        }

        if (stopGame == null)
        {
            Debug.LogError("StopGame script is not assigned in the Inspector!");
        }
    }

    void Update()
    {
        if (lineDrawer != null && !isIntersected)
        {
            List<Vector3> points = lineDrawer.GetPoints();

            if (CheckForSelfIntersection(points))
            {
                Debug.Log("GAME LOST! Line self-intersected.");
                lineDrawer.StopDrawing();
                isIntersected = true;
                EndGame(); // Stop game when self-intersection is detected
            }
        }
    }

    public bool CheckForSelfIntersection(List<Vector3> points)
    {
        if (points.Count < 2)
        {
            return false;
        }

        Vector3 lastPoint = points[points.Count - 1];

        for (int i = 0; i < points.Count - 1; i++)
        {
            if (Vector3.Distance(lastPoint, points[i]) < 0.1f)
            {
                return true;
            }
        }

        return false;
    }

    public void ResetIntersection()
    {
        isIntersected = false;
    }

    void EndGame()
    {
        Debug.Log("GAME OVER - You lost!");

        // Roep de methode aan om de game te stoppen
        if (stopGame != null)
        {
            stopGame.StopGameProcess(); // Stop de game door de tijd stil te zetten
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Line"))
        {
            Debug.Log("Lijn raakt zichzelf! GAME LOST.");
            lineDrawer.StopDrawing();
            EndGame(); // Stop game when self-intersection happens
        }
    }
}
