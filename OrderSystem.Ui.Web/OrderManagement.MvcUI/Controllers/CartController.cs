using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.MvcUI.Dtos;
using OrderManagement.MvcUI.Extensions;
using OrderManagement.MvcUI.Models;

namespace OrderManagement.MvcUI.Controllers;

// Controllers/CartController.cs
[Authorize(Roles = "Customer")]
public class CartController : Controller
{
    private readonly IHttpContextAccessor _accessor;
    private readonly HttpClient _http;

    public CartController(IHttpClientFactory factory, IHttpContextAccessor accessor)
    {
        _http = factory.CreateClient("api");
        _accessor = accessor;
    }

    public async Task<IActionResult> MyOrders(Guid productId)
    {
        var response = await _http.GetAsync($"api/product/{productId}");
        if (!response.IsSuccessStatusCode) return NotFound();
        var orders = await response.Content.ReadFromJsonAsync<OrderDto>();
        return View(orders);
    }

    
    public async Task<IActionResult> Index()
    {
        var cart = _accessor.HttpContext?.Session.GetObject<CartViewModel>("Cart") ?? new CartViewModel();
        return View(cart);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCart(Guid productId)
    {
        var cart = _accessor.HttpContext?.Session.GetObject<CartViewModel>("Cart") ?? new CartViewModel();
        var existing = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (existing != null)
            cart.Items.Remove(existing);
        
        _accessor.HttpContext?.Session.SetObject("Cart", cart);
        return RedirectToAction("Index");        
    }
    [HttpPost]
    public async Task<IActionResult> AddToCart(Guid productId, int quantity)
    {
        var response = await _http.GetAsync($"api/product/{productId}");
        if (!response.IsSuccessStatusCode) return NotFound();

        var product = await response.Content.ReadFromJsonAsync<ProductDto>();
        if (product == null || product.StockQuantity < quantity) return BadRequest("موجودی کافی نیست.");

        var cart = _accessor.HttpContext?.Session.GetObject<CartViewModel>("Cart") ?? new CartViewModel();

        var existing = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (existing != null)
            existing.Quantity += quantity;
        else
            cart.Items.Add(new CartItemViewModel
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = quantity,
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
            Items = cart.Items.Select(i => new OrderItemDto
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };

        var response = await _http.PostAsJsonAsync("api/order", orderDto);
        if (!response.IsSuccessStatusCode)
            return BadRequest("ثبت سفارش ناموفق بود.");

        _accessor.HttpContext?.Session.Remove("Cart");
        return RedirectToAction("MyOrders", "Order");
    }
}
