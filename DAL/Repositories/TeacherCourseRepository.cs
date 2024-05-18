using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class TeacherCourseRepository(DbSet<TeacherCourse> entities) : GenericRepository<TeacherCourse, int>(entities)
{
    public async Task<List<UserCourseDto>> GetForUser(int userId, CancellationToken ct)
    {
        return await Entities
            .Where(x => x.TeacherUserId == userId)
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
}

public class UserCourseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public required string CreatorUserFullName { get; set; }
    public int CreatorUserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}