// This script checks for self-intersections in a drawn line, stops the game when an intersection is detected, and updates player stats accordingly.
// It tracks the time elapsed, sends player stats to the API, and resets the intersection status when needed.

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic; // For the List<T> type

[RequireComponent(typeof(LineDrawer))]
public class LineSelfIntersectionChecker : MonoBehaviour
{
    private LineDrawer lineDrawer;  // Reference to the LineDrawer component for managing the drawn line
    private bool isIntersected = false;  // Flag to track if a self-intersection has occurred

    [SerializeField] private StopGame stopGame;  // Reference to the StopGame script to stop the game when necessary
    private float startTime;  // Time when the line drawing starts

    void Start()
    {
        // Retrieve the LineDrawer component from the GameObject and verify its existence.
        lineDrawer = GetComponent<LineDrawer>();
        if (lineDrawer == null)
        {
            Debug.LogError("LineDrawer component is missing on this GameObject.");
        }

        // Verify that the StopGame script is properly assigned in the Inspector.
        if (stopGame == null)
        {
            Debug.LogError("StopGame script is not assigned in the Inspector!");
        }

        // Set the start time when the game begins.
        startTime = Time.time;
    }

    void Update()
    {
        if (lineDrawer != null && !isIntersected)
        {
            List<Vector3> points = lineDrawer.GetPoints();

            // Check if the line has self-intersected
            if (CheckForSelfIntersection(points))
            {
                Debug.Log("GAME OVER! The line has self-intersected.");
                isIntersected = true;

                // Calculate the elapsed time since the start
                float timeElapsed = Time.time - startTime;

                // Update PlayerStats via API with the new death information
                string playerId = LeaderboardManager.Instance.PlayerId;
                int currentLevel = LeaderboardManager.Instance.LastCompletedLevel + 1;  // Set the current level

                // Update player stats and stop the game
                StartCoroutine(UpdateStatsForLineIntersection(playerId, currentLevel));

                // Stop the game process
                stopGame?.StopGameProcess(lineDrawer, timeElapsed);
            }
        }
    }

    // Update the player stats when a line intersection occurs
    private IEnumerator UpdateStatsForLineIntersection(string playerId, int level)
    {
        string updateStatsUrl = "http://localhost:5033/api/playerstats/update-stats";

        var statsRequest = new PlayerStatsUpdateRequest
        {
            playerId = playerId,
            level = level,
            playsToAdd = 0,  // No new plays added
            deathsByLine = 1,  // 1 death caused by line intersection
            deathsByObstacles = 0,
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
                Debug.Log("Updated line intersection death in PlayerStats.");
            }
            else
            {
                Debug.LogError($"Failed to update line intersection death: {request.error}");
            }
        }
    }

    // Check if the line intersects itself based on a list of points
    public bool CheckForSelfIntersection(List<Vector3> points)
    {
        if (points.Count < 2)
        {
            return false;  // Not enough points to cause a self-intersection
        }

        Vector3 lastPoint = points[points.Count - 1];

        // Compare the last point with previous points in the list for intersection
        for (int i = 0; i < points.Count - 1; i++)
        {
            if (Vector3.Distance(lastPoint, points[i]) < 0.1f)  // Threshold for precision in intersection
            {
                return true;  // Intersection detected
            }
        }

        return false;  // No intersection found
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

    // Reset the intersection status to allow a new line to be drawn without issues
    public void ResetIntersection()
    {
        isIntersected = false;  // Reset the self-intersection flag
    }
}
