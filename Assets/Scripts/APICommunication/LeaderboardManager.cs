// This script manages the leaderboard data for a player, including the highest level reached and the time spent.
// It also provides methods to update the leaderboard data in the database and retrieve the player's leaderboard stats.

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance { get; private set; }

    public string PlayerId { get; private set; }
    public string Username { get; private set; }
    public int HighestLevelReached { get; private set; }
    public float TimeForHighestLevel { get; private set; } // Time for highest level
    public int LastCompletedLevel { get; private set; } // Last fully completed level
    public float TimeForLastCompletedLevel { get; private set; } // Time for last completed level

    public int MinutesForHighestLevel { get; private set; } // Minutes for highest level
    public int SecondsForHighestLevel { get; private set; } // Seconds for highest level
    public int MillisecondsForHighestLevel { get; private set; } // Milliseconds for highest level

    private string updateLevelUrl = "http://localhost:5033/api/leaderboard/update-highest-level";

    // Ensures only one instance of LeaderboardManager exists across scenes.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Retain across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Set leaderboard data after login.
    public void SetLeaderboardData(string playerId, string username, int highestLevelReached, int minutes, int seconds, int milliseconds)
    {
        PlayerId = playerId;
        Username = username;
        HighestLevelReached = highestLevelReached;
        MinutesForHighestLevel = minutes;
        SecondsForHighestLevel = seconds;
        MillisecondsForHighestLevel = milliseconds;

        Debug.Log($"LeaderboardManager initialized: " +
                  $"PlayerId={playerId}, Username={username}, " +
                  $"HighestLevelReached={highestLevelReached}, Time={minutes:00}:{seconds:00}.{milliseconds:000}");
    }

    // Update the last completed level and its time.
    public void UpdateLastCompletedLevel(int completedLevel, float timeElapsed)
    {
        LastCompletedLevel = completedLevel;
        TimeForLastCompletedLevel = timeElapsed;

        Debug.Log($"Last completed level updated: {LastCompletedLevel} at {TimeForLastCompletedLevel} seconds.");
    }

    // Update the highest level reached and its time if higher than the current level.
    public void UpdateHighestLevel(int completedLevel, float timeElapsed)
    {
        if (completedLevel > HighestLevelReached)
        {
            HighestLevelReached = completedLevel;
            TimeForHighestLevel = timeElapsed;

            Debug.Log($"New highest level reached: {HighestLevelReached} at {TimeForHighestLevel} seconds.");

            // Start a coroutine to update the database with the new highest level and time.
            StartCoroutine(UpdateHighestLevelAndTimeInDatabase(timeElapsed, completedLevel));
        }

        // Update the last completed level data as well.
        UpdateLastCompletedLevel(completedLevel, timeElapsed);
    }

    // Update the highest level and completion time in the database.
    public IEnumerator UpdateHighestLevelAndTimeInDatabase(float timeElapsed, int achievedLevel)
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);
        int milliseconds = Mathf.FloorToInt((timeElapsed * 1000) % 1000);

        var json = JsonUtility.ToJson(new UpdateTimeRequest
        {
            PlayerId = PlayerId,
            NewLevel = achievedLevel,
            Minutes = minutes,
            Seconds = seconds,
            Milliseconds = milliseconds
        });

        using (UnityWebRequest request = new UnityWebRequest(updateLevelUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Highest level and completion time updated successfully in the database.");
            }
            else
            {
                Debug.LogError($"Failed to update highest level and time: {request.error}");
            }
        }
    }

    // Retrieve the player's leaderboard stats from the database.
    public IEnumerator GetLeaderboardFromDatabase(System.Action<Leaderboard> onStatsRetrieved)
    {
        string url = $"http://localhost:5033/api/leaderboard/{PlayerId}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var leaderboard = JsonUtility.FromJson<Leaderboard>(request.downloadHandler.text);
                onStatsRetrieved?.Invoke(leaderboard);
            }
            else
            {
                Debug.LogError($"Failed to retrieve leaderboard: {request.error}");
                onStatsRetrieved?.Invoke(null); // Send null if request fails
            }
        }
    }

    [System.Serializable]
    public class UpdateTimeRequest
    {
        public string PlayerId;
        public int NewLevel;
        public int Minutes;
        public int Seconds;
        public int Milliseconds;
    }

    [System.Serializable]
    public class Leaderboard
    {
        public string PlayerId;
        public int HighestLevelReached;
        public int Minutes;
        public int Seconds;
        public int Milliseconds;
    }
}
