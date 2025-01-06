using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyGameAPI.Models
{
    public class Leaderboard
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string PlayerId { get; set; }
        public int HighestLevelReached { get; set; }

        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int Milliseconds { get; set; }
    }
}
