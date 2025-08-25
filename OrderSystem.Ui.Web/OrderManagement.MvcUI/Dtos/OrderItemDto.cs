namespace OrderManagement.MvcUI.Dtos;

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public double Price  { get; set; }
}