using UnityEngine;
using UnityEngine.SceneManagement;
using MongoDB.Driver;
using System.IO; // Voor Path en File
using Newtonsoft.Json; // Voor JsonConvert

public class StartGame : MonoBehaviour
{
    private MongoClient client;
    private IMongoDatabase database;
    private string connectionString;

    void Start()
    {
        // Laad configuratie voor de databaseverbinding
        LoadConfiguration();
        client = new MongoClient(connectionString);
        database = client.GetDatabase("TheLine");
    }

    public void StartGameProcess()
    {
        if (SignIn.isLoggedIn)
        {
            Debug.Log("Starting the game...");
            SceneManager.LoadScene(2); // Laad het eerste level
        }
        else
        {
            Debug.LogError("You must be logged in to start the game!");
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
