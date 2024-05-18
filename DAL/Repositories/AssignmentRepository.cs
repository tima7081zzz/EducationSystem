using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class AssignmentRepository(DbSet<Assignment> entities) : GenericRepository<Assignment, int>(entities)
{
    public async Task<List<Assignment>> GetByCourse(int courseId, CancellationToken ct)
    {
        return await Entities
            .Where(x => x.CourseId == courseId)
            .ToListAsync(cancellationToken: ct);
    }
}