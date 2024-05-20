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

    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<StudentAssignmentAttachment> StudentAssignmentAttachments { get; set; }
    public DbSet<StudentAssignment> StudentAssignments { get; set; }
    //public DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(x =>
        {
            x.HasMany(x => x.Courses).WithOne(x => x.CreatorUser).OnDelete(DeleteBehavior.NoAction);
            x.HasMany(x => x.StudentCourses).WithOne(x => x.User).OnDelete(DeleteBehavior.NoAction);
            x.HasMany(x => x.TeacherCourses).WithOne(x => x.User).OnDelete(DeleteBehavior.NoAction);
            x.HasMany(x => x.Assignments).WithOne(x => x.CreatorTeacher).OnDelete(DeleteBehavior.NoAction);
            x.HasMany(x => x.StudentAssignmentAttachments).WithOne(x => x.StudentUser).OnDelete(DeleteBehavior.NoAction);
        });
    }
}