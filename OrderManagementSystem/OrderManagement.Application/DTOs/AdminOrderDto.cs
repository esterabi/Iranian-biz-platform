namespace OrderManagement.Application.DTOs;

public class AdminOrderDto
{
    public Guid OrderId { get; set; }
    public string CustomerName { get; set; } = default!;
    public DateTime OrderDate { get; set; }
    public int TotalItems { get; set; }
    public string Status { get; set; } = default!;
}
