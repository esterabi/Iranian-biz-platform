using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.WebAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IOrderService _orderService;

    public AdminController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _orderService.GetAllAsync();
        return Ok(products);
    }
}