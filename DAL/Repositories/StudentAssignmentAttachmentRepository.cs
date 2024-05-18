using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class StudentAssignmentAttachmentRepository(DbSet<StudentAssignmentAttachment> entities)
    : GenericRepository<StudentAssignmentAttachment, int>(entities)
{
}