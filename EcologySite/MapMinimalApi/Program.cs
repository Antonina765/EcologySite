using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MapMinimalApi.Data;
using MapMinimalApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<LocationContext>(opt => opt.UseNpgsql(LocationContext.CONNECTION_STRING));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MapMinimalApi v1"));
}

app.UseHttpsRedirection();

app.MapGet("/api/locations", async (LocationContext db) =>
{
    return await db.Locations.ToListAsync();
});

app.MapPost("/api/locations", async (LocationContext db, Location location) =>
{
    db.Locations.Add(location);
    await db.SaveChangesAsync();
    return Results.Created($"/api/locations/{location.Id}", location);
});

app.Run();