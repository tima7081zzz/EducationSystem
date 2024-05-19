namespace Auth.Models;

public class RegisterUserDto
{
    public required string Fullname { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}