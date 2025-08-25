namespace OrderManagement.Application.DTOs;

public class OrderDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrdersListDto
{
    public string Status { get; set; }
    public double Price { get; set; }
    public double Fee { get; set; }
    public int Quantity { get; set; }
    public string CustomerName { get; set; }
    public string Description { get; set; }
    public Guid CustomerId { get; set; }
    public Guid OrderId { get; set; }
    public string ProductName { get; set; }
    public DateTime CreatedAt { get; set; }
}