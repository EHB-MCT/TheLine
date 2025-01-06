// This script handles the completion of a level, including updating player stats through an API call.

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LevelCompletionHandler : MonoBehaviour
{
    // Method to handle level completion
    public void OnLevelComplete(int level)
    {
        // Check if the player is logged in
        if (!Login.IsLoggedIn)
        {
            Debug.LogError("Player is not logged in. Cannot update stats.");
            return;
        }

        // Retrieve the player ID from the LeaderboardManager
        string playerId = LeaderboardManager.Instance.PlayerId;

        Debug.Log($"Level {level} completed.");

        // Call the method to update the player's stats for the completed level
        StartCoroutine(UpdateStats(playerId, level));
    }

    // Coroutine to update the player's stats via an API call
    private IEnumerator UpdateStats(string playerId, int level)
    {
        // API URL for updating stats
        string updateStatsUrl = "http://localhost:5033/api/playerstats/update-stats";

        // Create the request data to be sent
        UpdateStatsRequest statsRequest = new UpdateStatsRequest
        {
            playerId = playerId,
            level = level,
            playsToAdd = 1 // Increment the play count for the level
        };

        // Serialize the data into JSON
        string json = JsonUtility.ToJson(statsRequest);
        Debug.Log($"Sending update stats request: {json}");

        // Set up the UnityWebRequest to send the data to the API
        using (UnityWebRequest request = new UnityWebRequest(updateStatsUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Wait for the request to complete
            yield return request.SendWebRequest();

            // Handle the response from the API
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

    // Request structure for updating stats
    [System.Serializable]
    public class UpdateStatsRequest
    {
        public string playerId; // The player's ID
        public int level; // The level that was completed
        public int playsToAdd; // The number of plays to add
    }
}
