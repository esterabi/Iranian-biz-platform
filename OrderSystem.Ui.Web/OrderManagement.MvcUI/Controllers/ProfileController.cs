using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.MvcUI.Dtos;
using OrderManagement.MvcUI.Models;

namespace OrderManagement.MvcUI.Controllers;

// Controllers/ProfileController.cs
[Authorize(Roles = "Customer")]
public class ProfileController : Controller
{
    private readonly HttpClient _http;

    public ProfileController(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("api");
    }

    [HttpGet]
    public async Task<IActionResult> Complete()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var response = await _http.GetAsync($"api/customerprofile/user/{userId}");

        if (response.IsSuccessStatusCode)
        {
            var profile = await response.Content.ReadFromJsonAsync<CustomerProfileDto>();
            if (profile != null)
            {
                var vm = new CustomerProfileViewModel
                {
                    UserId = profile.UserId,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    Mobile = profile.Mobile,
                    Address = profile.Address,
                    Gender = profile.Gender
                };
                return View(vm);
            }
        }

        return View(new CustomerProfileViewModel { UserId = userId });
    }

    [HttpPost]
    public async Task<IActionResult> Complete(CustomerProfileViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = new CustomerProfileDto
        {
            UserId = model.UserId,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Mobile = model.Mobile,
            Address = model.Address,
            Gender = model.Gender
        };

        var response = await _http.PostAsJsonAsync("api/customerprofile", dto);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index", "Store");
        }

        ModelState.AddModelError("", "ثبت اطلاعات ناموفق بود.");
        return View(model);
    }
}
