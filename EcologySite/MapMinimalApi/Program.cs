using MapMinimalApi.Data;
using MapMinimalApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi.Models;
using MapMinimalApi.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddDbContext<LocationContext>(options =>
    options.UseNpgsql(LocationContext.CONNECTION_STRING));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MapMinimalApi", Version = "v1" });
});

builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.SetIsOriginAllowed(origin => true);
        policy.AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MapMinimalApi V1");
    c.RoutePrefix = string.Empty; 
});

app.MapHub<MapHub>("/hub/map");

app.Run();