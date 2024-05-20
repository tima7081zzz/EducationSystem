using System.ComponentModel.DataAnnotations;

namespace Web.Models.ViewModels;

public class RegisterUserViewModel
{
    [Required]
    public required string Fullname { get; set; }
    
    [Required]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public required string ConfirmPassword { get; set; }

    public required string ReturnUrl { get; set; }
}