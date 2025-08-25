using OrderManagement.Domain.Common;

namespace OrderManagement.Domain.Entities;


public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }

    public Order Order { get; set; } = default!;
    public Product Product { get; set; } = default!;
}