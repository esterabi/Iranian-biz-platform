using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<ProductDto> CreateAsync(ProductDto productDto);
    Task Update(ProductDto productDto);
    Task<ProductDto?> GetByIdAsync(Guid id);
    Task<bool> UpdateStockAsync(Guid productId, int quantity);
}