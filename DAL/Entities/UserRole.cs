using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[PrimaryKey(nameof(UserId), nameof(RoleType))]
public class UserRole
{
    public User User { get; set; }
    public int UserId { get; set; }
    public UserRoleType RoleType { get; set; }
}