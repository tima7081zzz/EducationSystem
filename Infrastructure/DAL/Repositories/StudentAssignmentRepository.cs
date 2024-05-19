using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class StudentAssignmentRepository(DbSet<StudentAssignment> entities) : GenericRepository<StudentAssignment, int>(entities)
{
    public async Task<StudentAssignment?> Get(int userId, int assignmentId, CancellationToken ct)
    {
        return await Entities
            .FirstOrDefaultAsync(x => x.UserId == userId && x.AssignmentId == assignmentId, ct);
    }
}