// This script handles the player sign-up process, validating the input and sending registration data to the server.
// It allows the player to create a new account by providing a username and password.

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SignUp : MonoBehaviour
{
    public InputField UsernameInput;     // Input field for the username
    public InputField PasswordInput;     // Input field for the password
    public InputField RepeatPasswordInput; // Input field to confirm the password
    public Button RegisterButton;        // Button to initiate registration

    private const string SignUpUrl = "http://localhost:5033/api/player/signup";  // URL for the sign-up API

    private void Start()
    {
        RegisterButton.onClick.AddListener(OnRegisterButtonClicked);  // Attach event listener to the register button
    }

    private void OnRegisterButtonClicked()
    {
        string username = UsernameInput.text;
        string password = PasswordInput.text;
        string repeatPassword = RepeatPasswordInput.text;

        // Validate if passwords match
        if (password != repeatPassword)
        {
            Debug.LogError("Passwords do not match!");
            return;
        }

        // Start sign-up process if passwords match
        StartCoroutine(SignUpPlayer(username, password));
    }

    private IEnumerator SignUpPlayer(string username, string password)
    {
        // Prepare the data to send in the request
        string jsonData = JsonUtility.ToJson(new PlayerData(username, password));

        UnityWebRequest request = new UnityWebRequest(SignUpUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();  // Wait for the response

        // Check if the request was successful
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Player successfully registered!");  // Display success message
        }
        else
        {
            // Handle errors during sign-up
            Debug.LogError($"Error: {request.error}");
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public string username;  // Player's chosen username
        public string password;  // Player's chosen password

        public PlayerData(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}