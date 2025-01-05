using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MyGameAPI.Services;
using MyGameAPI.Models;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly MongoDbService _mongoDbService;

    public PlayerController(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] Player player)
    {
        if (player == null || string.IsNullOrEmpty(player.Username) || string.IsNullOrEmpty(player.Password))
        {
            return BadRequest(new { message = "Invalid player data" });
        }

        var playersCollection = _mongoDbService.Database.GetCollection<Player>("Players");
        var playerStatsCollection = _mongoDbService.Database.GetCollection<PlayerStats>("PlayerStats");
        var existingPlayer = await playersCollection.Find(p => p.Username == player.Username).FirstOrDefaultAsync();

        if (existingPlayer != null)
        {
            return Conflict(new { message = "Username already exists" });
        }

        // Genereer een unieke ID als deze nog niet is ingesteld
        if (string.IsNullOrEmpty(player.Id))
        {
            player.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(); // Of gebruik Guid.NewGuid().ToString();
        }

        // Hash het wachtwoord
        player.Password = BCrypt.Net.BCrypt.HashPassword(player.Password);

        await playersCollection.InsertOneAsync(player);

        // Voeg een nieuw record toe in PlayerStats
        var playerStats = new PlayerStats
        {
            PlayerId = player.Id,
            HighestLevelReached = 0
        };

        await playerStatsCollection.InsertOneAsync(playerStats);

        return Ok(new { message = "Player signed up successfully!", playerId = player.Id });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Player loginDetails)
    {
        if (string.IsNullOrEmpty(loginDetails.Username) || string.IsNullOrEmpty(loginDetails.Password))
        {
            return BadRequest(new { message = "Username or password cannot be empty" });
        }

        var playersCollection = _mongoDbService.Database.GetCollection<Player>("Players");
        var player = await playersCollection.Find(p => p.Username == loginDetails.Username).FirstOrDefaultAsync();

        if (player == null || !BCrypt.Net.BCrypt.Verify(loginDetails.Password, player.Password))
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        // Voeg de username en eventueel andere velden toe aan de respons
        return Ok(new
        {
            message = "Login successful!",
            playerId = player.Id,
            username = player.Username // Voeg username toe aan de respons
        });
    }
}