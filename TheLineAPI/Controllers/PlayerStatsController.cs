using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MyGameAPI.Services;
using MyGameAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic; // Voor de List<T> type
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

[ApiController]
[Route("api/[controller]")]
public class PlayerStatsController : ControllerBase
{
    private readonly MongoDbService _mongoDbService;

    public PlayerStatsController(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    [HttpPost("update-stats")]
    public async Task<IActionResult> UpdateStats([FromBody] UpdateStatsRequest request)
    {
        if (string.IsNullOrEmpty(request.PlayerId) || request.Level < 1 || request.Level > 5)
        {
            return BadRequest(new { message = "Invalid request data" });
        }

        var playerStatsCollection = _mongoDbService.Database.GetCollection<PlayerStats>("PlayerStats");

        // PlayerStats bijwerken
        var playerStats = await playerStatsCollection.Find(ps => ps.PlayerId == request.PlayerId).FirstOrDefaultAsync();
        if (playerStats == null)
        {
            playerStats = new PlayerStats
            {
                Id = ObjectId.GenerateNewId().ToString(), // Genereer een uniek ID
                PlayerId = request.PlayerId,
                Levels = new List<LevelStats>
                {
                    new LevelStats { LevelNumber = request.Level }
                }
            };
        }

        var levelStats = playerStats.Levels.FirstOrDefault(ls => ls.LevelNumber == request.Level);
        if (levelStats == null)
        {
            levelStats = new LevelStats { LevelNumber = request.Level };
            playerStats.Levels.Add(levelStats);
        }

        levelStats.Plays += request.PlaysToAdd;
        levelStats.DeathsByLine += request.DeathsByLine;
        levelStats.DeathsByObstacles += request.DeathsByObstacles;
        levelStats.TimeIconsCollected += request.TimeIconsCollected;

        await playerStatsCollection.ReplaceOneAsync(
            ps => ps.PlayerId == request.PlayerId,
            playerStats,
            new ReplaceOptions { IsUpsert = true }
        );

        // Roep GlobalStats API aan
        var globalStatsUpdateRequest = new GlobalStatsController.UpdateGlobalStatsRequest
        {
            Level = request.Level,
            PlaysToAdd = request.PlaysToAdd,
            DeathsByLine = request.DeathsByLine,
            DeathsByObstacles = request.DeathsByObstacles,
            TimeIconsCollected = request.TimeIconsCollected
        };

        await new GlobalStatsController(_mongoDbService).UpdateGlobalStats(globalStatsUpdateRequest);

        return Ok(new { message = $"Player and Global stats updated for Player ID: {request.PlayerId}, Level: {request.Level}" });
    }

    [HttpGet("get-stats/{playerId}")]
    public async Task<IActionResult> GetStats(string playerId)
    {
        if (string.IsNullOrEmpty(playerId))
        {
            return BadRequest(new { message = "Player ID is required" });
        }

        var playerStatsCollection = _mongoDbService.Database.GetCollection<PlayerStats>("PlayerStats");
        var playerStats = await playerStatsCollection.Find(ps => ps.PlayerId == playerId).FirstOrDefaultAsync();

        if (playerStats == null)
        {
            return NotFound(new { message = "No stats found for this player." });
        }

        return Ok(playerStats);
    }

    public class UpdateStatsRequest
    {
        public string PlayerId { get; set; }
        public int Level { get; set; } // Levelnummer
        public int PlaysToAdd { get; set; } // Hoeveel keer een level moet worden toegevoegd
        public int DeathsByLine { get; set; } // Aantal keer dood door eigen lijn
        public int DeathsByObstacles { get; set; } // Aantal keer dood door obstakels
        public int TimeIconsCollected { get; set; } // Aantal opgepikte time icons
    }
}
