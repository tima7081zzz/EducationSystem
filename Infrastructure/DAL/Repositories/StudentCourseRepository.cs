using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class StudentCourseRepository(DbSet<StudentCourse> entities) : GenericRepository<StudentCourse, int>(entities)
{
    public async Task<List<UserCourseDto>> GetForUser(int userId, CancellationToken ct)
    {
        return await Entities
            .Where(x => x.UserId == userId)
            .Select(x => new UserCourseDto
            {
                Id = x.CourseId,
                Name = x.Course.Name,
                Description = x.Course.Description,
                Category = x.Course.Category,
                CreatorUserFullName = x.Course.CreatorUser.Fullname,
                CreatorUserId = x.Course.CreatorUserId,
                CreatedAt = x.Course.CreatedAt,
            })
            .ToListAsync(ct);
    }

    public async Task<StudentCourse?> Get(int userId, int courseId, CancellationToken ct)
    {
        return await Entities
            .FirstOrDefaultAsync(x => x.UserId == userId && x.CourseId == courseId, cancellationToken: ct);
    }
}