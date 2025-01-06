using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ObstacleCollisionChecker : MonoBehaviour
{
    [SerializeField] private StopGame stopGame; // Reference to the StopGame script
    [SerializeField] private LineDrawer lineDrawer; // Reference to the LineDrawer script
    private float startTime; // Houd de starttijd bij

    void Start()
    {
        // Stel de starttijd in bij het begin van het spel of level.
        startTime = Time.time;

        // Controleer of de vereiste componenten zijn toegewezen.
        if (stopGame == null)
        {
            Debug.LogError("StopGame script is not assigned in the Inspector!");
        }
        if (lineDrawer == null)
        {
            Debug.LogError("LineDrawer script is not assigned in the Inspector!");
        }
    }
void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("obstacle"))
    {
        Debug.Log("Obstacle hit!");

        // Bereken verstreken tijd
        float timeElapsed = Time.time - startTime;

        // Update PlayerStats via API
        string playerId = LeaderboardManager.Instance.PlayerId;
        int currentLevel = LeaderboardManager.Instance.LastCompletedLevel + 1; // Stel het huidige level in

        StartCoroutine(UpdateStatsForObstacleDeath(playerId, currentLevel));

        // Stop the game and line drawing
        stopGame?.StopGameProcess(lineDrawer, timeElapsed);
    }
}

    private IEnumerator UpdateStatsForObstacleDeath(string playerId, int level)
    {
        string updateStatsUrl = "http://localhost:5033/api/playerstats/update-stats";

        var statsRequest = new PlayerStatsUpdateRequest
        {
            playerId = playerId,
            level = level,
            playsToAdd = 0, // Geen nieuwe play toevoegen
            deathsByLine = 0,
            deathsByObstacles = 1, // 1 dood door obstakel
            timeIconsCollected = 0
        };

        string json = JsonUtility.ToJson(statsRequest);

        using (UnityWebRequest request = new UnityWebRequest(updateStatsUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Updated obstacle death in PlayerStats.");
            }
            else
            {
                Debug.LogError($"Failed to update obstacle death: {request.error}");
            }
        }
    }

    [System.Serializable]
    public class PlayerStatsUpdateRequest
    {
        public string playerId;
        public int level;
        public int playsToAdd;
        public int deathsByLine;
        public int deathsByObstacles;
        public int timeIconsCollected;
    }
}
