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
        // Log the name of the object involved in the collision.
        Debug.Log("Collision with object: " + other.name);

        // Check if the object is an 'obstacle' based on its tag.
        if (other.CompareTag("obstacle"))
        {
            Debug.Log("Obstacle hit!"); // Indicate that an obstacle has been hit.

            // Call StopGameProcess to stop the game and line drawing.
            if (stopGame != null)
            {
                stopGame.StopGameProcess(lineDrawer);
            }
        }
    }
}