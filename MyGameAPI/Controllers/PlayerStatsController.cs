using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MyGameAPI.Services;
using MyGameAPI.Models;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class PlayerStatsController : ControllerBase
{
    private readonly MongoDbService _mongoDbService;

    public PlayerStatsController(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    [HttpPost("update-highest-level")]
    public async Task<IActionResult> UpdateHighestLevel([FromBody] UpdateLevelRequest request)
    {
        if (string.IsNullOrEmpty(request.PlayerId) || request.NewLevel <= 0 || (request.Minutes < 0 && request.Seconds < 0 && request.Milliseconds < 0))
        {
            return BadRequest(new { message = "Invalid data provided" });
        }

        var playerStatsCollection = _mongoDbService.Database.GetCollection<PlayerStats>("PlayerStats");
        var playerStats = await playerStatsCollection.Find(ps => ps.PlayerId == request.PlayerId).FirstOrDefaultAsync();

        if (playerStats == null)
        {
            return NotFound(new { message = "Player stats not found" });
        }

        if (request.NewLevel > playerStats.HighestLevelReached)
        {
            var update = Builders<PlayerStats>.Update
                .Set(ps => ps.HighestLevelReached, request.NewLevel)
                .Set(ps => ps.Minutes, request.Minutes)
                .Set(ps => ps.Seconds, request.Seconds)
                .Set(ps => ps.Milliseconds, request.Milliseconds);

            await playerStatsCollection.UpdateOneAsync(ps => ps.PlayerId == request.PlayerId, update);

            return Ok(new { message = "Highest level and time updated successfully!" });
        }

        return Ok(new { message = "No update needed; level not higher than current highest." });
    }

    [HttpGet("{playerId}")]
    public async Task<IActionResult> GetPlayerStats(string playerId)
    {
        var playerStatsCollection = _mongoDbService.Database.GetCollection<PlayerStats>("PlayerStats");
        var playerStats = await playerStatsCollection.Find(ps => ps.PlayerId == playerId).FirstOrDefaultAsync();

        if (playerStats == null)
        {
            return NotFound(new { message = "Player stats not found" });
        }

        return Ok(playerStats);
    }

    public class UpdateLevelRequest
    {
        public string PlayerId { get; set; }
        public int NewLevel { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int Milliseconds { get; set; }
    }
}