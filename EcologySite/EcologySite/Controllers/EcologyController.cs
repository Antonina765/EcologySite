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
using EcologySite.Controllers.AuthAttributes;
using EcologySite.Models;
using EcologySite.Services;
using Enums.Users;
using Ecology.Data.DataLayerModels;
using EcologySite.Hubs;
using Microsoft.AspNetCore.SignalR;


namespace EcologySite.Controllers;

public class EcologyController : Controller
{ 
    private IEcologyRepositoryReal _ecologyRepository;
    private IUserRepositryReal _userRepositryReal;
    private WebDbContext _webDbContext;
    private ICommentRepositoryReal _commentRepositoryReal;
    private AuthService _authService;
    private IWebHostEnvironment _webHostEnvironment;
    public IHubContext<ChatHub, IChatHub> _chatHub;
    
    public EcologyController(IEcologyRepositoryReal ecologyRepository, 
        ICommentRepositoryReal commentRepositoryReal,
        IUserRepositryReal userRepositryReal,
        AuthService authService,
        WebDbContext webDbContext,
        IWebHostEnvironment webHostEnvironment,
        IHubContext<ChatHub, IChatHub> hubContext)
    {
        _ecologyRepository = ecologyRepository;
        _commentRepositoryReal = commentRepositoryReal;
        _webDbContext = webDbContext;
        _userRepositryReal = userRepositryReal;
        _authService = authService;
        _webHostEnvironment = webHostEnvironment;
        _chatHub = hubContext;
    }

    public IActionResult Index()
    {
        var mainPagePosts = _ecologyRepository.GetAllWithUsersAndComments() 
            .Where(p => p.ForMainPage == 1) 
            .Select(post => new EcologyViewModel 
            { 
                PostId = post.Id, 
                ImageSrc = post.ImageSrc, 
                Texts = post.Text, 
                UserName = post.User?.Login ?? "Unknown", 
                CanDelete = false, // Перенесенные посты не могут быть удалены
                CanMove = false // Перенесенные посты не могут быть снова перенесены
            }) 
            .ToList(); 
        
        var viewModel = new MovedPostsViewModel
        {
            Posts = mainPagePosts
        };
        
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult SetForMainPage(Type postId)
    {
        _ecologyRepository.SetForMainPage(postId);
        return RedirectToAction("EcologyChat");
    }
    
    /* Метод для установки поста на главную страницу, доступный только для администраторов
    [HttpPost] 
    public IActionResult SetForMainPage(int postId) 
    { 
        var userRole = User.IsInRole("admin") ? "admin" : "user"; 
        _ecologyRepository.SetForMainPage(postId, userRole); 
        return RedirectToAction("Index"); 
    }*/
    
    [HttpGet]
    public IActionResult EcologyProfile()
    {
        var viewModel = new EcologyProfileViewModel();
        var userId = _authService.GetUserId();

        if (userId != null)
        {
            viewModel.AvatarUrl = _userRepositryReal.GetAvatarUrl(userId!.Value);

            var info = _commentRepositoryReal.GetCommentAuthors((int)userId);

            if (info != null)
            {
                viewModel.Comments = info 
                    .Comments 
                    .Select(dbComment => new CommentForProfileViewModel()
                    {
                        CommentId = dbComment.Id, 
                        CommentText = dbComment.CommentText
                    }) .ToList(); 
                
                viewModel.Posts = info 
                    .Posts 
                    .Select(dbPost => new EcologyForProfileViewModel
                    {
                        ImageSrc = dbPost.ImageSrc, 
                        Texts = dbPost.Text,
                    }) .ToList();
            }
            else
            {
                viewModel.Comments = new List<CommentForProfileViewModel>(); 
                viewModel.Posts = new List<EcologyForProfileViewModel>();
            }
        }
        else 
        {
            viewModel.UserName = "Guest";
            viewModel.AvatarUrl = "~/images/Ecology/defaltavatar.JPG";
            viewModel.Posts = new List<EcologyForProfileViewModel>();
            viewModel.Comments = new List<CommentForProfileViewModel>();
        }
        
        return View(viewModel);
    }
    
    [HttpGet]
    public IActionResult EcologyMap()
    {
        var viewModel = new LocationViewModel();

        var userName = _authService.GetName();
        var userId = _authService.GetUserId();
            
        viewModel.UserName = userName;
        viewModel.UserId = userId ?? -1;
            
        return View(viewModel);
    }
    
    [HttpGet]
    public IActionResult EcologyChat()
    {
        var posts = _ecologyRepository.GetPostsForMainPage().ToArray();
        var ecologyFromDb = _ecologyRepository.GetAllWithUsersAndComments();
        var currentUserId = _authService.GetUserId();
        var isAdmin = User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin"); 
        
        if (currentUserId is null)
        {
            return RedirectToAction("Index");
        }

        var user = _userRepositryReal.Get(currentUserId.Value);
        
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

        var ecologyViewModels = ecologyFromDb
            .Select(dbEcology =>
                new EcologyViewModel
                {
                    PostId = dbEcology.Id,
                    ImageSrc = dbEcology.ImageSrc,
                    Texts = dbEcology.Text,
                    UserName = dbEcology.User?.Login ?? "Unknown",
                    //Text = dbEcology.Comments?.CommentText ?? "Without comments",
                    CanDelete = dbEcology.User?.Id == currentUserId || isAdmin,
                    CanMove = isAdmin,
                    PostsForMainPage = dbEcology.ForMainPage == 1,
                    LikeCount = dbEcology.UsersWhoLikeIt.Count(),                                                                                                     
                    IsLiked = dbEcology.UsersWhoLikeIt.Any(x => x.UserId == user.Id)
                }
            )
            .ToList();

        for (int i = 0; i < ecologyViewModels.Count; i++)
        {
            ecologyViewModels[i].ForMainPage = posts[i].ForMainPage;
        }

        var viewModel = new PostViewModel
        {
            Ecologies = ecologyViewModels,
            Posts = new List<PostCreationViewModel>()
        };
        
        return View(viewModel);
    }
    
    [HttpPost] //Это и есть создание 
    public IActionResult EcologyChat(PostCreationViewModel viewModel, IFormFile imageFile)
    {
        if (CalcCountWorldRepeat.IsEclogyTextHas(viewModel.Text) >= 4)
        {
            ModelState.AddModelError(nameof(PostCreationViewModel.Text), "so similar texts");
        }

        if (!ModelState.IsValid)
        {
            return View("EcologyChat");
        }

        var currentUserId = _authService.GetUserId();
        
        string imageUrl = null; //изначально null для того, чтобы затем получить либо URL, либо путь к загруженному изображению с компьютера. Для того, чтобы  использовать одно из значений в объекте EcologyData
        
        if (imageFile != null && imageFile.Length > 0)
        {
            var webRootPath = _webHostEnvironment.WebRootPath; 
            var fileName = Path.GetFileNameWithoutExtension(imageFile.FileName); 
            var extension = Path.GetExtension(imageFile.FileName); 
            var newFileName = $"{fileName}-{currentUserId}{extension}";
            var path = Path.Combine(webRootPath, "images", "uploads", newFileName);
        
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }
            imageUrl = $"/images/Ecology/ecologyPosts/{newFileName}";
        }
        else if (!string.IsNullOrEmpty(viewModel.Url))
        {
            imageUrl = viewModel.Url;
        }
        else
        {
            ModelState.AddModelError("", "Please provide either an image URL or upload an image."); 
            return View("EcologyChat");
        }

        var ecology = new EcologyData
        {
            ImageSrc = imageUrl,
            Text = viewModel.Text
        };

        _ecologyRepository.Create(ecology, currentUserId!.Value, viewModel.PostId);
        //_ecologyRepository.Add(ecology);

        // Отправка уведомления о новом посте
        var userName = _authService.GetName();
        _chatHub.Clients.All.NewMessageAdded($"User {viewModel.UserName} create a new post: {viewModel.Text}");
        
        return RedirectToAction("EcologyChat");
    }
    
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
    
    public IActionResult Forbidden()
    {
        return View();
    }
    
    [IsAuthenticated]
    public IActionResult UpdateLocale(Language language)
    {
        var userId = _authService.GetUserId()!.Value;
        _userRepositryReal.UpdateLocal(userId, language);

        return RedirectToAction("Index");
    }
    
    [IsAuthenticated]
    [HttpPost]
    public IActionResult UpdateAvatar(IFormFile avatar)
    {
        var webRootPath = _webHostEnvironment.WebRootPath;

        var userId = _authService.GetUserId()!.Value;
        var avatarFileName = $"avatar-{userId}.jpg";

        var path = Path.Combine(webRootPath, "images", "avatars", avatarFileName);
        using (var fileStream = new FileStream(path, FileMode.Create))
        {
            avatar
                .CopyToAsync(fileStream)
                .Wait();
        }

        var avatarUrl = $"/images/avatars/{avatarFileName}";
        _userRepositryReal.UpdateAvatarUrl(userId, avatarUrl);

        return RedirectToAction("EcologyProfile");
    }
}
