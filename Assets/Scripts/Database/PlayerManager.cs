using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public string PlayerId { get; private set; }
    public int HighestLevelReached { get; private set; }

    private string updateLevelUrl = "http://localhost:5033/api/playerstats/update-highest-level";

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

    public void SetPlayerData(string playerId, int highestLevelReached)
    {
        PlayerId = playerId;
        HighestLevelReached = highestLevelReached;
        Debug.Log($"PlayerManager initialized: PlayerId={PlayerId}, HighestLevelReached={HighestLevelReached}");
    }

    public void UpdateHighestLevel(int newLevel, float timeElapsed)
    {
        if (newLevel > HighestLevelReached)
        {
            HighestLevelReached = newLevel;
            Debug.Log($"New highest level reached: {HighestLevelReached}");
            StartCoroutine(UpdateHighestLevelAndTimeInDatabase(timeElapsed));
        }
        else
        {
            Debug.Log($"Level {newLevel} completed, but it's not a new highest level.");
        }
    }

    private IEnumerator UpdateHighestLevelAndTimeInDatabase(float timeElapsed)
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);
        int milliseconds = Mathf.FloorToInt((timeElapsed * 1000) % 1000);

        var json = JsonUtility.ToJson(new UpdateTimeRequest
        {
            PlayerId = PlayerId,
            NewLevel = HighestLevelReached,
            Minutes = minutes,
            Seconds = seconds,
            Milliseconds = milliseconds
        });

        Debug.Log($"Sending JSON: {json}"); // Log de JSON voor debugging

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
                Debug.LogError($"Response: {request.downloadHandler.text}"); // Log de response van de server
                Debug.Log($"Sending JSON: {json}");
            }
        }
    }
    [System.Serializable]
    public class UpdateTimeRequest
    {
        public string PlayerId;
        public int NewLevel; // Verander van LevelCompleted naar HighestLevelReached
        public int Minutes;
        public int Seconds;
        public int Milliseconds;
    }
}
