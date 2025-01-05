using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MyGameAPI.Services;
using MyGameAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly MongoDbService _mongoDbService;

    public LeaderboardController(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    [HttpPost("update-highest-level")]
    public async Task<IActionResult> UpdateHighestLevel([FromBody] UpdateLevelRequest request)
    {
        if (string.IsNullOrEmpty(request.PlayerId))
        {
            return BadRequest(new { message = "PlayerId is required" });
        }

        if (request.NewLevel <= 0)
        {
            return BadRequest(new { message = "Invalid level provided" });
        }

        if (request.Minutes < 0 || request.Seconds < 0 || request.Milliseconds < 0)
        {
            return BadRequest(new { message = "Invalid time values provided" });
        }

        var leaderboardCollection = _mongoDbService.Database.GetCollection<Leaderboard>("Leaderboard");
        var leaderboard = await leaderboardCollection.Find(ps => ps.PlayerId == request.PlayerId).FirstOrDefaultAsync();

        if (leaderboard == null)
        {
            return NotFound(new { message = "Leaderboard not found" });
        }

        if (request.NewLevel > leaderboard.HighestLevelReached)
        {
            var update = Builders<Leaderboard>.Update
                .Set(ps => ps.HighestLevelReached, request.NewLevel)
                .Set(ps => ps.Minutes, request.Minutes)
                .Set(ps => ps.Seconds, request.Seconds)
                .Set(ps => ps.Milliseconds, request.Milliseconds);

            await leaderboardCollection.UpdateOneAsync(ps => ps.PlayerId == request.PlayerId, update);

            return Ok(new { message = "Highest level and time updated successfully!" });
        }

        return Ok(new { message = "No update needed; level not higher than current highest." });
    }

    [HttpGet("{playerId}")]
    public async Task<IActionResult> GetLeaderboard(string playerId)
    {
        var leaderboardCollection = _mongoDbService.Database.GetCollection<Leaderboard>("Leaderboard");
        var leaderboard = await leaderboardCollection.Find(ps => ps.PlayerId == playerId).FirstOrDefaultAsync();

        if (leaderboard == null)
        {
            return NotFound(new { message = "Leaderboard not found" });
        }

        return Ok(leaderboard);
    }

    [HttpGet("rankings")]
    public async Task<IActionResult> GetRankedLeaderboard()
    {
        var leaderboardCollection = _mongoDbService.Database.GetCollection<Leaderboard>("Leaderboard");
        var leaderboardEntries = await leaderboardCollection.Find(_ => true).ToListAsync();

        // Sorteer op hoogste level, dan op tijd (minuten, seconden, milliseconden)
        var rankedLeaderboard = leaderboardEntries
            .OrderByDescending(entry => entry.HighestLevelReached)
            .ThenBy(entry => entry.Minutes)
            .ThenBy(entry => entry.Seconds)
            .ThenBy(entry => entry.Milliseconds)
            .Select((entry, index) => new
            {
                Rank = index + 1,
                entry.PlayerId,
                entry.HighestLevelReached,
                entry.Minutes,
                entry.Seconds,
                entry.Milliseconds
            })
            .ToList();

        return Ok(rankedLeaderboard);
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