using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class UserRepository(DbSet<User> entities) : GenericRepository<User, int>(entities)
{
    public async Task<User?> Get(string email, CancellationToken ct)
    {
        return await Entities
            .Where(x => x.Email == email)
            .FirstOrDefaultAsync(ct);
    }
}