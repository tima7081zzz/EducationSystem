using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class TeacherCourseRepository(DbSet<TeacherCourse> entities) : GenericRepository<TeacherCourse, int>(entities)
{
    public async Task<List<TeacherCourse>> GetForUser(int userId, CancellationToken ct)
    {
        return await Entities
            .Where(x => x.UserId == userId)
            .Include(x => x.Course)
            .Include(x => x.Course.CreatorUser)
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