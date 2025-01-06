using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MyGameAPI.Services;
using MyGameAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic; // For the List<T> type
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

// This controller manages player statistics, including updating stats for specific levels and fetching the player's stats.
// It also updates global stats by calling the GlobalStats API when player stats are modified.

[ApiController]
[Route("api/[controller]")]
public class PlayerStatsController : ControllerBase
{
    private readonly MongoDbService _mongoDbService;

    public PlayerStatsController(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    // Endpoint to update player statistics for a specific level
    [HttpPost("update-stats")]
    public async Task<IActionResult> UpdateStats([FromBody] UpdateStatsRequest request)
    {
        // Validate the request data (PlayerId and Level range)
        if (string.IsNullOrEmpty(request.PlayerId) || request.Level < 1 || request.Level > 5)
        {
            return BadRequest(new { message = "Invalid request data" });
        }

        // Access the PlayerStats collection in MongoDB
        var playerStatsCollection = _mongoDbService.Database.GetCollection<PlayerStats>("PlayerStats");

        // Retrieve the player's statistics or create a new document if it doesn't exist
        var playerStats = await playerStatsCollection.Find(ps => ps.PlayerId == request.PlayerId).FirstOrDefaultAsync();
        if (playerStats == null)
        {
            playerStats = new PlayerStats
            {
                Id = ObjectId.GenerateNewId().ToString(), // Generate a unique ID
                PlayerId = request.PlayerId,
                Levels = new List<LevelStats>
                {
                    new LevelStats { LevelNumber = request.Level }
                }
            };
        }

        // Find or create a level record for the given level number
        var levelStats = playerStats.Levels.FirstOrDefault(ls => ls.LevelNumber == request.Level);
        if (levelStats == null)
        {
            levelStats = new LevelStats { LevelNumber = request.Level };
            playerStats.Levels.Add(levelStats);
        }

        // Update the statistics for the specified level
        levelStats.Plays += request.PlaysToAdd;
        levelStats.DeathsByLine += request.DeathsByLine;
        levelStats.DeathsByObstacles += request.DeathsByObstacles;
        levelStats.TimeIconsCollected += request.TimeIconsCollected;

        // Save the updated player stats in the database
        await playerStatsCollection.ReplaceOneAsync(
            ps => ps.PlayerId == request.PlayerId,
            playerStats,
            new ReplaceOptions { IsUpsert = true }
        );

        // Call the GlobalStats API to update global statistics
        var globalStatsUpdateRequest = new GlobalStatsController.UpdateGlobalStatsRequest
        {
            Level = request.Level,
            PlaysToAdd = request.PlaysToAdd,
            DeathsByLine = request.DeathsByLine,
            DeathsByObstacles = request.DeathsByObstacles,
            TimeIconsCollected = request.TimeIconsCollected
        };

        // Update global stats via the GlobalStats controller
        await new GlobalStatsController(_mongoDbService).UpdateGlobalStats(globalStatsUpdateRequest);

        return Ok(new { message = $"Player and Global stats updated for Player ID: {request.PlayerId}, Level: {request.Level}" });
    }

    // Endpoint to get statistics for a specific player by their PlayerId
    [HttpGet("get-stats/{playerId}")]
    public async Task<IActionResult> GetStats(string playerId)
    {
        // Validate the PlayerId
        if (string.IsNullOrEmpty(playerId))
        {
            return BadRequest(new { message = "Player ID is required" });
        }

        // Access the PlayerStats collection in MongoDB
        var playerStatsCollection = _mongoDbService.Database.GetCollection<PlayerStats>("PlayerStats");

        // Retrieve the player's statistics based on the PlayerId
        var playerStats = await playerStatsCollection.Find(ps => ps.PlayerId == playerId).FirstOrDefaultAsync();

        // Return a 404 if the player's stats are not found
        if (playerStats == null)
        {
            return NotFound(new { message = "No stats found for this player." });
        }

        // Return the player's statistics
        return Ok(playerStats);
    }

    // Request model for updating player stats
    public class UpdateStatsRequest
    {
        public string PlayerId { get; set; } // The player's unique ID
        public int Level { get; set; } // Level number
        public int PlaysToAdd { get; set; } // Number of plays to add for the level
        public int DeathsByLine { get; set; } // Number of deaths caused by own line
        public int DeathsByObstacles { get; set; } // Number of deaths caused by obstacles
        public int TimeIconsCollected { get; set; } // Number of time icons collected
    }

    // Player statistics model
    public class PlayerStats
    {
        [BsonId]
        public string Id { get; set; } // Unique identifier for the stats record
        public string PlayerId { get; set; } // The player's unique ID
        public List<LevelStats> Levels { get; set; } // List of level stats for this player
    }

    // Level statistics model
    public class LevelStats
    {
        public int LevelNumber { get; set; } // Level number (1, 2, 3, etc.)
        public int Plays { get; set; } // Number of times the player played this level
        public int DeathsByLine { get; set; } // Number of deaths due to the player's own line
        public int DeathsByObstacles { get; set; } // Number of deaths due to obstacles
        public int TimeIconsCollected { get; set; } // Number of time icons collected in this level
    }
}
