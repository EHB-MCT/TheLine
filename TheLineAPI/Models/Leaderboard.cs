using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyGameAPI.Models
{
    // This class represents a leaderboard entry.
    public class Leaderboard
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]  // Use ObjectId for the unique identifier.
        public string Id { get; set; }  // Unique identifier for the leaderboard entry.

        // The ID of the player who achieved this leaderboard entry.
        public string PlayerId { get; set; }

        // The highest level the player has reached in the game.
        public int HighestLevelReached { get; set; }

        // Time spent by the player to reach the highest level.
        public int Minutes { get; set; } // The number of minutes taken.
        public int Seconds { get; set; } // The number of seconds taken.
        public int Milliseconds { get; set; } // The number of milliseconds taken.
    }
}
