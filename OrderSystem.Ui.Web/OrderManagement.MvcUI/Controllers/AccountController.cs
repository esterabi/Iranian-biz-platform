using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.MvcUI.Dtos;
using OrderManagement.MvcUI.Models;

namespace OrderManagement.MvcUI.Controllers;

// Controllers/AccountController.cs
public class AccountController : Controller
{
    private readonly HttpClient _http;

    public AccountController(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("api");
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var userDto = new UserDto
        {
            UserName = model.Email,
            Password = model.Password,
            Role = "Customer"
        };

        var response = await _http.PostAsJsonAsync("api/user/register", userDto);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Login");
        }

        ModelState.AddModelError("", "ثبت‌نام ناموفق بود.");
        return View(model);
    }
    
    // Controllers/AccountController.cs
    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var userDto = new UserDto
        {
            UserName = model.Email,
            Password = model.Password
        };

        var response = await _http.PostAsJsonAsync("api/user/login", userDto);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "ایمیل یا رمز عبور اشتباه است.");
            return View(model);
        }

        var user = await response.Content.ReadFromJsonAsync<UserDto>();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Store");
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
    
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View("~/Views/Shared/AccessDenied.cshtml");
    }
}
