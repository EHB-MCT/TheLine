// This script manages the collection of "time icons" in the game and maintains the count across scenes.
// It also allows for resetting the data when necessary, ensuring persistent data during gameplay.

using UnityEngine;

public class LevelDataManager : MonoBehaviour
{
    public static LevelDataManager Instance { get; private set; }

    public int TimeIconsCollected { get; private set; } = 0;

    // Ensures that only one instance of LevelDataManager exists and is preserved between scenes.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Retain across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // Increases the count of collected time icons by one.
    public void AddTimeIcon()
    {
        TimeIconsCollected++;
        Debug.Log($"TimeIconsCollected: {TimeIconsCollected}");
    }

    // Resets the collected time icons count to zero.
    public void ResetData()
    {
        TimeIconsCollected = 0;
        Debug.Log("Level data reset.");
    }
}
