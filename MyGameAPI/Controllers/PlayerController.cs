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

        return Ok(new { message = "Login successful!", playerId = player.Id });
    }
}
