namespace OrderManagement.Domain.Entities;

public class CustomerOrderView
{
    public string Status { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public Guid CustomerId { get; set; }
    public Guid OrderId { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public double Fee { get; set; }
    public DateTime CreatedAt { get; set; }
}