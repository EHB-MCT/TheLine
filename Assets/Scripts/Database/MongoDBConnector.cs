/*
 * This script connects to a MongoDB database using a connection string stored in a config.json file
 * located in the StreamingAssets folder. It logs success or error messages depending on the outcome.
 */

using MongoDB.Driver; // Import MongoDB driver
using UnityEngine; // Import Unity Engine
using System.IO; // For file path operations
using Newtonsoft.Json; // For JSON deserialization

public class MongoDBConnector : MonoBehaviour
{
    private string connectionString; // MongoDB connection string
    private string databaseName = "TheLine"; // Database name
    private MongoClient client; // MongoDB client
    private IMongoDatabase database; // MongoDB database object

    /*
     * Attempts to load configuration and connect to MongoDB.
     * Logs success or error messages accordingly.
     */
    void Start()
    {
        try
        {
            LoadConfiguration(); // Load configuration from config.json

            client = new MongoClient(connectionString); // Connect to MongoDB
            database = client.GetDatabase(databaseName); // Access the database
            Debug.Log("Connected to MongoDB database!");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error connecting to MongoDB: " + ex.Message);
        }
    }

    /*
     * Loads the connection string from the config.json file in StreamingAssets.
     * Logs an error if the file is missing or the connection string is empty.
     */
    private void LoadConfiguration()
    {
        string configFile = Path.Combine(Application.streamingAssetsPath, "config.json");

        if (File.Exists(configFile))
        {
            string json = File.ReadAllText(configFile);
            Config config = JsonConvert.DeserializeObject<Config>(json);
            connectionString = config.mongoConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                Debug.LogError("The connection string is empty!");
            }
        }
        else
        {
            Debug.LogError("Config file not found!");
        }
    }
}