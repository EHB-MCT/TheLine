// This script handles the collision with TimeIcon objects. Upon collision, it updates the TimeIconManager and Timer, 
// reduces time, and destroys the TimeIcon object.

using UnityEngine;

public class TimeCollisionHandler : MonoBehaviour
{
    // Triggered when the object collides with another collider.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TimeIcon"))  // Check if the collided object is tagged as "TimeIcon"
        {
            Debug.Log("TimeIcon hit! The object will be destroyed.");

            // Check if the TimeIconManager instance is available and add the time icon count
            if (TimeIconManager.Instance != null)
            {
                TimeIconManager.Instance.AddTimeIcon();
            }
            else
            {
                Debug.LogError("TimeIconManager instance is null. Make sure the TimeIconManager is in the scene.");
            }

            // Check if the Timer instance is available and reduce time by 2 seconds
            if (Timer.Instance != null)
            {
                Timer.Instance.ReduceTime(2f);
            }
            else
            {
                Debug.LogError("Timer instance is null. Make sure the Timer is in the scene.");
            }

            // Destroy the TimeIcon object after collision
            Destroy(other.gameObject);
        }
    }
}
