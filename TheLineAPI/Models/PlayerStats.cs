using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MyGameAPI.Models
{
    public class PlayerStats
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string PlayerId { get; set; }

        // Een lijst van statistieken per level
        public List<LevelStats> Levels { get; set; } = new List<LevelStats>
        {
            new LevelStats { LevelNumber = 1 },
            new LevelStats { LevelNumber = 2 },
            new LevelStats { LevelNumber = 3 },
            new LevelStats { LevelNumber = 4 },
            new LevelStats { LevelNumber = 5 }
        };
    }

    public class LevelStats
    {
        public int LevelNumber { get; set; } // Levelnummer
        public int Plays { get; set; } // Hoe vaak dit level is gespeeld
        public int DeathsByLine { get; set; } // Aantal keren dood door eigen lijn
        public int DeathsByObstacles { get; set; } // Aantal keren dood door obstakels
        public int TimeIconsCollected { get; set; } // Aantal opgepikte time icons
    }
}
