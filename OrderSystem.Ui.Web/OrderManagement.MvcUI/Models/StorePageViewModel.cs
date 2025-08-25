using OrderManagement.MvcUI.Dtos;

namespace OrderManagement.MvcUI.Models;

public class StorePageViewModel
{
    public List<ProductDto> Products { get; set; } = new();
    public CartViewModel Cart { get; set; } = new();
    public UserDto User { get; set; } = new();
}
