using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class StudentCourseRepository(DbSet<StudentCourse> entities) : GenericRepository<StudentCourse, int>(entities)
{
    public async Task<List<StudentCourse>> GetForUser(int userId, CancellationToken ct)
    {
        return await Entities
            .Where(x => x.UserId == userId)
            .Include(x => x.Course)
            .Include(x => x.Course.CreatorUser)
            .ToListAsync(ct);
    }

    public async Task<StudentCourse?> Get(int userId, int courseId, CancellationToken ct)
    {
        return await Entities
            .FirstOrDefaultAsync(x => x.UserId == userId && x.CourseId == courseId, cancellationToken: ct);
    }

    public async Task<List<StudentCourse>> GetByCourse(int courseId, CancellationToken ct)
    {
        return await Entities
            .Include(x => x.User)
            .Where(x => x.CourseId == courseId)
            .ToListAsync(ct);
    }

    public async Task<List<User>> GetUsersForNotification(int courseId, Func<UserNotificationSettings, bool> notificationAction, CancellationToken ct)
    {
        return await Entities
            .Include(x => x.User)
            .Where(x => x.User.NotificationSettings.IsEnabled && notificationAction(x.User.NotificationSettings))
            .Where(x => x.CourseId == courseId)
            .Where(x => x.IsNotificationsEnabled)
            .Select(x => x.User)
            .ToListAsync(ct);
    }
}