using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MyGameAPI.Services;
using System.Threading.Tasks;
using MyGameAPI.Models;

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
        var existingPlayer = await playersCollection.Find(p => p.Username == player.Username).FirstOrDefaultAsync();

        if (existingPlayer != null)
        {
            return Conflict(new { message = "Username already exists" });
        }

        // Hash het wachtwoord
        player.Password = BCrypt.Net.BCrypt.HashPassword(player.Password);

        await playersCollection.InsertOneAsync(player);
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

        // Voeg HighestLevelReached toe aan de respons
        return Ok(new 
        { 
            message = "Login successful!", 
            playerId = player.Id, 
            highestLevelReached = player.HighestLevelReached 
        });
    }

    [HttpPost("update-highest-level")]
    public async Task<IActionResult> UpdateHighestLevel([FromBody] UpdateLevelRequest request)
    {
        if (string.IsNullOrEmpty(request.PlayerId) || request.NewLevel <= 0)
        {
            return BadRequest(new { message = "Invalid data provided" });
        }

        var playersCollection = _mongoDbService.Database.GetCollection<Player>("Players");
        var player = await playersCollection.Find(p => p.Id == request.PlayerId).FirstOrDefaultAsync();

        if (player == null)
        {
            return NotFound(new { message = "Player not found" });
        }

        if (request.NewLevel > player.HighestLevelReached)
        {
            var update = Builders<Player>.Update.Set(p => p.HighestLevelReached, request.NewLevel);
            await playersCollection.UpdateOneAsync(p => p.Id == request.PlayerId, update);
            return Ok(new { message = "Highest level updated successfully!" });
        }

        return Ok(new { message = "No update needed; level not higher than current highest." });
    }

    public class UpdateLevelRequest
    {
        public string PlayerId { get; set; }
        public int NewLevel { get; set; }
    }
}
