using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Index(nameof(Email))]
public class User
{
    [Key]
    public int Id { get; set; }
    public required string Fullname { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    //public ICollection<UserRole> UserRoles { get; set; } = Array.Empty<UserRole>();
    /*public ICollection<Professor> Professors { get; set; }
    public ICollection<UserActivity> UserActivities { get; set; }
    public ICollection<UserGroup> UserGroups { get; set; }*/
}

public enum UserRoleType : byte
{
    [Description("Owner")]
    Owner = 0,

    [Description("Professor")]
    Professor = 1,

    [Description("Student")]
    Student = 2,
}