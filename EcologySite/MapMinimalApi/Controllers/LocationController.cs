using Microsoft.AspNetCore.Mvc;
using MapMinimalApi.Data;
using MapMinimalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MapMinimalApi.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationController : Controller
{
    private LocationContext _dbContext;

    public LocationController(LocationContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<List<Location>> GetLocations()
    {
        return await _dbContext.Locations.ToListAsync();
    }
    
    [HttpPost]
    public async Task<ActionResult<Location>> PostLocation(Location location)
    {
        _dbContext.Locations.Add(location);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLocations), new { id = location.Id }, location);
    }
}