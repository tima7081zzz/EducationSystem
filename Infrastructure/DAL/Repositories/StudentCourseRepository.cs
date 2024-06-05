using System.Linq.Expressions;
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

    public async Task<List<User>> GetUsersForNotification(int courseId, Expression<Func<StudentCourse, bool>> notificationAction, CancellationToken ct, int? userId = null)
    {
        var query = Entities
            .Include(x => x.User)
            .Where(x => x.User.NotificationSettings.IsEnabled)
            .Where(notificationAction)
            .Where(x => x.CourseId == courseId)
            .Where(x => x.IsNotificationsEnabled);

        if (userId.HasValue)
        {
            query = query.Where(x => x.UserId == userId);
        }
        
        return await query
            .Select(x => x.User)
            .ToListAsync(ct);
    }

    public async Task DeleteByCourse(int courseId, CancellationToken ct)
    {
        await Entities
            .Where(x => x.CourseId == courseId)
            .ExecuteDeleteAsync(ct);
    }
    
    public async Task DeleteByUser(int userId, CancellationToken ct)
    {
        await Entities
            .Where(x => x.UserId == userId)
            .ExecuteDeleteAsync(ct);
    }

    public async Task<List<StudentCourse>> GetCommonUsers(int userId, int courseId, CancellationToken ct)
    {
        return await Entities
            .Where(x => x.CourseId == courseId)
            .Join(
                Entities.Where(sc => sc.UserId == userId),
                sc1 => sc1.CourseId,
                sc2 => sc2.CourseId,
                (sc1, sc2) => sc1
            )
            .Where(sc1 => sc1.UserId != userId)
            .Distinct()
            .ToListAsync(ct);
    }
}