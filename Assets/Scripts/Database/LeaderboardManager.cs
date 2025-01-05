using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance { get; private set; }

    public string PlayerId { get; private set; }
    public int HighestLevelReached { get; private set; }
    public float TimeForHighestLevel { get; private set; } // Tijd voor de hoogste level
    public int LastCompletedLevel { get; private set; } // Laatst volledig voltooide level
    public float TimeForLastCompletedLevel { get; private set; } // Tijd voor de laatst voltooide level

    public int MinutesForHighestLevel { get; private set; } // Minuten voor hoogste level
    public int SecondsForHighestLevel { get; private set; } // Seconden voor hoogste level
    public int MillisecondsForHighestLevel { get; private set; } // Milliseconden voor hoogste level

    public string Username { get; private set; }

    private string updateLevelUrl = "http://localhost:5033/api/leaderboard/update-highest-level";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Blijf bestaan tussen scènes
        }
        else
        {
            Destroy(gameObject);
        }
    }
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

    public void UpdateLastCompletedLevel(int attemptedLevel, float timeElapsed)
    {
        // Het laatst volledig voltooide level is het level vóór het huidige
        LastCompletedLevel = attemptedLevel - 1;
        TimeForLastCompletedLevel = timeElapsed;

        Debug.Log($"Last completed level updated: {LastCompletedLevel} at {TimeForLastCompletedLevel} seconds.");
    }


    public void UpdateHighestLevel(int completedLevel, float timeElapsed)
    {
        if (completedLevel > HighestLevelReached)
        {
            HighestLevelReached = completedLevel - 1;
            TimeForHighestLevel = timeElapsed;
            Debug.Log($"New highest level reached: {HighestLevelReached} at {TimeForHighestLevel} seconds.");
        }

        // Update het laatst volledig voltooide level
        UpdateLastCompletedLevel(completedLevel, timeElapsed);
    }


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
                onStatsRetrieved?.Invoke(null); // Stuur null als de aanvraag mislukt
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
