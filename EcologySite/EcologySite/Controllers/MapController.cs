using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using EcologySite.Controllers;
using EcologySite.Models.Ecology;
using EcologySite.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EcologySite.Controllers;
[ApiController]
[Route("[controller]")]
public class LocationController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AuthService _authService;

    public LocationController(IHttpClientFactory httpClientFactory, AuthService authService)
    {
        _httpClientFactory = httpClientFactory;
        _authService = authService;
    }

    [HttpGet("getLocations")]
    public async Task<IActionResult> GetLocations()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("http://localhost:5173/location"); // URL минимального API

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var locations = JsonConvert.DeserializeObject<List<Location>>(content);
            return Ok(locations);
        }

        return StatusCode((int)response.StatusCode);
    }

    [HttpPost("addLocation")]
    public async Task<IActionResult> AddLocation([FromBody] LocationViewModel viewModel)
    {
        var userId = _authService.GetUserId();
        var userName = _authService.GetName();

        viewModel.UserId = userId;
        viewModel.UserName = userName;

        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync("http://localhost:5173/location", viewModel); // URL минимального API

        if (response.IsSuccessStatusCode)
        {
            return Ok();
        }

        return StatusCode((int)response.StatusCode);
    }
    
    public class Location
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
