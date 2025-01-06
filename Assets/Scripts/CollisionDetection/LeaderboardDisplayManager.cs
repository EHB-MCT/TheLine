// This script fetches leaderboard data from an API and displays it in the UI using prefabs for each leaderboard entry.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI; // Use Unity UI for legacy Text
using Newtonsoft.Json; // Newtonsoft for JSON deserialization

public class LeaderboardDisplayManager : MonoBehaviour
{
    [SerializeField] private Transform contentParent; // Parent of the leaderboard items (Scroll View Content)
    [SerializeField] private GameObject leaderboardItemPrefab; // Prefab for each leaderboard item
    private const string apiUrl = "http://localhost:5033/api/Leaderboard/rankings"; // API endpoint to fetch leaderboard data

    void Start()
    {
        StartCoroutine(GetLeaderboardData()); // Start the coroutine to fetch leaderboard data
    }

    // Coroutine to fetch leaderboard data from the API
    IEnumerator GetLeaderboardData()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl); // Create a GET request to the API

        yield return request.SendWebRequest(); // Wait for the request to complete

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            // Log error if the request fails
            Debug.LogError("Error fetching leaderboard: " + request.error);
        }
        else
        {
            // Process the JSON response from the API
            string jsonResponse = request.downloadHandler.text; 
            Debug.Log("JSON Response received: " + jsonResponse); // Log the full JSON response

            List<LeaderboardEntry> leaderboardEntries = JsonConvert.DeserializeObject<List<LeaderboardEntry>>(jsonResponse); // Deserialize the JSON response to leaderboard entries

            if (leaderboardEntries != null)
            {
                Debug.Log($"Number of leaderboard entries: {leaderboardEntries.Count}"); // Log the number of entries received
            }
            else
            {
                Debug.LogError("Failed to convert JSON into leaderboard entries.");
            }

            // Create UI items for each leaderboard entry
            foreach (var entry in leaderboardEntries)
            {
                CreateLeaderboardItem(entry); // Call method to create UI item for each leaderboard entry
            }
        }
    }

    // Method to create a UI item for a leaderboard entry
    private void CreateLeaderboardItem(LeaderboardEntry entry)
    {
        GameObject item = Instantiate(leaderboardItemPrefab, contentParent); // Instantiate a new item from the prefab

        // Find all Text components in the prefab and set their values
        Text[] texts = item.GetComponentsInChildren<Text>(); // Use legacy Unity UI Text components
        texts[0].text = $"{entry.Rank}"; // Set rank
        texts[1].text = entry.Username;    // Set username
        texts[2].text = $"level {entry.HighestLevelReached}"; // Set highest level reached
        texts[3].text = $"{entry.Minutes}min {entry.Seconds}sec {entry.Milliseconds}ms"; // Set time in minutes, seconds, and milliseconds

        // Log the created leaderboard item details for debugging
        Debug.Log($"Leaderboard item created: {texts[0].text}, {texts[1].text}, {texts[2].text}, {texts[3].text}");
    }

    // Serializable class for each leaderboard entry, to map the JSON data
    [System.Serializable]
    public class LeaderboardEntry
    {
        [JsonProperty("rank")]
        public int Rank;

        [JsonProperty("username")]
        public string Username;

        [JsonProperty("highestLevelReached")]
        public int HighestLevelReached;

        [JsonProperty("minutes")]
        public int Minutes;

        [JsonProperty("seconds")]
        public int Seconds;

        [JsonProperty("milliseconds")]
        public int Milliseconds;
    }

    // Helper class for JSON deserialization if required (wrapper for lists)
    public static class JsonHelper
    {
        public static List<T> FromJson<T>(string json)
        {
            string newJson = "{ \"Items\": " + json + "}"; // Wrap the JSON in an "Items" object for proper deserialization
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson); // Deserialize the JSON into a wrapper object
            return wrapper.Items; // Return the deserialized list of items
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public List<T> Items; // List of items inside the wrapper
        }
    }
}
