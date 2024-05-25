﻿using DAL.Entities;
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
}