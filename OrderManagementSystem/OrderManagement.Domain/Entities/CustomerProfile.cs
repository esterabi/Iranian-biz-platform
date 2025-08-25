using OrderManagement.Domain.Common;

namespace OrderManagement.Domain.Entities;


public class CustomerProfile : BaseEntity
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Mobile { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Gender { get; set; } = default!; // "Male", "Female"

    public User User { get; set; } = default!;
}