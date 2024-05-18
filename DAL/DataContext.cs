using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }

    public DbSet<TeacherCourse> TeacherCourses { get; set; }

    public DbSet<StudentCourse> StudentCourses { get; set; }
    //public DbSet<UserRole> UserRoles { get; set; }
}