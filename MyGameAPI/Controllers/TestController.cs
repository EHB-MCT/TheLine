using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MyGameAPI.Services;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "Hello from TestController!";
    }
}

