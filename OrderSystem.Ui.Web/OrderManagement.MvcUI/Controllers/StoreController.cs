using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.MvcUI.Dtos;
using OrderManagement.MvcUI.Extensions;
using OrderManagement.MvcUI.Models;

namespace OrderManagement.MvcUI.Controllers;


public class StoreController : Controller
{
    private readonly HttpClient _http;
    private readonly IHttpContextAccessor _accessor;

    public StoreController(IHttpClientFactory factory, IHttpContextAccessor accessor)
    {
        _http = factory.CreateClient("api");
        _accessor = accessor;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _http.GetFromJsonAsync<List<ProductDto>>("api/product");
        var cart = _accessor.HttpContext?.Session.GetObject<CartViewModel>("Cart") ?? new CartViewModel();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        UserDto user = null; 
        if (userId != null)
        {
         user = await _http.GetFromJsonAsync<UserDto>($"api/user/{userId}");
            
        }

        var model = new StorePageViewModel
        {
            Products = products ?? new(),
            Cart = cart,
            User = user ?? new()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(Guid productId)
    {
        var response = await _http.GetAsync($"api/product/{productId}");
        if (!response.IsSuccessStatusCode) return NotFound();

        var product = await response.Content.ReadFromJsonAsync<ProductDto>();
        var cart = _accessor.HttpContext?.Session.GetObject<CartViewModel>("Cart") ?? new CartViewModel();

        var existing = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (existing != null)
            existing.Quantity += 1;
        else
            cart.Items.Add(new CartItemViewModel
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = 1,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            });

        _accessor.HttpContext?.Session.SetObject("Cart", cart);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder()
    {
        var cart = _accessor.HttpContext?.Session.GetObject<CartViewModel>("Cart");
        if (cart == null || !cart.Items.Any()) return BadRequest("سبد خرید خالی است.");

        var customerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var orderDto = new OrderDto
        {
            CustomerId = customerId,
            Status = "Pending",
            CreatedAt = DateTime.Now,
            Items = cart.Items.Select(i => new OrderItemDto
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };

        var response = await _http.PostAsJsonAsync("api/order", orderDto);
        if (!response.IsSuccessStatusCode) return BadRequest("ثبت سفارش ناموفق بود.");

        _accessor.HttpContext?.Session.Remove("Cart");
        return RedirectToAction("Index");
    }
}
