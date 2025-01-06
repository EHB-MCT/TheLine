using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic; // Needed for List<>

namespace MyGameAPI.Models
{
    // This class represents the global statistics for the game.
    // It contains an ID and a list of level statistics.
    public class GlobalStats
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)] // Use a string as the ID
        public string Id { get; set; }  // Unique identifier for the global stats document.

        // List of statistics for each level in the game.
        public List<GlobalLevelStats> Levels { get; set; }  // A collection of global level stats.
    }

    // This class holds statistics for a specific level.
    public class GlobalLevelStats
    {
        public int LevelNumber { get; set; } // The level number for which the stats are recorded.
        public int DeathsByLine { get; set; } // The number of deaths caused by the player's own line.
        public int DeathsByObstacles { get; set; } // The number of deaths caused by obstacles.
        public int TimeIconsCollected { get; set; } // The number of time icons collected in this level.
    }
}
