// This script handles the player login process, validating the user's credentials and retrieving leaderboard data after a successful login.
// It communicates with a server API to authenticate the player and update their leaderboard information.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    [Header("UI Elements")]
    public InputField usernameField;  // Input field for username
    public InputField passwordField;  // Input field for password
    public Button loginButton;        // Button to trigger login
    public Text feedbackText;         // Text field to display feedback to the user

    private string loginUrl = "http://localhost:5033/api/player/login";  // URL for login API
    public static bool IsLoggedIn { get; private set; } = false; // Static flag to track login status
    public static string LastFeedbackMessage { get; private set; } = ""; // Persistent feedback message

    void Start()
    {
        // Restore last feedback message when the scene loads
        if (!string.IsNullOrEmpty(LastFeedbackMessage))
        {
            feedbackText.text = LastFeedbackMessage;
        }
        else
        {
            feedbackText.text = ""; // Clear feedback if no previous message
        }

        loginButton.onClick.AddListener(() => StartCoroutine(LoginProcess()));  // Attach login process to button click
    }

    IEnumerator LoginProcess()
    {
        // Check if username or password is empty
        if (string.IsNullOrEmpty(usernameField.text) || string.IsNullOrEmpty(passwordField.text))
        {
            feedbackText.text = "Username and password cannot be empty!";
            LastFeedbackMessage = feedbackText.text; 
            yield break;  // Exit the coroutine if fields are empty
        }

        // Prepare login data to send to the server
        string json = JsonUtility.ToJson(new PlayerLoginData
        {
            username = usernameField.text,
            password = passwordField.text
        });

        // Send login request to the server
        using (UnityWebRequest request = new UnityWebRequest(loginUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();  // Wait for the response

            if (request.result == UnityWebRequest.Result.Success)
            {
                feedbackText.text = "Login successful!";
                LastFeedbackMessage = feedbackText.text;

                try
                {
                    // Parse the server response and log the successful login
                    PlayerLoginResponse response = JsonUtility.FromJson<PlayerLoginResponse>(request.downloadHandler.text);
                    Debug.Log($"Login successful for {response.username}! PlayerID: {response.playerId}, HighestLevelReached: {response.highestLevelReached}, Time: {response.minutes:00}:{response.seconds:00}.{response.milliseconds:000}");

                    // Set leaderboard data using the response
                    LeaderboardManager.Instance.SetLeaderboardData(
                        response.playerId, 
                        response.username, 
                        response.highestLevelReached,
                        response.minutes,
                        response.seconds,
                        response.milliseconds
                    );
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Failed to parse response: " + ex.Message);  // Handle any errors in parsing the response
                }

                IsLoggedIn = true;
            }
            else
            {
                // Display error feedback if the login fails
                feedbackText.text = "Login failed: " + request.responseCode;
                LastFeedbackMessage = feedbackText.text;
                Debug.LogError("Error: " + request.error);
            }
        }
    }

    [System.Serializable]
    public class PlayerLoginResponse
    {
        public string message;
        public string playerId;
        public string username;
        public int highestLevelReached;
        public int minutes;   // Minutes spent on highest level
        public int seconds;   // Seconds spent on highest level
        public int milliseconds;  // Milliseconds spent on highest level
    }

    [System.Serializable]
    public class PlayerLoginData
    {
        public string username;  // Username for login
        public string password;  // Password for login
    }
}