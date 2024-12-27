using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollisionChecker : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("obstacle")) // Controleer of het object de tag 'obstacle' heeft
        {
            Debug.Log("Obstacle geraakt!"); // Console-log wanneer een obstacle wordt geraakt
            HandleObstacleCollision();
        }
    }

    void HandleObstacleCollision()
    {
        // Voeg hier je logica toe voor wat er gebeurt als een obstacle wordt geraakt
        Debug.Log("GAME OVER - Je hebt een obstacle geraakt.");
        
        // Voorbeeld: toon een game over UI of stop verdere input
        // Mogelijk kun je hier ook een andere functie aanroepen
    }
}

