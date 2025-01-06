using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MyGameAPI.Services;
using MyGameAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

// This controller manages the leaderboard functionality, including updating the highest level reached and retrieving player rankings.
// It interacts with a MongoDB database to track each player's highest level and time to reach that level.

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly MongoDbService _mongoDbService;

    public LeaderboardController(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    // Endpoint to update the highest level reached by a player
    [HttpPost("update-highest-level")]
    public async Task<IActionResult> UpdateHighestLevel([FromBody] UpdateLevelRequest request)
    {
        // Validate the input fields
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

        // Access the leaderboard collection in the MongoDB database
        var leaderboardCollection = _mongoDbService.Database.GetCollection<Leaderboard>("Leaderboard");
        var leaderboard = await leaderboardCollection.Find(ps => ps.PlayerId == request.PlayerId).FirstOrDefaultAsync();

        // If no leaderboard entry exists for the player, return an error
        if (leaderboard == null)
        {
            return NotFound(new { message = "Leaderboard not found" });
        }

        // If the new level is higher than the current highest level, update the record
        if (request.NewLevel > leaderboard.HighestLevelReached)
        {
            var update = Builders<Leaderboard>.Update
                .Set(ps => ps.HighestLevelReached, request.NewLevel)
                .Set(ps => ps.Minutes, request.Minutes)
                .Set(ps => ps.Seconds, request.Seconds)
                .Set(ps => ps.Milliseconds, request.Milliseconds);

            // Perform the update in the MongoDB database
            await leaderboardCollection.UpdateOneAsync(ps => ps.PlayerId == request.PlayerId, update);

            return Ok(new { message = "Highest level and time updated successfully!" });
        }

        // If no update is needed, return a message indicating that
        return Ok(new { message = "No update needed; level not higher than current highest." });
    }

    // Endpoint to retrieve a player's leaderboard entry by PlayerId
    [HttpGet("{playerId}")]
    public async Task<IActionResult> GetLeaderboard(string playerId)
    {
        var leaderboardCollection = _mongoDbService.Database.GetCollection<Leaderboard>("Leaderboard");
        var leaderboard = await leaderboardCollection.Find(ps => ps.PlayerId == playerId).FirstOrDefaultAsync();

        // If the player is not found, return a not found error
        if (leaderboard == null)
        {
            return NotFound(new { message = "Leaderboard not found" });
        }

        return Ok(leaderboard);
    }

    // Endpoint to retrieve the top 10 ranked leaderboard entries, sorted by highest level and time
    [HttpGet("rankings")]
    public async Task<IActionResult> GetRankedLeaderboard()
    {
        var leaderboardCollection = _mongoDbService.Database.GetCollection<Leaderboard>("Leaderboard");
        var playersCollection = _mongoDbService.Database.GetCollection<Player>("Players");

        // Fetch all leaderboard entries
        var leaderboardEntries = await leaderboardCollection.Find(_ => true).ToListAsync();

        // Fetch all players and map their IDs to usernames for quick lookup
        var players = await playersCollection.Find(_ => true).ToListAsync();
        var playerDictionary = players.ToDictionary(p => p.Id, p => p.Username);

        // Sort leaderboard entries by highest level, then by time (minutes, seconds, milliseconds)
        var rankedLeaderboard = leaderboardEntries
            .OrderByDescending(entry => entry.HighestLevelReached)
            .ThenBy(entry => entry.Minutes)
            .ThenBy(entry => entry.Seconds)
            .ThenBy(entry => entry.Milliseconds)
            .Take(10) // Limit to top 10 entries
            .Select((entry, index) => new
            {
                Rank = index + 1,
                entry.PlayerId,
                Username = playerDictionary.ContainsKey(entry.PlayerId) ? playerDictionary[entry.PlayerId] : "Unknown",
                entry.HighestLevelReached,
                entry.Minutes,
                entry.Seconds,
                entry.Milliseconds
            })
            .ToList();

        return Ok(rankedLeaderboard);
    }

    // Request model for updating highest level and time for a player
    public class UpdateLevelRequest
    {
        public string PlayerId { get; set; } // The player's unique identifier
        public int NewLevel { get; set; } // The new level reached by the player
        public int Minutes { get; set; } // Time in minutes when the level was reached
        public int Seconds { get; set; } // Time in seconds when the level was reached
        public int Milliseconds { get; set; } // Time in milliseconds when the level was reached
    }
}

// Class to represent the leaderboard data for a player
public class Leaderboard
{
    public string PlayerId { get; set; }
    public int HighestLevelReached { get; set; } // The highest level the player has reached
    public int Minutes { get; set; } // Time in minutes to reach the highest level
    public int Seconds { get; set; } // Time in seconds to reach the highest level
    public int Milliseconds { get; set; } // Time in milliseconds to reach the highest level
}

// Class to represent a player in the game
public class Player
{
    public string Id { get; set; } // The player's unique identifier
    public string Username { get; set; } // The player's username
}
