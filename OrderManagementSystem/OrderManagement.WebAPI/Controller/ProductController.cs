using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.WebAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// دریافت همه محصولات
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    /// <summary>
    /// دریافت محصول با شناسه
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            return NotFound($"محصولی با شناسه {id} یافت نشد.");

        return Ok(product);
    }

    /// <summary>
    /// ایجاد محصول جدید
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDto productDto)
    {
        if (string.IsNullOrWhiteSpace(productDto.Name) || productDto.StockQuantity < 0)
            return BadRequest("نام محصول و موجودی معتبر الزامی است.");

        var createdProduct = await _productService.CreateAsync(productDto);
        return Ok(createdProduct);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromBody] ProductDto productDto)
    {
        if (string.IsNullOrWhiteSpace(productDto.Name) || productDto.StockQuantity < 0)
            return BadRequest("نام محصول و موجودی معتبر الزامی است.");

        await _productService.Update(productDto);
        return Ok();
    }
    
    
    /*/// <summary>
    /// به‌روزرسانی موجودی محصول
    /// </summary>
    [HttpPut("{id:guid}/stock")]
    public async Task<IActionResult> UpdateStock(Guid id, [FromQuery] int quantity)
    {
        if (quantity < 0)
            return BadRequest("مقدار موجودی نمی‌تواند منفی باشد.");

        var success = await _productService.UpdateStockAsync(id, quantity);
        if (!success)
            return NotFound($"محصولی با شناسه {id} یافت نشد یا به‌روزرسانی انجام نشد.");

        return Ok("موجودی با موفقیت به‌روزرسانی شد.");
    }*/
}