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
public class GlobalStatsController : ControllerBase
{
    private readonly MongoDbService _mongoDbService;

    public GlobalStatsController(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    [HttpPost("update-global-stats")]
    public async Task<IActionResult> UpdateGlobalStats([FromBody] UpdateGlobalStatsRequest request)
    {
        if (request.Level < 1 || request.Level > 5)
        {
            return BadRequest(new { message = "Invalid level specified." });
        }

        var globalStatsCollection = _mongoDbService.Database.GetCollection<GlobalStats>("GlobalStats");

        // Gebruik een statisch ID voor de globale statistieken
        var globalStatsId = "global-stats";

        // Haal of maak het globale stats-document
        var globalStats = await globalStatsCollection.Find(gs => gs.Id == globalStatsId).FirstOrDefaultAsync();
        if (globalStats == null)
        {
            globalStats = new GlobalStats
            {
                Id = globalStatsId,
                Levels = new List<GlobalLevelStats>()
            };
        }

        // Zoek of het level al bestaat in de statistieken
        var levelStats = globalStats.Levels.FirstOrDefault(ls => ls.LevelNumber == request.Level);
        if (levelStats == null)
        {
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

        // Update de statistieken voor het specifieke level
        levelStats.TotalPlays += request.PlaysToAdd;
        levelStats.TotalDeathsByLine += request.DeathsByLine;
        levelStats.TotalDeathsByObstacles += request.DeathsByObstacles;
        levelStats.TotalTimeIconsCollected += request.TimeIconsCollected;

        // Sla de wijzigingen op in de database
        await globalStatsCollection.ReplaceOneAsync(
            gs => gs.Id == globalStatsId,
            globalStats,
            new ReplaceOptions { IsUpsert = true }
        );

        return Ok(new { message = $"Global stats updated for Level: {request.Level}" });
    }

    [HttpGet("get-global-stats")]
    public async Task<IActionResult> GetGlobalStats()
    {
        var globalStatsCollection = _mongoDbService.Database.GetCollection<GlobalStats>("GlobalStats");
        var globalStats = await globalStatsCollection.Find(gs => true).FirstOrDefaultAsync();

        if (globalStats == null)
        {
            return NotFound(new { message = "No global stats found." });
        }

        return Ok(globalStats);
    }

    public class UpdateGlobalStatsRequest
    {
        public int Level { get; set; }
        public int PlaysToAdd { get; set; } // Hoeveel keer een level is gespeeld
        public int DeathsByLine { get; set; } // Aantal keren dood door eigen lijn
        public int DeathsByObstacles { get; set; } // Aantal keren dood door obstakels
        public int TimeIconsCollected { get; set; } // Aantal opgepikte time icons
    }
}

public class GlobalStats
{
    [BsonId]
    [BsonRepresentation(BsonType.String)] // Statisch ID voor global stats
    public string Id { get; set; }
    public List<GlobalLevelStats> Levels { get; set; }
}

public class GlobalLevelStats
{
    public int LevelNumber { get; set; }
    public int TotalPlays { get; set; }
    public int TotalDeathsByLine { get; set; }
    public int TotalDeathsByObstacles { get; set; }
    public int TotalTimeIconsCollected { get; set; }
}
