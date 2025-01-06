using UnityEngine;

public class TimeCollisionHandler : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TimeIcon"))
        {
            Debug.Log("TimeIcon hit! The object will be destroyed.");

            // Verlaag de resterende tijd met 2 seconden
            Timer.Instance.ReduceTime(2f);

            // Verhoog de teller in de TimeIconManager
            TimeIconManager.Instance.AddTimeIcon();

            // Vernietig het TimeIcon-object
            Destroy(other.gameObject);
        }
    }
}
