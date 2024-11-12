using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EcologySite.Models;
using EcologySite.Models.Home;
using EcologySite.Services;

namespace EcologySite.Controllers;

public class HomeController : Controller
{
    //private readonly ILogger<HomeController> _logger;
    private AuthService _authService;

    public HomeController(AuthService authService)
    {
        _authService = authService;
    }

    public IActionResult Index()
    {
        var viewModel = new IndexViewModel();

        var userName = _authService.GetName();

        viewModel.UserName = userName;

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}