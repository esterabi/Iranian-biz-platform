namespace OrderManagement.MvcUI.Models;

// Models/CartItemViewModel.cs
public class CartItemViewModel
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public int Quantity { get; set; }
    public double Price { get; set; }
    public int StockQuantity { get; set; }
}
