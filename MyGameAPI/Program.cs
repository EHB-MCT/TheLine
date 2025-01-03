using MyGameAPI.Services; // Zorg ervoor dat je de juiste namespace gebruikt voor MongoDbService

var builder = WebApplication.CreateBuilder(args);

// Voeg services toe aan de container
builder.Services.AddSingleton<MongoDbService>();

// Voeg CORS toe als je Unity toegang wil geven tot de API vanaf een andere origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configureer de HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Stel CORS in (gebruik de gedefinieerde policy)
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers(); // Zorg dat je controllers gemapt worden als je ze gebruikt

app.Run();
