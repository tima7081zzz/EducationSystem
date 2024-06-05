using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class RecommendedCourseRepository(DbSet<RecommendedCourse> entities)
    : GenericRepository<RecommendedCourse, int>(entities)
{
    public async Task Add(IEnumerable<RecommendedCourse> entities, CancellationToken ct)
    {
        await Entities.AddRangeAsync(entities, ct);
    }

    public async Task<List<RecommendedCourse>> GetForUser(int userId, int take, CancellationToken ct)
    {
        return await Entities
            .Where(x => x.UserId == userId)
            .Include(x => x.Course)
            .Include(x => x.Course.CreatorUser)
            .OrderByDescending(x => x.TopsisScore)
            .Take(take)
            .ToListAsync(ct);
    }
};