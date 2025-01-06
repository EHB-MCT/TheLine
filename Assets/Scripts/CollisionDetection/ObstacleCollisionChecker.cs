// This script handles collisions with obstacles, tracks the time elapsed, stops the game when a collision occurs, 
// and updates the player's stats for deaths caused by obstacles.

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ObstacleCollisionChecker : MonoBehaviour
{
    [SerializeField] private StopGame stopGame;  // Reference to the StopGame script to stop the game when an obstacle is hit
    [SerializeField] private LineDrawer lineDrawer;  // Reference to the LineDrawer script to stop line drawing on collision
    private float startTime;  // Track the start time of the game or level

    void Start()
    {
        // Set the start time when the game or level begins.
        startTime = Time.time;

        // Verify that the required components are properly assigned in the Inspector.
        if (stopGame == null)
        {
            Debug.LogError("StopGame script is not assigned in the Inspector!");
        }
        if (lineDrawer == null)
        {
            Debug.LogError("LineDrawer script is not assigned in the Inspector!");
        }
    }

    // Triggered when the line collides with an obstacle.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("obstacle"))  // Check if the collided object is tagged as "obstacle"
        {
            Debug.Log("Obstacle hit!");

            // Calculate the time elapsed since the game started
            float timeElapsed = Time.time - startTime;

            // Update PlayerStats via API for the death caused by the obstacle
            string playerId = LeaderboardManager.Instance.PlayerId;
            int currentLevel = LeaderboardManager.Instance.LastCompletedLevel + 1;  // Set the current level

            // Update player stats and stop the game
            StartCoroutine(UpdateStatsForObstacleDeath(playerId, currentLevel));

            // Stop the game and line drawing after collision
            stopGame?.StopGameProcess(lineDrawer, timeElapsed);
        }
    }

    // Update the player stats when a death caused by an obstacle occurs
    private IEnumerator UpdateStatsForObstacleDeath(string playerId, int level)
    {
        string updateStatsUrl = "http://localhost:5033/api/playerstats/update-stats";

        var statsRequest = new PlayerStatsUpdateRequest
        {
            playerId = playerId,
            level = level,
            playsToAdd = 0,  // No new plays added
            deathsByLine = 0,  // No death by line
            deathsByObstacles = 1,  // 1 death caused by obstacle
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
