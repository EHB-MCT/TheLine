/*
 * This script checks if the line collides with an obstacle.
 * If a collision occurs with an object tagged as 'obstacle', the game is stopped using the StopGame script.
 */

using UnityEngine;

public class ObstacleCollisionChecker : MonoBehaviour
{
    [SerializeField] private StopGame stopGame; // Reference to the StopGame script
    [SerializeField] private LineDrawer lineDrawer; // Reference to the LineDrawer script

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object is an 'obstacle' based on its tag.
        if (other.CompareTag("obstacle"))
        {
            Debug.Log("Obstacle hit!"); // Log that an obstacle has been hit.

            // Stop the game and line drawing.
            stopGame?.StopGameProcess(lineDrawer);
        }
    }
}
