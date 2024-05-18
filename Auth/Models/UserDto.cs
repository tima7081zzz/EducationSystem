namespace Auth.Models;

public class UserDto
{
    public int Id { get; set; }
    public required string Fullname { get; set; }
    public required string Email { get; set; }
}