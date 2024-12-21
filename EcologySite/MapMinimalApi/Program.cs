using MapMinimalApi.Data;
using MapMinimalApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LocationContext>(options =>
    options.UseNpgsql(LocationContext.CONNECTION_STRING));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MapMinimalApi", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MapMinimalApi v1");
    c.RoutePrefix = string.Empty;  // Делает Swagger UI доступным по корню приложения
});

// сохранение местоположения
app.MapPost("/location", async (Location location, LocationContext db) =>
    {
        db.Locations.Add(location);
        await db.SaveChangesAsync();
        return Results.Ok(location);
    })
    .WithName("AddLocation")
    .WithTags("Location");

app.Run();

public class Location
{
    public int Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}