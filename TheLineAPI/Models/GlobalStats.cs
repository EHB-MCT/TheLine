using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic; // Nodig voor List<>

namespace MyGameAPI.Models
{
    public class GlobalStats
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)] // Gebruik een string als ID
        public string Id { get; set; }

        public List<GlobalLevelStats> Levels { get; set; }
    }

    public class GlobalLevelStats
    {
        public int LevelNumber { get; set; }
        public int DeathsByLine { get; set; }
        public int DeathsByObstacles { get; set; }
        public int TimeIconsCollected { get; set; }
    }
}
