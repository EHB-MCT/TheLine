using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollisionChecker : MonoBehaviour
{
    [SerializeField] private StopGame stopGame; // Verwijzing naar StopGame
    [SerializeField] private LineDrawer lineDrawer; // Verwijzing naar de LineDrawer

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision met object: " + other.name);

        if (other.CompareTag("obstacle")) // Controleer of het object de tag 'obstacle' heeft
        {
            Debug.Log("Obstacle geraakt!"); // Console-log wanneer een obstacle wordt geraakt
            HandleObstacleCollision();
        }

        // Check of het object de tag "Image" heeft
        if (other.CompareTag("Image"))
        {
            Debug.Log("Image geraakt! Het object wordt vernietigd.");
            
            // Trek 2 seconden af van de tijd
            TimerScript.Instance.ReduceTime(2f); // Verminder de tijd met 2 seconden
            
            // Vernietig het object dat de tag "Image" heeft
            Destroy(other.gameObject); // Vernietig het object
        }
    }

    void HandleObstacleCollision()
    {
        Debug.Log("GAME OVER - Je hebt een obstacle geraakt.");

        // Roep de StopGame methode aan om de game te stoppen
        if (stopGame != null)
        {
            stopGame.StopGameProcess(); // Roep de StopGameProcess aan om het spel te stoppen
        }

        // Stop het tekenen van de lijn
        if (lineDrawer != null)
        {
            lineDrawer.StopDrawing(); // Stop het tekenen van de lijn
        }
    }
}
