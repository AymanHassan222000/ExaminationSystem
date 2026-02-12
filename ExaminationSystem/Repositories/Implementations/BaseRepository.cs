using ExaminationSystem.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace ExaminationSystem.Repositories.Implementations;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
{
    protected readonly ApplicationDbContext _context;
    protected DbSet<T> _dbSet;
    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public IQueryable<T> GetAll() =>
           _dbSet.Where(e => !e.IsDeleted);


    public IQueryable<T> GetById(int id) => 
           _dbSet.Where(m => m.ID == id && !m.IsDeleted);

    public IQueryable<T> Find(Expression<Func<T, bool>> criteria) => 
           _dbSet.Where(m => !m.IsDeleted).Where(criteria);

    public async Task<bool> UpdateIncludeAsync(T entity, params string[] modifiedParams)
    {
        var local = _dbSet.Local.FirstOrDefault(m => m.ID == entity.ID);

        EntityEntry entityEntry;

        if (local == null)
        {
            _dbSet.Attach(entity);
            entityEntry = _dbSet.Entry(entity);
        }
        else
        {
            entityEntry = _context.ChangeTracker.Entries<T>().First(m => m.Entity.ID == entity.ID);
        }

        foreach (var propName in modifiedParams)
        {
            var propInfo = entity.GetType().GetProperty(propName);
            if (propInfo != null)
            {
                entityEntry.Property(propName).CurrentValue = propInfo.GetValue(entity);
                entityEntry.Property(propName).IsModified = true;
            }
        }

        var result = await _context.SaveChangesAsync();
        return result > 0 ? true : false;
    }

    public async Task HardDeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<int> SoftDeleteAsync(int id, int instructorID)
    {
        return await _dbSet
                    .Where(e => e.ID == id)
                    .ExecuteUpdateAsync(s => s
                         .SetProperty(d => d.IsDeleted, true)
                         .SetProperty(d => d.UpdatedAt, DateTime.UtcNow)
                         .SetProperty(d => d.UpdatedBy, instructorID)
                    );
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> criteria) =>
           await _dbSet.AnyAsync(criteria);

}
