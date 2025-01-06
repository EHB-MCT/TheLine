using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MyGameAPI.Services;

// This controller serves as a basic test endpoint to verify that the API is working correctly.
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    // Simple GET endpoint that returns a string message.
    [HttpGet]
    public string Get()
    {
        // Return a welcome message from the TestController
        return "Hello from TestController!";
    }
}
