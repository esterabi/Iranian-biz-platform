using OrderManagement.Domain.Common;

namespace OrderManagement.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public string Category { get; set; } = default!;
    public int StockQuantity { get; set; }
    public double Price { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}