using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.MvcUI.Dtos;

namespace OrderManagement.MvcUI.Controllers;

[Authorize(Roles = "Admin")]
public class AdminOrderController : Controller
{
    private readonly HttpClient _http;

    public AdminOrderController(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("api");
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var orders = await _http.GetFromJsonAsync<List<AdminOrderDto>>("api/admin");
        return View(orders);
    }
}
