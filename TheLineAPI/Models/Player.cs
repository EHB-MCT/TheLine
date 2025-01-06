using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyGameAPI.Models
{
    // This class represents a player in the game.
    public class Player
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]  // Use ObjectId for the unique identifier.
        public string Id { get; set; }  // Unique identifier for the player.

        // The player's chosen username.
        public string Username { get; set; }

        // The player's password, which should be securely stored (e.g., hashed).
        public string Password { get; set; }
    }
}
