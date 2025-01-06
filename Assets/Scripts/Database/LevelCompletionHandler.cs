using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LevelCompletionHandler : MonoBehaviour
{
    public void OnLevelComplete(int level)
    {
        if (!Login.IsLoggedIn)
        {
            Debug.LogError("Player is not logged in. Cannot update stats.");
            return;
        }

        string playerId = LeaderboardManager.Instance.PlayerId;

        Debug.Log($"Level {level} completed.");

        // Stuur alleen het aantal keer dat een level is gespeeld
        StartCoroutine(UpdateStats(playerId, level));
    }

    private IEnumerator UpdateStats(string playerId, int level)
    {
        string updateStatsUrl = "http://localhost:5033/api/playerstats/update-stats";

        UpdateStatsRequest statsRequest = new UpdateStatsRequest
        {
            playerId = playerId,
            level = level,
            playsToAdd = 1 // Verhoog het aantal keer dat het level is gespeeld
        };

        string json = JsonUtility.ToJson(statsRequest);
        Debug.Log($"Sending update stats request: {json}");

        using (UnityWebRequest request = new UnityWebRequest(updateStatsUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Stats updated successfully for Player ID: {playerId}. Response: {request.downloadHandler.text}");
            }
            else
            {
                Debug.LogError($"Failed to update stats: {request.error}");
            }
        }
    }

    [System.Serializable]
    public class UpdateStatsRequest
    {
        public string playerId;
        public int level;
        public int playsToAdd;
    }
}
