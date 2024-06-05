using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class CourseRepository(DbSet<Course> entities) : GenericRepository<Course, int>(entities)
{
    public async Task<Course?> GetWithCourses(string publicId, CancellationToken ct)
    {
        return await Entities
            .Where(x => x.PublicId == publicId)
            .Include(x => x.TeacherCourses)
            .Include(x => x.StudentCourses)
            .FirstOrDefaultAsync(ct);
    }
    
    public async Task<Course?> GetWithCourses(int id, CancellationToken ct)
    {
        return await Entities
            .Include(x => x.TeacherCourses)
            .Include(x => x.StudentCourses)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<Course>> GetAll(CancellationToken ct)
    {
        return await Entities.ToListAsync(ct);
    }
}