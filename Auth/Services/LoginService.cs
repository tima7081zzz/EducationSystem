using Auth.Models;
using DAL;

namespace Auth.Services;

public interface ILoginService
{
    Task<UserDto?> GetUser(LoginUserDto dto, CancellationToken ct);
};

public class LoginService : ILoginService
{
    private readonly IUnitOfWork _unitOfWork;

    public LoginService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto?> GetUser(LoginUserDto dto, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
        {
            return null;
        }

        //TODO: Add password hashing
        var user = await _unitOfWork.UserRepository.Get(dto.Email, ct);
        if (user is null)
        {
            return null;
        }

        if (user.Password != dto.Password)
        {
            return null;
        }

        return new UserDto
        {
            Id = user.Id,
            Fullname = user.Fullname,
            Email = user.Email
        };
    }
}