using System.ComponentModel.DataAnnotations;

namespace OrderManagement.MvcUI.Models;
// Models/CustomerProfileViewModel.cs
public class CustomerProfileViewModel
{
    public Guid UserId { get; set; }

    [Required]
    public string FirstName { get; set; } = default!;

    [Required]
    public string LastName { get; set; } = default!;

    [Required]
    [Phone]
    public string Mobile { get; set; } = default!;

    [Required]
    public string Address { get; set; } = default!;

    [Required]
    public string Gender { get; set; } = default!; // "Male", "Female"
}
