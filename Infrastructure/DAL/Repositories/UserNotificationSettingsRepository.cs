using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class UserNotificationSettingsRepository(DbSet<UserNotificationSettings> entities)
    : GenericRepository<UserNotificationSettings, int>(entities)
{
    public async Task<UserNotificationSettings?> GetByUser(int userId, CancellationToken ct)
    {
        return await Entities
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(ct);
    }
}