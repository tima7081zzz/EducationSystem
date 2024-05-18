using Microsoft.EntityFrameworkCore;

namespace DAL;

public class GenericRepository<TEntitiy, TEntityKey>(DbSet<TEntitiy> entities)
    where TEntitiy : class
{
    protected readonly DbSet<TEntitiy> Entities = entities;

    public virtual async ValueTask<TEntitiy?> Get(TEntityKey id, CancellationToken ct = default)
    {
        var key = new object?[] {id};
        
        return await Entities.FindAsync(key, ct);
    }

    public virtual TEntitiy Add(TEntitiy entity)
    {
        return Entities.Add(entity).Entity;
    }
}