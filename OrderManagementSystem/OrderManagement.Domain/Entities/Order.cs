using OrderManagement.Domain.Common;

namespace OrderManagement.Domain.Entities;

public class Order : BaseEntity
{
    public Guid CustomerId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Registered"; // "Registered", "Cancelled", "Paid"

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}