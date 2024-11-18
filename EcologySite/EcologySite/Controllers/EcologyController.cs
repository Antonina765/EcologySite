using System.Diagnostics;
using Ecology.Data;
using Ecology.Data.Interface.Models;
using Ecology.Data.Interface.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ecology.Data.Repositories;
using EcologySite.Models.Ecology;
using Ecology.Data.Models;
using Ecology.Data.Models.Ecology;
using EcologySite.Models;
using EcologySite.Services;


namespace EcologySite.Controllers;

public class EcologyController : Controller
{ 
    private IEcologyRepositoryReal _ecologyRepository;
    private IUserRepositryReal _userRepositryReal;
    private WebDbContext _webDbContext;
    private ICommentRepositoryReal _commentRepositoryReal;
    private AuthService _authService;

    public EcologyController(IEcologyRepositoryReal ecologyRepository, 
        ICommentRepositoryReal commentRepositoryReal,
        IUserRepositryReal userRepositryReal,
        AuthService authService,
        WebDbContext webDbContext)
    {
        _ecologyRepository = ecologyRepository;
        _commentRepositoryReal = commentRepositoryReal;
        _webDbContext = webDbContext;
        _userRepositryReal = userRepositryReal;
        _authService = authService;
    }

    public IActionResult Index()
    {
        var model = new EcologyViewModel();
        
        return View(model);
    }

    [HttpGet]
    public IActionResult EcologyProfile()
    {
        return View();
    }
    [HttpPost]
    public IActionResult EcologyProfile(EcologyProfileViewModel profileViewModel /* я не особо понимаю зачем этот profileViewModel как параметр */)
    {
        var userId = _authService.GetUserId();
        
        if (userId is null)
            throw new Exception("User is not authenticated");
        // нужно просто как то показать в ui что не авторизован, лучше поменять на какую то свою логику в ui условно передать на индекс или на что то еще и написать что мол ошибка

        var info = _commentRepositoryReal.GetCommentAuthors((int)userId);

        var profileModel = new EcologyProfileViewModel();
        
        // дальше нужно просто присвоить все значения, 
        //todo ВАЖНО!!!! тебе нужно мапить CommentData в CommentViewModel и EcologyData в EcologyViewModel !!!!!!!!!
        
        
        
        return View(profileModel);
    }
  
    [HttpGet]
    public IActionResult EcologyChat()
    {
        var currentUserId = _authService.GetUserId();
        if (currentUserId is null)
        {
            return RedirectToAction("Index");
        }

        var user = _userRepositryReal.Get(currentUserId.Value);
        /*if (user.Coins < 150)
        {
            return RedirectToAction("Index");
        }*/
        
        /*if (User.Identity.IsAuthenticated)
        {
            string typeUser;
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role); 
            if (roleClaim != null && roleClaim.Value == "Admin") 
            { 
                typeUser = "Admin";
            } 
            else 
            { 
                typeUser = "User";
            } 
        }*/
        var ecologyFromDb = _ecologyRepository.GetAllWithUsersAndComments();

        var ecologyViewModels = ecologyFromDb
            .Select(dbEcology =>
                new EcologyViewModel
                {
                    PostId = dbEcology.Id,
                    ImageSrc = dbEcology.ImageSrc,
                    Texts = dbEcology.Text,
                    UserName = dbEcology.User?.Login ?? "Unknown",
                    //Text = dbEcology.Comments?.CommentText ?? "Without comments",
                    //CanDelete = typeUser == "Admin" || dbEcology.User?.Id == currentUserId
                    CanDelete = dbEcology.User?.Id == currentUserId
                }
            )
            .ToList();
        return View(ecologyViewModels);
    }

    [HttpPost]
    public IActionResult EcologyChat(PostCreationViewModel viewModel)
    {
        if (CalcCountWorldRepeat.IsEclogyTextHas(viewModel.Text)>=4)
        {
            ModelState.AddModelError(
                nameof(PostCreationViewModel.Text),
                "so similar texts");
        }

        if (!ModelState.IsValid)
        {
            return View("EcologyChat");
        }
        var currentUserId = _authService.GetUserId();
        
        var ecology = new EcologyData
        {
            ImageSrc = viewModel.Url,
            Text = viewModel.Text
        };
        _ecologyRepository.Create(ecology, currentUserId!.Value, viewModel.PostId);
        //_ecologyRepository.Add(ecology);
            
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
        return View(comments as string);
    }*/

    [HttpPost]
    public IActionResult AddComment(int postId, string commentText)
    {
        //var userId
        if (!ModelState.IsValid) return BadRequest("Invalid comment data.");
        var comment = new CommentData()
        {
            PostId = postId, 
            CommentText = commentText
        }; 
        _commentRepositoryReal.Add(comment); 
        return RedirectToAction("EcologyChat");
    }

    [HttpGet]
    public IActionResult CommentsForPost(int postId)
    {
        var comm = _commentRepositoryReal.GetCommentsByPostId(postId);
        
        return View(comm);
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
