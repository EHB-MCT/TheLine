using UnityEngine;
using UnityEngine.UI; // For UI components like InputFields and Buttons
using MongoDB.Driver;
using MongoDB.Bson;
using System.Security.Cryptography; // For password hashing
using System.Text; // To convert strings into bytes
using System; // For GUID generation
using System.IO; // To work with file paths
using Newtonsoft.Json; // For deserializing the JSON configuration

public class SignUp : MonoBehaviour
{
    public InputField playerNameInputField; // Reference to the player name input field
    public InputField passwordInputField; // Reference to the password input field
    public InputField confirmPasswordInputField; // Reference to the confirm password input field
    public Button signUpButton; // Reference to the sign up button

    private MongoClient client;
    private IMongoDatabase database;
    private string connectionString; // Store the connection string

    void Start()
    {
        // Load the configuration and set up the MongoDB client and database connection
        LoadConfiguration();
        client = new MongoClient(connectionString);
        database = client.GetDatabase("TheLine");

        // Add listener for the Sign Up button
        signUpButton.onClick.AddListener(OnSignUpClicked);
    }

    // Method to be called when the Sign Up button is clicked
    private void OnSignUpClicked()
    {
        string playerName = playerNameInputField.text;
        string password = passwordInputField.text;
        string confirmPassword = confirmPasswordInputField.text;

        // Check if the password and confirm password match
        if (password != confirmPassword)
        {
            Debug.LogError("Passwords do not match!");
            return;
        }

        // Hash the password before storing it
        string hashedPassword = HashPassword(password);

        // Check if the player already exists
        if (PlayerExists(playerName))
        {
            Debug.LogError("Player already exists!");
            return;
        }

        // Insert the new player into the database
        InsertNewPlayer(playerName, hashedPassword);
    }

    // Hash the password using SHA-256 for security
    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return System.BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }

    // Check if the player already exists in the database
    private bool PlayerExists(string playerName)
    {
        var collection = database.GetCollection<BsonDocument>("Players");
        var filter = Builders<BsonDocument>.Filter.Eq("playerName", playerName);
        var player = collection.Find(filter).FirstOrDefault();
        return player != null;
    }

    // Insert a new player into the database
    private void InsertNewPlayer(string playerName, string hashedPassword)
    {
        var collection = database.GetCollection<BsonDocument>("Players");

        // Create a new Player document
        var newPlayer = new BsonDocument
        {
            { "playerId", Guid.NewGuid().ToString() }, // Generate a unique Player ID (GUID)
            { "playerName", playerName },
            { "password", hashedPassword }
        };

        // Insert the new player into the collection
        collection.InsertOne(newPlayer);
        Debug.Log("Player successfully registered!");
    }

    // Method to load the configuration file and set the connection string
    private void LoadConfiguration()
    {
        string configFilePath = Path.Combine(Application.streamingAssetsPath, "config.json");

        if (File.Exists(configFilePath))
        {
            string json = File.ReadAllText(configFilePath);
            Config config = JsonConvert.DeserializeObject<Config>(json);

            // Set the connection string from the config
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