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
    private ICommentRepositoryReal _commentRepositoryReal;

    public EcologyController(IEcologyRepositoryReal ecologyRepository, ICommentRepositoryReal commentRepositoryReal, WebDbContext webDbContext)
    {
        _ecologyRepository = ecologyRepository;
        _commentRepositoryReal = commentRepositoryReal;
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
                    PostId = dbEcology.Id,
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
    
    /*[HttpPost]
    public IActionResult AddComment(int postId, string commentTect, string userId)
    {
       //var userId
        if (ModelState.IsValid && userId != null)
        {
            var comment = new CommentViewModel()
            {
                PostId = postId,
                CommentText = commentText,
                UserId = userId.Value
            };
            _commentRepositoryReal.Add(comment);
            return RedirectToAction("EcologyChat");
        }
        return BadRequest("Invalid comment data.");
    }

    [HttpGet]
    public async Task<IActionResult> CommentsForPost(int postId)
    {
        var comments = await _commentRepositoryReal.Comments
            .Where(c => c.PostId == postId)
            .Include(c => c.User)
            .ToListAsync();
        return View(comments);
    }*/

    [HttpPost]
    public IActionResult AddComment(int postId, string commentText)
    {
        //var userId
        if (ModelState.IsValid)
        {
            var comment = new CommentData()
            {
                PostId = postId, 
                CommentText = commentText
            }; 
            _commentRepositoryReal.Add(comment); 
            return RedirectToAction("EcologyChat");
        } 
        return BadRequest("Invalid comment data.");
    }

    [HttpGet]
    public async Task<IActionResult> CommentsForPost(int postId)
    {
        var comments = await _commentRepositoryReal.Comments 
            .Where(c => c.PostId == postId) 
            .ToListAsync(); 
        return View(comments as string);
    }
}

