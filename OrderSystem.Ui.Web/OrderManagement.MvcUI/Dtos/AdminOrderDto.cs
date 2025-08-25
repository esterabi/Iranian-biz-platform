namespace OrderManagement.MvcUI.Dtos;

public class AdminOrderDto
{
    public string Status { get; set; }
    public double Price { get; set; }
    public double Fee { get; set; }
    public int Quantity { get; set; }
    public string CustomerName { get; set; }
    public string Description { get; set; }
    public string CustomerId { get; set; }
    public string OrderId { get; set; }
    public string ProductName { get; set; }
    public DateTime CreatedAt { get; set; }
}
