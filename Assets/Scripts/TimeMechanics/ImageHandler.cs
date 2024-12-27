using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageHandler : MonoBehaviour
{
    public float timeReduction = 2f; // Tijd in seconden die van de timer wordt afgetrokken

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detected with: " + other.gameObject.name);

        if (other.gameObject.CompareTag("Line"))
        {
            Debug.Log("Line triggered the image!");
            // Trek tijd af van de timer via het TimerScript
            if (TimerScript.Instance != null)
            {
                TimerScript.Instance.ReduceTime(timeReduction);
            }

            // Verwijder het object
            Destroy(gameObject);
        }
    }
}

