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
    
    public async Task<StudentAssignment?> GetWithAssignmentAndAttachments(int userId, int assignmentId, CancellationToken ct)
    {
        return await Entities
            .Include(x => x.Assignment)
            .Include(x => x.StudentAssignmentAttachments)
            .FirstOrDefaultAsync(x => x.UserId == userId && x.AssignmentId == assignmentId, ct);
    }
    
    public async Task<StudentAssignment?> GetWithAttachments(int userId, int assignmentId, CancellationToken ct)
    {
        return await Entities
            .Include(x => x.StudentAssignmentAttachments)
            .FirstOrDefaultAsync(x => x.UserId == userId && x.AssignmentId == assignmentId, ct);
    }

    public async Task<List<StudentAssignment>> GetByAssignment(int assignmentId, CancellationToken ct)
    {
        return await Entities
            .Where(x => x.AssignmentId == assignmentId)
            .ToListAsync(ct);
    }

    public async Task DeleteByCourse(int courseId, CancellationToken ct)
    {
        await Entities
            .Where(x => x.Assignment.CourseId == courseId)
            .ExecuteDeleteAsync(ct);
    }
    
    public async Task DeleteByUser(int userId, CancellationToken ct)
    {
        await Entities
            .Where(x => x.UserId == userId)
            .ExecuteDeleteAsync(ct);
    }
}