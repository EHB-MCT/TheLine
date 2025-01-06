// This script manages the collection and reset of time icons, keeping track of how many icons the player has collected.
// It provides methods to add collected time icons and reset the collection count.

using UnityEngine;

public class TimeIconManager : MonoBehaviour
{
    public static TimeIconManager Instance { get; private set; }  // Singleton instance to access the manager globally

    public int TimeIconsCollected { get; private set; } = 0;  // Tracks the number of collected time icons

    private void Awake()
    {
        // Ensure only one instance of TimeIconManager exists across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Retain across scene changes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances
        }
    }

    // Method to add a collected time icon
    public void AddTimeIcon()
    {
        TimeIconsCollected++;  // Increment the collected icon count
        Debug.Log($"TimeIconsCollected: {TimeIconsCollected}");  // Log the current count
    }

    // Method to reset the collected time icons count
    public void ResetTimeIcons()
    {
        TimeIconsCollected = 0;  // Reset the count
        Debug.Log("Time icon data reset.");  // Log the reset action
    }
}