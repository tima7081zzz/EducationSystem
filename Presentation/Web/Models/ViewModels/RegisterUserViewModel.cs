using System.ComponentModel.DataAnnotations;

namespace Web.Models.ViewModels;

public class RegisterUserViewModel
{
    [Required]
    public string Fullname { get; set; }
    
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }

    public string ReturnUrl { get; set; }
}