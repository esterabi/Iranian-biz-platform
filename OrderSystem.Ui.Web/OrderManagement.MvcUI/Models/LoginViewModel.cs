using System.ComponentModel.DataAnnotations;

namespace OrderManagement.MvcUI.Models;

// Models/LoginViewModel.cs
public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;
}
