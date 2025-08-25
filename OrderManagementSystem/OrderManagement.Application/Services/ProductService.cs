using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Persistence.Context;

namespace OrderManagement.Application.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// دریافت همه محصولات
    /// </summary>
    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _context.Products.ToListAsync();

        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            ImageUrl = p.ImageUrl,
            Category = p.Category,
            StockQuantity = p.StockQuantity,
            Price = p.Price,
        });
    }

    public async Task Update(ProductDto productDto)
    {
        var product = await _context.Products.FindAsync(productDto.Id);
        if (product != null)
        {
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.ImageUrl = productDto.ImageUrl;
            product.Category = productDto.Category;
            product.StockQuantity = productDto.StockQuantity;
            product.Price = productDto.Price;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// دریافت محصول با شناسه
    /// </summary>
    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return null;

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            Category = product.Category,
            StockQuantity = product.StockQuantity,
            Price = product.Price,
        };
    }

    /// <summary>
    /// ایجاد محصول جدید
    /// </summary>
    public async Task<ProductDto> CreateAsync(ProductDto productDto)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = productDto.Name,
            Description = productDto.Description,
            ImageUrl = productDto.ImageUrl,
            Category = productDto.Category,
            StockQuantity = productDto.StockQuantity,
            Price = productDto.Price
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        productDto.Id = product.Id;
        return productDto;
    }

    /// <summary>
    /// به‌روزرسانی موجودی محصول
    /// </summary>
    public async Task<bool> UpdateStockAsync(Guid productId, int quantity)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
            return false;

        product.StockQuantity = quantity;
        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        return true;
    }
}