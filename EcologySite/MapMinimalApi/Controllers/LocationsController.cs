using Microsoft.AspNetCore.Mvc;
using MapMinimalApi.Data;
using MapMinimalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MapMinimalApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly LocationContext _context;

    public LocationsController(LocationContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
    {
        return await _context.Locations.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Location>> PostLocation([FromBody] Location location)
    {
        await _context.Locations.AddAsync(location);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetLocations), new { id = location.Id }, location);
    }
}
