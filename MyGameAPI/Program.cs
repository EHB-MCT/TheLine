using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using MyGameAPI.Services; // Ensure you're using the correct namespace for MongoDbService
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(); // Add support for controllers
builder.Services.AddSingleton<MongoDbService>();

// Add CORS to allow Unity to access the API from a different origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Set up CORS (using the defined policy)
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();

// Map controllers (make sure controllers are used)
app.MapControllers();

app.MapGet("/", () => "API is running!");

app.Run();
