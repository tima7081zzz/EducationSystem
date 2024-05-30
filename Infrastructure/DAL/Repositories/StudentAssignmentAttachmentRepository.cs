using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class StudentAssignmentAttachmentRepository(DbSet<StudentAssignmentAttachment> entities)
    : GenericRepository<StudentAssignmentAttachment, int>(entities)
{
    public async Task Delete(int id, CancellationToken ct)
    {
        await Entities
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(ct);
    }

    public async Task DeleteByCourse(int courseId, CancellationToken ct)
    {
        await Entities
            .Where(x => x.StudentAssignment.Assignment.CourseId == courseId)
            .ExecuteDeleteAsync(ct);
    }
    
    public async Task DeleteByUser(int userId, CancellationToken ct)
    {
        await Entities
            .Where(x => x.StudentUserId == userId)
            .ExecuteDeleteAsync(ct);
    }

    public async Task<StudentAssignmentAttachment?> GetWithAssignment(int id, CancellationToken ct)
    {
        return await Entities
            .Where(x => x.Id == id)
            .Include(x => x.StudentAssignment)
            .FirstOrDefaultAsync(ct);
    }
}