using UnityEngine;

public class TimeCollisionHandler : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TimeIcon"))
        {
            Debug.Log("TimeIcon hit! The object will be destroyed.");

            // Controleer of de instantie van TimeIconManager niet null is
            if (TimeIconManager.Instance != null)
            {
                TimeIconManager.Instance.AddTimeIcon();
            }
            else
            {
                Debug.LogError("TimeIconManager instance is null. Make sure the TimeIconManager is in the scene.");
            }

            // Controleer of de instantie van Timer niet null is
            if (Timer.Instance != null)
            {
                Timer.Instance.ReduceTime(2f);
            }
            else
            {
                Debug.LogError("Timer instance is null. Make sure the Timer is in the scene.");
            }

            // Vernietig het TimeIcon-object
            Destroy(other.gameObject);
        }
    }
}
