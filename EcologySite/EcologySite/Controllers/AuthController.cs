using Ecology.Data.Models;
using Ecology.Data.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using EcologySite.Models.Auth;
using EcologySite.Services;
namespace EcologySite.Controllers;
public class AuthController : Controller
{ 
    public IUserRepositryReal _userRepositryReal;

    public AuthController(IUserRepositryReal userRepositryReal)
    {
        _userRepositryReal = userRepositryReal;
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
        _userRepositryReal.Register(
            viewModel.UserName,
            viewModel.Password);

        return RedirectToAction("Login");
    }
    
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