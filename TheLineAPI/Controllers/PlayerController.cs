using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MyGameAPI.Services;
using MyGameAPI.Models;
using System.Threading.Tasks;

// This controller handles player sign-up and login functionality, including creating a new player, validating login credentials, 
// and fetching player-related statistics from the leaderboard.

[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly MongoDbService _mongoDbService;

    public PlayerController(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    // Endpoint to handle player sign-up
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] Player player)
    {
        // Validate player data
        if (player == null || string.IsNullOrEmpty(player.Username) || string.IsNullOrEmpty(player.Password))
        {
            return BadRequest(new { message = "Invalid player data" });
        }

        // Access the players and leaderboard collections in MongoDB
        var playersCollection = _mongoDbService.Database.GetCollection<Player>("Players");
        var leaderboardCollection = _mongoDbService.Database.GetCollection<Leaderboard>("Leaderboard");

        // Check if a player with the same username already exists
        var existingPlayer = await playersCollection.Find(p => p.Username == player.Username).FirstOrDefaultAsync();

        if (existingPlayer != null)
        {
            return Conflict(new { message = "Username already exists" });
        }

        // Generate a unique ID if not already set
        if (string.IsNullOrEmpty(player.Id))
        {
            player.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(); // Or use Guid.NewGuid().ToString();
        }

        // Hash the player's password for secure storage
        player.Password = BCrypt.Net.BCrypt.HashPassword(player.Password);

        // Insert the player into the database
        await playersCollection.InsertOneAsync(player);

        // Create a leaderboard record for the new player
        var leaderboard = new Leaderboard
        {
            PlayerId = player.Id,
            HighestLevelReached = 0
        };

        await leaderboardCollection.InsertOneAsync(leaderboard);

        return Ok(new { message = "Player signed up successfully!", playerId = player.Id });
    }

    // Endpoint to handle player login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Player loginDetails)
    {
        // Validate login details
        if (string.IsNullOrEmpty(loginDetails.Username) || string.IsNullOrEmpty(loginDetails.Password))
        {
            return BadRequest(new { message = "Username or password cannot be empty" });
        }

        // Access the players and leaderboard collections in MongoDB
        var playersCollection = _mongoDbService.Database.GetCollection<Player>("Players");
        var leaderboardCollection = _mongoDbService.Database.GetCollection<Leaderboard>("Leaderboard");

        // Look for the player by their username
        var player = await playersCollection.Find(p => p.Username == loginDetails.Username).FirstOrDefaultAsync();

        // Check if the player exists and if the password matches
        if (player == null || !BCrypt.Net.BCrypt.Verify(loginDetails.Password, player.Password))
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        // Fetch the player's leaderboard statistics
        var leaderboard = await leaderboardCollection.Find(ps => ps.PlayerId == player.Id).FirstOrDefaultAsync();

        if (leaderboard == null)
        {
            // If no stats record exists, create an empty one
            leaderboard = new Leaderboard
            {
                PlayerId = player.Id,
                HighestLevelReached = 0,
                Minutes = 0,
                Seconds = 0,
                Milliseconds = 0
            };
            await leaderboardCollection.InsertOneAsync(leaderboard);
        }

        // Return a response with the player's info and leaderboard statistics
        return Ok(new
        {
            message = "Login successful!",
            playerId = player.Id,
            username = player.Username,
            highestLevelReached = leaderboard.HighestLevelReached,
            minutes = leaderboard.Minutes,
            seconds = leaderboard.Seconds,
            milliseconds = leaderboard.Milliseconds
        });
    }
}

// Class to represent a player in the game
public class Player
{
    public string Id { get; set; } // The player's unique identifier
    public string Username { get; set; } // The player's username
    public string Password { get; set; } // The player's hashed password
}

// Class to represent the leaderboard statistics of a player
public class Leaderboard
{
    public string PlayerId { get; set; } // The player's unique identifier
    public int HighestLevelReached { get; set; } // The highest level the player has reached
    public int Minutes { get; set; } // Time in minutes to reach the highest level
    public int Seconds { get; set; } // Time in seconds to reach the highest level
    public int Milliseconds { get; set; } // Time in milliseconds to reach the highest level
}
