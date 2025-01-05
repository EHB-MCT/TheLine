using UnityEngine;

public class ObstacleCollisionChecker : MonoBehaviour
{
    [SerializeField] private StopGame stopGame; // Reference to the StopGame script
    [SerializeField] private LineDrawer lineDrawer; // Reference to the LineDrawer script
    private float startTime; // Houd de starttijd bij

    void Start()
    {
        // Stel de starttijd in bij het begin van het spel of level.
        startTime = Time.time;

        // Controleer of de vereiste componenten zijn toegewezen.
        if (stopGame == null)
        {
            Debug.LogError("StopGame script is not assigned in the Inspector!");
        }
        if (lineDrawer == null)
        {
            Debug.LogError("LineDrawer script is not assigned in the Inspector!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object is an 'obstacle' based on its tag.
        if (other.CompareTag("obstacle"))
        {
            Debug.Log("Obstacle hit!"); // Log that an obstacle has been hit.

            // Bereken de verstreken tijd.
            float timeElapsed = Time.time - startTime;

            // Stop the game and line drawing.
            stopGame?.StopGameProcess(lineDrawer, timeElapsed);
        }
    }
}
