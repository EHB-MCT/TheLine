using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System; 

namespace MyGameAPI.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public IMongoDatabase Database => _database; // Expose the database

        public MongoDbService(IConfiguration config)
        {
            try
            {
                var client = new MongoClient(config["MongoDB:ConnectionString"]);
                _database = client.GetDatabase(config["MongoDB:DatabaseName"]);
                Console.WriteLine("Database connection established!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect to database: {ex.Message}");
                throw;
            }
        }
    }
}
