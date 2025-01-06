using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MyGameAPI.Models
{
    // This class represents the player stats that track progress and achievements at different levels.
    public class PlayerStats
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]  // The unique identifier for the player stats.
        public string Id { get; set; }

        public string PlayerId { get; set; }  // The ID of the player these stats belong to.

        // A list to hold stats for each level the player plays.
        public List<LevelStats> Levels { get; set; } = new List<LevelStats>
        {
            new LevelStats { LevelNumber = 1 },  // Initialize stats for level 1
            new LevelStats { LevelNumber = 2 },  // Initialize stats for level 2
            new LevelStats { LevelNumber = 3 },  // Initialize stats for level 3
            new LevelStats { LevelNumber = 4 },  // Initialize stats for level 4
            new LevelStats { LevelNumber = 5 }   // Initialize stats for level 5
        };
    }

    // This class holds individual statistics for a specific level in the game.
    public class LevelStats
    {
        public int LevelNumber { get; set; }  // The number of the level (1 to 5).
        public int Plays { get; set; }  // How many times the player has played this level.
        public int DeathsByLine { get; set; }  // How many times the player died due to their own line.
        public int DeathsByObstacles { get; set; }  // How many times the player died due to obstacles.
        public int TimeIconsCollected { get; set; }  // How many time icons the player collected in this level.
    }
}
