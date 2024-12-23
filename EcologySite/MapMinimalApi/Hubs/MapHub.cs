using MapMinimalApi.Data;
using Microsoft.AspNetCore.SignalR;
using MapMinimalApi.Models;

namespace MapMinimalApi.Hubs;

public interface IMapHub
{
    Task UpdateMap(Location location);
}

public class MapHub : Hub<IMapHub>
{
    private LocationContext _locationContext;

    public MapHub(LocationContext context)
    {
        _locationContext = context;
    }

    public async Task UpdateMap(string userName, double latitude, double longitude)
    {
        var location = new Location { UserName = userName, Latitude = latitude, Longitude = longitude };
        _locationContext.Locations.Add(location);
        await _locationContext.SaveChangesAsync();
        await Clients.All.UpdateMap(location);
    }
}
