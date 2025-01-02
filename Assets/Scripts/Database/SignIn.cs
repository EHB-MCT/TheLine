using UnityEngine;
using UnityEngine.UI; // For UI components like InputFields and Buttons
using MongoDB.Driver;
using MongoDB.Bson;
using System.Security.Cryptography; // For password hashing
using System.Text; // To convert strings into bytes
using System.IO; // To work with file paths
using Newtonsoft.Json; // For deserializing the JSON configuration

public class SignIn : MonoBehaviour
{
    public InputField playerNameInputField; // Reference to the player name input field
    public InputField passwordInputField; // Reference to the password input field
    public Button signInButton; // Reference to the sign in button
    public Text errorMessageText; // Reference to display error messages (Optional)

    private MongoClient client;
    private IMongoDatabase database;
    private string connectionString;

    void Start()
    {
        // Load the configuration and set up the MongoDB client and database connection
        LoadConfiguration();
        client = new MongoClient(connectionString);
        database = client.GetDatabase("TheLine");

        // Add listener for the Sign In button
        signInButton.onClick.AddListener(OnSignInClicked);
    }

    // Method to be called when the Sign In button is clicked
    private void OnSignInClicked()
    {
        string playerName = playerNameInputField.text;
        string password = passwordInputField.text;

        // Check if the player exists in the database
        var player = GetPlayerByName(playerName);

        if (player == null)
        {
            ShowErrorMessage("Player not found!");
            return;
        }

        // Get the hashed password from the database
        string storedHashedPassword = player["password"].AsString;

        // Hash the input password and compare with the stored hash
        string hashedPassword = HashPassword(password);

        if (hashedPassword == storedHashedPassword)
        {
            Debug.Log("Login successful!");
            // Proceed to the next screen or game logic
        }
        else
        {
            ShowErrorMessage("Incorrect password!");
        }
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

    // Retrieve player data from the database based on player name
    private BsonDocument GetPlayerByName(string playerName)
    {
        var collection = database.GetCollection<BsonDocument>("Players");
        var filter = Builders<BsonDocument>.Filter.Eq("playerName", playerName);
        return collection.Find(filter).FirstOrDefault();
    }

    // Show an error message (optional: you can link this to a UI Text field)
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