[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly MongoDbService _mongoDbService;

    public PlayersController(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPlayers()
    {
        var players = await _mongoDbService.GetPlayersCollection().Find(_ => true).ToListAsync();
        return Ok(players);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlayer([FromBody] Player newPlayer)
    {
        await _mongoDbService.GetPlayersCollection().InsertOneAsync(newPlayer);
        return Created("", newPlayer);
    }
}
