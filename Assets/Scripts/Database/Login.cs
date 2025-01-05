using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    [Header("UI Elements")]
    public InputField usernameField;
    public InputField passwordField;
    public Button loginButton;
    public Text feedbackText;

    private string loginUrl = "http://localhost:5033/api/player/login";
    public static bool IsLoggedIn { get; private set; } = false; // Statische vlag
    public static string LastFeedbackMessage { get; private set; } = ""; // Persistent feedbackbericht

    void Start()
    {
        // Herstel de laatste feedbacktekst bij het laden van de scène
        if (!string.IsNullOrEmpty(LastFeedbackMessage))
        {
            feedbackText.text = LastFeedbackMessage;
        }
        else
        {
            feedbackText.text = ""; // Leegmaken als er geen eerdere feedback is
        }

        loginButton.onClick.AddListener(() => StartCoroutine(LoginProcess()));
    }

    IEnumerator LoginProcess()
    {
        if (string.IsNullOrEmpty(usernameField.text) || string.IsNullOrEmpty(passwordField.text))
        {
            feedbackText.text = "Username and password cannot be empty!";
            LastFeedbackMessage = feedbackText.text; 
            yield break;
        }

        string json = JsonUtility.ToJson(new PlayerLoginData
        {
            username = usernameField.text,
            password = passwordField.text
        });

        using (UnityWebRequest request = new UnityWebRequest(loginUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                feedbackText.text = "Login successful!";
                LastFeedbackMessage = feedbackText.text;

                // Parse de respons
                try
                {
                    PlayerLoginResponse response = JsonUtility.FromJson<PlayerLoginResponse>(request.downloadHandler.text);
                    Debug.Log($"Login successful for {response.username}!");

                    // Stel gegevens in via PlayerManager
                    PlayerManager.Instance.SetPlayerData(response.playerId, response.username, response.highestLevelReached);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Failed to parse response: " + ex.Message);
                }

                IsLoggedIn = true;
            }
            else
            {
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
        public string username; // Voeg de gebruikersnaam toe
        public int highestLevelReached;
    }

    [System.Serializable]
    public class PlayerLoginData
    {
        public string username;
        public string password;
    }
}