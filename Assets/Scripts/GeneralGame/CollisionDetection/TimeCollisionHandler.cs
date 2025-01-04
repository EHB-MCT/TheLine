/*
 * This script handles collisions with objects tagged as "TimeIcon". 
 * On collision, it reduces the remaining time by 2 seconds and destroys the object.
 */

using UnityEngine;

public class TimeCollisionHandler : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object has the tag "TimeIcon".
        if (other.CompareTag("TimeIcon"))
        {
            Debug.Log("TimeIcon hit! The object will be destroyed.");

            // Reduce the remaining time by 2 seconds via the Timer.
            Timer.Instance.ReduceTime(2f);

            // Destroy the colliding object.
            Destroy(other.gameObject);
        }
    }
}