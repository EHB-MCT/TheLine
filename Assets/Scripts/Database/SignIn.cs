using UnityEngine;
using UnityEngine.UI; // For UI components like InputFields and Buttons
using MongoDB.Driver;
using MongoDB.Bson;
using System.Security.Cryptography; // For password hashing
using System.Text; // To convert strings into bytes
using System.IO; // To work with file paths
using Newtonsoft.Json; // For deserializing the JSON configuration
using System;

public class SignIn : MonoBehaviour
{
    public InputField playerNameInputField; 
    public InputField passwordInputField; 
    public Button signInButton;
    public Text errorMessageText;

    private MongoClient client;
    private IMongoDatabase database;
    private string connectionString;

    public static bool isLoggedIn = false;
    public static string playerName;

    void Start()
    {
        LoadConfiguration();
        client = new MongoClient(connectionString);
        database = client.GetDatabase("TheLine");

        signInButton.onClick.AddListener(OnSignInClicked);
    }

    private void OnSignInClicked()
    {
        string playerName = playerNameInputField.text;
        string password = passwordInputField.text;

        SignIn.playerName = playerName; // Store the player name in the static variable

        var player = GetPlayerByName(playerName);

        if (player == null)
        {
            ShowErrorMessage("Player not found!");
            return;
        }

        string storedHashedPassword = player["password"].AsString;
        string hashedPassword = HashPassword(password);

        if (hashedPassword == storedHashedPassword)
        {
            Debug.Log("Login successful!");
            isLoggedIn = true;
        }
        else
        {
            ShowErrorMessage("Incorrect password!");
        }
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }

    private BsonDocument GetPlayerByName(string playerName)
    {
        var collection = database.GetCollection<BsonDocument>("Players");
        var filter = Builders<BsonDocument>.Filter.Eq("playerName", playerName);
        return collection.Find(filter).FirstOrDefault();
    }

    private void ShowErrorMessage(string message)
    {
        if (errorMessageText != null)
        {
            errorMessageText.text = message;
        }
        else
        {
            Debug.LogError(message);
        }
    }

    private void LoadConfiguration()
    {
        string configFilePath = Path.Combine(Application.streamingAssetsPath, "config.json");

        if (File.Exists(configFilePath))
        {
            string json = File.ReadAllText(configFilePath);
            Config config = JsonConvert.DeserializeObject<Config>(json);

            connectionString = config.mongoConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                Debug.LogError("Connection string is empty!");
            }
        }
        else
        {
            Debug.LogError("Config file not found!");
        }
    }
}