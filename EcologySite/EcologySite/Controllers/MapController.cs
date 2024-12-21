using Microsoft.AspNetCore.Mvc;

namespace EcologySite.Controllers;

public class MapController : Controller
{
    public IActionResult EcologyMap()
    {
        return View();
    }
}
