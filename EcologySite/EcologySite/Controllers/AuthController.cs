using Ecology.Data.Models;
using Ecology.Data.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using EcologySite.Models.Auth;
using EcologySite.Services;
using Microsoft.AspNetCore.SignalR;
using EcologySite.Hubs;

namespace EcologySite.Controllers;
public class AuthController : Controller
{ 
    public IUserRepositryReal _userRepositryReal;
    private IWebHostEnvironment _webHostEnvironment;
    public IHubContext<ChatHub, IChatHub> _chatHub;

    public AuthController(IUserRepositryReal userRepositryReal, 
        IWebHostEnvironment webHostEnvironment, 
        IHubContext<ChatHub, IChatHub> hubContext)
    {
        _userRepositryReal = userRepositryReal;
        _webHostEnvironment = webHostEnvironment;
        _chatHub = hubContext;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginUserViewModel viewModel)
    {
        var user = _userRepositryReal.Login(viewModel.UserName, viewModel.Password);
            
        if (user is null)
        {
            ModelState.AddModelError(
                nameof(viewModel.UserName), 
                "Не правильный логин или пароль");
        }

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        //Good user

        var claims = new List<Claim>()
        {
            new Claim(AuthService.CLAIM_TYPE_ID, user.Id.ToString()),
            new Claim(AuthService.CLAIM_TYPE_NAME, user.Login),
            new Claim(AuthService.CLAIM_TYPE_ROLE, ((int)user.Role).ToString()),
            new Claim (ClaimTypes.AuthenticationMethod, AuthService.AUTH_TYPE_KEY),
        };

        var identity = new ClaimsIdentity(claims, AuthService.AUTH_TYPE_KEY);

        var principal = new ClaimsPrincipal(identity);

        HttpContext
            .SignInAsync(principal)
            .Wait();

        return RedirectToAction("EcologyProfile", "Ecology");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Register(RegisterUserViewModel viewModel)
    {
        if (!_userRepositryReal.CheckIsNameAvailable(viewModel.UserName))
        {
            return View(viewModel);
        }
        
        _userRepositryReal.Register(
            viewModel.UserName,
            viewModel.Password);

        _chatHub.Clients.All.NewMessageAdded($"Новый пользователь зарегестировался у нас на сайте. Его зовут {viewModel.UserName}");

        return RedirectToAction("Login");
    }
    /*public IActionResult Register(RegisterUserViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            string avatarUrl = null;

            if (viewModel.AvatarImage != null && viewModel.AvatarImage.Length > 0)
            {
                var webRootPath = _webHostEnvironment.WebRootPath;
                var fileName = Path.GetFileNameWithoutExtension(viewModel.AvatarImage.FileName);
                var extension = Path.GetExtension(viewModel.AvatarImage.FileName);
                var newFileName = $"{fileName}-{Guid.NewGuid()}{extension}";
                var path = Path.Combine(webRootPath, "images", "avatars", newFileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    viewModel.AvatarImage.CopyTo(fileStream);
                }
                avatarUrl = $"/images/avatars/{newFileName}";
            }

            _userRepositryReal.Register(viewModel.UserName, viewModel.Password, avatarUrl);

            return RedirectToAction("Login");
        }

        return View(viewModel);
    }*/

    
    [HttpPost] 
    public IActionResult RegisterFromProfile(RegisterUserViewModel viewModel) 
    { 
        if (ModelState.IsValid) 
        { 
            ModelState.AddModelError("", "Registration failed. Please try again."); 
        } 
        // Assuming you redirect back to the profile page if there's an error.
        return RedirectToAction("EcologyProfile", "Ecology"); 
    }

    public IActionResult Logout()
    {
        HttpContext
            .SignOutAsync()
            .Wait();

        return RedirectToAction("EcologyProfile", "Ecology");
    }
}