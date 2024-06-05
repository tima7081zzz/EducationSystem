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
    public DbSet<RecommendedCourse> RecommendedCourses { get; set; }
    public DbSet<UserNotificationSettings> UserNotificationSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(u =>
        {
            u.HasMany(x => x.Courses).WithOne(y => y.CreatorUser).OnDelete(DeleteBehavior.NoAction);
            u.HasMany(x => x.StudentCourses).WithOne(y => y.User).OnDelete(DeleteBehavior.NoAction);
            u.HasMany(x => x.TeacherCourses).WithOne(y => y.User).OnDelete(DeleteBehavior.NoAction);
            u.HasMany(x => x.Assignments).WithOne(y => y.CreatorTeacher).OnDelete(DeleteBehavior.NoAction);
            u.HasMany(x => x.StudentAssignmentAttachments).WithOne(y => y.StudentUser).OnDelete(DeleteBehavior.NoAction);
            u.HasMany(x => x.RecommendedCourses).WithOne(y => y.User).OnDelete(DeleteBehavior.NoAction);
            u.HasOne(x => x.NotificationSettings).WithOne(y => y.User).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<UserNotificationSettings>(e =>
        {
            e.Property(x => x.IsEnabled).HasDefaultValue(true);
            e.Property(x => x.NewAssignmentEnabled).HasDefaultValue(true);
            e.Property(x => x.DeadlineReminderEnabled).HasDefaultValue(true);
            e.Property(x => x.GradingAssignmentEnabled).HasDefaultValue(true);
            e.Property(x => x.AssignmentSubmittedEnabled).HasDefaultValue(true);
        });

        modelBuilder.Entity<StudentCourse>(e =>
        {
            e.Property(x => x.IsNotificationsEnabled).HasDefaultValue(true);
        });
        
        modelBuilder.Entity<TeacherCourse>(e =>
        {
            e.Property(x => x.IsNotificationsEnabled).HasDefaultValue(true);
        });
    }
}