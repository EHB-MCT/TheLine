/*
 * This script handles the game timer, keeping track of elapsed time, displaying it on the UI,
 * and providing functions to reset and reduce the timer.
 * It also follows the Singleton pattern to ensure there is only one active timer instance.
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    private float timeElapsed;  // Keeps track of the elapsed time
    public Text timerText;      // UI Text element to display the timer

    // Singleton pattern to ensure only one instance of the Timer exists
    public static Timer Instance { get; private set; }

    void Awake()
    {
        // Ensure only one instance of the Timer script exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Prevents this object from being destroyed when loading a new scene
        }
        else
        {
            Destroy(gameObject);  // Destroys this object if another instance already exists
        }
    }

    void Start()
    {
        if (Instance == this)  // Start the timer only if this is the active instance
        {
            timeElapsed = 0f;  // Initialize the timer to 0 when starting the game
        }
    }

    void Update()
    {
        if (Instance == this)  // Only update the timer if this is the active instance
        {
            string currentSceneName = SceneManager.GetActiveScene().name;

            // Stop the timer if we are on the main menu or leaderboard scene
            if (currentSceneName == "MainMenu" || currentSceneName == "Leaderboard") 
            {
                return;  // Stop the timer on the main menu or leaderboard screen
            }

            // If we're in the game, keep track of the time
            timeElapsed += Time.deltaTime;  // Increase the time by the elapsed time per frame

            // Calculate minutes, seconds, and milliseconds
            int minutes = Mathf.FloorToInt(timeElapsed / 60);
            int seconds = Mathf.FloorToInt(timeElapsed % 60);
            int milliseconds = Mathf.FloorToInt((timeElapsed * 1000) % 1000);

            // Update the UI text with the new time
            timerText.text = string.Format("{0:D2}:{1:D2}:{2:D3}", minutes, seconds, milliseconds);
        }
    }

    // Method to reset the timer to 0
    public void ResetTimer()
    {
        timeElapsed = 0f;  // Set the time back to 0
        timerText.text = "00:00:000";  // Set the UI text to 0 as well
    }

    // Method to reduce the timer by a specified amount
    public void ReduceTime(float amount)
    {
        timeElapsed -= amount;  // Subtract the given amount of time
        if (timeElapsed < 0f)
        {
            timeElapsed = 0f;  // Ensure the timer doesn't go negative
        }
    }

    // Method to get the elapsed time
    public float GetElapsedTime()
    {
        return timeElapsed;  // Return the elapsed time
    }
}
