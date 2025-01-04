using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SignUp : MonoBehaviour
{
    public InputField UsernameInput;
    public InputField PasswordInput;
    public InputField RepeatPasswordInput;
    public Button RegisterButton;

    private const string SignUpUrl = "http://localhost:5033/api/player/signup";

    private void Start()
    {
        RegisterButton.onClick.AddListener(OnRegisterButtonClicked);
    }

    private void OnRegisterButtonClicked()
    {
        string username = UsernameInput.text;
        string password = PasswordInput.text;
        string repeatPassword = RepeatPasswordInput.text;

        if (password != repeatPassword)
        {
            Debug.LogError("Passwords do not match!");
            return;
        }

        StartCoroutine(SignUpPlayer(username, password));
    }

    private IEnumerator SignUpPlayer(string username, string password)
    {
        string jsonData = JsonUtility.ToJson(new PlayerData(username, password));

        UnityWebRequest request = new UnityWebRequest(SignUpUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Player successfully registered!");
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public string username;
        public string password;

        public PlayerData(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}