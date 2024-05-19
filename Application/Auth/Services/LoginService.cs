using Auth.Models;
using Core.Exceptions;
using DAL;
using DAL.Entities;

namespace Auth.Services;

public interface ILoginService
{
    Task<UserDto?> LoginUser(LoginUserDto dto, CancellationToken ct);
    Task<UserDto?> RegisterUser(RegisterUserDto dto, CancellationToken ct);
};

public class LoginService : ILoginService
{
    private readonly IUnitOfWork _unitOfWork;

    public LoginService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto?> LoginUser(LoginUserDto dto, CancellationToken ct)
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
    
    public async Task<UserDto?> RegisterUser(RegisterUserDto dto, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password) ||
            string.IsNullOrWhiteSpace(dto.Fullname))
        {
            return null;
        }

        var user = await _unitOfWork.UserRepository.Get(dto.Email, ct);
        if (user is not null)
        {
            throw new AlreadyExistsException();
        }

        user = _unitOfWork.UserRepository.Add(new User
        {
            Fullname = dto.Fullname,
            Password = dto.Password,
            Email = dto.Email,
            CreatedAt = DateTimeOffset.UtcNow
        });

        await _unitOfWork.SaveChanges(ct);

        return new UserDto
        {
            Id = user.Id,
            Fullname = user.Fullname,
            Email = user.Email
        };
    }
}