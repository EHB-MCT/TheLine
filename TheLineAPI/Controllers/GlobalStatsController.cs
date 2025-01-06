using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MyGameAPI.Services;
using MyGameAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic; // For the List<T> type
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

// This controller manages global statistics for the game, including updating and retrieving stats.
// It works with a MongoDB database to track stats for each level, such as plays, deaths, and time icons collected.

[ApiController]
[Route("api/[controller]")]
public class GlobalStatsController : ControllerBase
{
    private readonly MongoDbService _mongoDbService;

    public GlobalStatsController(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    // Endpoint for updating the global statistics of a specific level
    [HttpPost("update-global-stats")]
    public async Task<IActionResult> UpdateGlobalStats([FromBody] UpdateGlobalStatsRequest request)
    {
        // Validate the level input (should be between 1 and 5)
        if (request.Level < 1 || request.Level > 5)
        {
            return BadRequest(new { message = "Invalid level specified." });
        }

        // Access the collection for global statistics in the MongoDB database
        var globalStatsCollection = _mongoDbService.Database.GetCollection<GlobalStats>("GlobalStats");

        // Use a static ID for the global statistics document
        var globalStatsId = "global-stats";

        // Retrieve the global stats document or create a new one if it doesn't exist
        var globalStats = await globalStatsCollection.Find(gs => gs.Id == globalStatsId).FirstOrDefaultAsync();
        if (globalStats == null)
        {
            globalStats = new GlobalStats
            {
                Id = globalStatsId,
                Levels = new List<GlobalLevelStats>()
            };
        }

        // Search for the specific level stats within the global stats
        var levelStats = globalStats.Levels.FirstOrDefault(ls => ls.LevelNumber == request.Level);
        if (levelStats == null)
        {
            // If no stats exist for this level, initialize them
            levelStats = new GlobalLevelStats
            {
                LevelNumber = request.Level,
                TotalPlays = 0,
                TotalDeathsByLine = 0,
                TotalDeathsByObstacles = 0,
                TotalTimeIconsCollected = 0
            };
            globalStats.Levels.Add(levelStats);
        }

        // Update the statistics for the specific level
        levelStats.TotalPlays += request.PlaysToAdd;
        levelStats.TotalDeathsByLine += request.DeathsByLine;
        levelStats.TotalDeathsByObstacles += request.DeathsByObstacles;
        levelStats.TotalTimeIconsCollected += request.TimeIconsCollected;

        // Save the changes to the MongoDB database
        await globalStatsCollection.ReplaceOneAsync(
            gs => gs.Id == globalStatsId,
            globalStats,
            new ReplaceOptions { IsUpsert = true }
        );

        return Ok(new { message = $"Global stats updated for Level: {request.Level}" });
    }

    // Endpoint for retrieving the global statistics
    [HttpGet("get-global-stats")]
    public async Task<IActionResult> GetGlobalStats()
    {
        var globalStatsCollection = _mongoDbService.Database.GetCollection<GlobalStats>("GlobalStats");
        var globalStats = await globalStatsCollection.Find(gs => true).FirstOrDefaultAsync();

        // Return an error message if no global stats are found
        if (globalStats == null)
        {
            return NotFound(new { message = "No global stats found." });
        }

        return Ok(globalStats);
    }

    // Request model for updating global stats
    public class UpdateGlobalStatsRequest
    {
        public int Level { get; set; } // The level number
        public int PlaysToAdd { get; set; } // Number of times a level was played
        public int DeathsByLine { get; set; } // Number of deaths by line
        public int DeathsByObstacles { get; set; } // Number of deaths by obstacles
        public int TimeIconsCollected { get; set; } // Number of time icons collected
    }
}

// Class to represent the global statistics
public class GlobalStats
{
    [BsonId]
    [BsonRepresentation(BsonType.String)] // Static ID for global stats
    public string Id { get; set; }
    public List<GlobalLevelStats> Levels { get; set; }
}

// Class to represent the statistics for each level
public class GlobalLevelStats
{
    public int LevelNumber { get; set; } // The level number
    public int TotalPlays { get; set; } // Total number of times this level was played
    public int TotalDeathsByLine { get; set; } // Total number of deaths by line
    public int TotalDeathsByObstacles { get; set; } // Total number of deaths by obstacles
    public int TotalTimeIconsCollected { get; set; } // Total number of time icons collected
}
