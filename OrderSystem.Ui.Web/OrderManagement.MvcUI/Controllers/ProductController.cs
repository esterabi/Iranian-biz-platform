using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.MvcUI.Dtos;
using OrderManagement.MvcUI.Extensions;
using OrderManagement.MvcUI.Models;

namespace OrderManagement.MvcUI.Controllers;

[Authorize]
public class ProductController : Controller
{
    private readonly HttpClient _http;

    public ProductController(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("api");
    }


    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _http.GetAsync("api/product");
        if (response.IsSuccessStatusCode)
        {
            var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
            return View(products);
        }
        return View(new List<ProductDto>());
    }


    
    public async Task<IActionResult> Create()
    {
        return View();
    }

    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(ProductViewModel productViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(productViewModel);
        }

        // آپلود تصویر
        string imageUrl = string.Empty;
        if (productViewModel.Image != null)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(productViewModel.Image.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await productViewModel.Image.CopyToAsync(stream);
            }
            imageUrl = $"/images/{fileName}";
        }

        var dto = new ProductDto()
        {
            Name = productViewModel.Name,
            Description = productViewModel.Description,
            Price = productViewModel.Price,
            StockQuantity = productViewModel.StockQuantity,
            Category = productViewModel.Category,
            ImageUrl = imageUrl
        };

        var response = await _http.PostAsJsonAsync("api/product", dto);
        
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("GetAll");
        }

        ModelState.AddModelError(string.Empty, "خطا در ارسال داده به سرویس");
        return View(productViewModel);
    }

    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Details(Guid id)
    {
        var product = await _http.GetFromJsonAsync<ProductDto>($"api/product/{id}");
        if (product == null) return NotFound();

        return View(product);
    }

    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(string id)
    {
        var response = await _http.GetAsync($"api/product/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var product = await response.Content.ReadFromJsonAsync<ProductDto>();
        
        var viewModel = new ProductEditViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Category = product.Category,
            StockQuantity = product.StockQuantity,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
        };

        return View(viewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(Guid id, ProductEditViewModel productViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(productViewModel);
        }

        string imageUrl = productViewModel.ImageUrl;
        if (productViewModel.Image != null)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // آپلود تصویر جدید
            var fileName = Guid.NewGuid() + Path.GetExtension(productViewModel.Image.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await productViewModel.Image.CopyToAsync(stream);
            }
            imageUrl = $"/images/{fileName}";
        }

        var dto = new ProductDto()
        {
            Id = id,
            Name = productViewModel.Name,
            Description = productViewModel.Description,
            Price = productViewModel.Price,
            StockQuantity = productViewModel.StockQuantity,
            Category = productViewModel.Category,
            ImageUrl = imageUrl
        };

        var response = await _http.PutAsJsonAsync($"api/product/{id}", dto);
        
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("GetAll");
        }

        ModelState.AddModelError(string.Empty, "خطا در به‌روزرسانی محصول");
        return View(productViewModel);
    }
    
    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> AddToCart(Guid productId, int quantity)
    {
        var response = await _http.GetAsync($"api/product/{productId}");
        if (!response.IsSuccessStatusCode) return NotFound();

        var product = await response.Content.ReadFromJsonAsync<ProductDto>();
        if (product == null || product.StockQuantity < quantity) return BadRequest("موجودی کافی نیست.");

        var cart = HttpContext.Session.GetObject<CartViewModel>("Cart") ?? new CartViewModel();

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

        HttpContext.Session.SetObject("Cart", cart);
        return RedirectToAction("Index", "Home");
    }
}