using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI; // Gebruik Unity UI voor legacy Text
using Newtonsoft.Json;

public class LeaderboardDisplayManager : MonoBehaviour
{
    [SerializeField] private Transform contentParent; // Parent van de items (Scroll View Content)
    [SerializeField] private GameObject leaderboardItemPrefab; // Prefab van een enkel item
    private const string apiUrl = "http://localhost:5033/api/Leaderboard/rankings";

    void Start()
    {
        StartCoroutine(GetLeaderboardData());
    }

    IEnumerator GetLeaderboardData()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error fetching leaderboard: " + request.error);
        }
        else
        {
            // Verwerk de JSON-respons
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("JSON Response ontvangen: " + jsonResponse); // Log de volledige JSON-respons

            List<LeaderboardEntry> leaderboardEntries = JsonConvert.DeserializeObject<List<LeaderboardEntry>>(jsonResponse);

            if (leaderboardEntries != null)
            {
                Debug.Log($"Aantal leaderboard entries: {leaderboardEntries.Count}"); // Log het aantal ontvangen items
            }
            else
            {
                Debug.LogError("De JSON kon niet worden geconverteerd naar leaderboard entries.");
            }

            // Maak de UI-items
            foreach (var entry in leaderboardEntries)
            {
                CreateLeaderboardItem(entry);
            }
        }
    }

    private void CreateLeaderboardItem(LeaderboardEntry entry)
    {
        GameObject item = Instantiate(leaderboardItemPrefab, contentParent);

        // Zoek de Text-elementen in de prefab en stel hun waarden in
        Text[] texts = item.GetComponentsInChildren<Text>(); // Gebruik de legacy UI Text-component
        texts[0].text = $"{entry.Rank}"; // Rank
        texts[1].text = entry.Username;    // Naam
        texts[2].text = $"level {entry.HighestLevelReached}"; // Level
        texts[3].text = $"{entry.Minutes}min {entry.Seconds}sec {entry.Milliseconds}ms"; // Tijd

        // Log de gemaakte items en hun informatie
        Debug.Log($"Leaderboard item aangemaakt: {texts[0].text}, {texts[1].text}, {texts[2].text}, {texts[3].text}");
    }

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
    // Helperklasse om JSON te deserialiseren
    public static class JsonHelper
    {
        public static List<T> FromJson<T>(string json)
        {
            string newJson = "{ \"Items\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.Items;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public List<T> Items;
        }
    }
}