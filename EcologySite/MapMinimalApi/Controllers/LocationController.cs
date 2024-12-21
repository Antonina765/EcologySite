using Microsoft.AspNetCore.Mvc;
using MapMinimalApi.Data;
using MapMinimalApi.Models;

namespace MapMinimalApi.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationController : Controller
{
    private readonly LocationContext _dbContext;

    public LocationController(LocationContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public List<Location> GetLocations()
    {
        return _dbContext.Locations.ToList();
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<int> AddLocation([FromBody] Location location)
    {
        if (_dbContext
            .Locations
            .OrderByDescending(x => x.Id)
            .Any(x => x.Latitude == location.Latitude && x.Longitude == location.Longitude && x.UserId == location.UserId))
        {
            return Conflict("Duplicate location entry.");
        }

        _dbContext.Locations.Add(location);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(GetLocations), new { id = location.Id }, location.Id);
    }
}