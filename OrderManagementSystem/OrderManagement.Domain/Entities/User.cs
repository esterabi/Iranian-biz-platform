using OrderManagement.Domain.Common;

namespace OrderManagement.Domain.Entities;

public class User : BaseEntity
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public DateTime CreateDate { get; set; }
    public string Role { get; set; } = "Customer"; // "Customer", "Admin"

    public CustomerProfile? Profile { get; set; }
}