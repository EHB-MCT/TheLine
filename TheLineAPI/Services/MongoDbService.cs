using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System; 

namespace MyGameAPI.Services
{
    // Service responsible for interacting with the MongoDB database.
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        // Expose the database property for access in other classes.
        public IMongoDatabase Database => _database; 

        // Constructor to initialize the MongoDbService with connection details from configuration.
        public MongoDbService(IConfiguration config)
        {
            try
            {
                // Create a MongoDB client using the connection string from configuration.
                var client = new MongoClient(config["MongoDB:ConnectionString"]);

                // Get the database by name from the MongoDB client.
                _database = client.GetDatabase(config["MongoDB:DatabaseName"]);

                Console.WriteLine("Database connection established!"); // Log successful connection.
            }
            catch (Exception ex)
            {
                // If connection fails, print the error and throw it.
                Console.WriteLine($"Failed to connect to database: {ex.Message}");
                throw;
            }
        }
    }
}
