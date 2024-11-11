using Ecology.Data;
using Ecology.Data.Interface.Models;
using Ecology.Data.Interface.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ecology.Data.Repositories;
using EcologySite.Models.Ecology;
using Ecology.Data.Models;
using Ecology.Data.Models.Ecology;


namespace EcologySite.Controllers;

public class EcologyController : Controller
{ 
    private IEcologyRepositoryReal _ecologyRepository;
    private WebDbContext _webDbContext;

    public EcologyController(IEcologyRepositoryReal ecologyRepository, WebDbContext webDbContext)
    {
        _ecologyRepository = ecologyRepository;
        _webDbContext = webDbContext;
    }

    public IActionResult Index()
    {
        var model = new EcologyViewModel();
        return View(model);
    }

    [HttpGet]
    public IActionResult EcologyChat()
    {
        var ecologyFromDb = _ecologyRepository.GetAll();

        var ecologyViewModels = ecologyFromDb
            .Select(dbEcology =>
                new EcologyViewModel
                {
                    Id = dbEcology.Id,
                    ImageSrc = dbEcology.ImageSrc,
                    Texts = dbEcology.Text
                }
            )
            .ToList();
        return View(ecologyViewModels);
    }

    [HttpPost]
    public IActionResult EcologyChat(PostCreationViewModel viewModel)
    {
        var ecology = new EcologyData
        {
            ImageSrc = viewModel.Url,
            Text = viewModel.Text
        };
        _ecologyRepository.Add(ecology);
            
        return RedirectToAction("EcologyChat");
    }
        
    [HttpPost]
    public IActionResult UpdatePost(int id, string url, string text)
    {
        _ecologyRepository.UpdatePost(id, url, text);
        return RedirectToAction("EcologyChat");
    }

    [HttpPost]
    public IActionResult Remove(int id)
    {
        _ecologyRepository.Delete(id);
        return RedirectToAction("EcologyChat");
    }
}