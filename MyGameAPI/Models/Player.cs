using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyGameAPI.Models
{
    public class Player
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        // Nieuw veld
        public int HighestLevelReached { get; set; } = 0; // Default is level 0
    }
}