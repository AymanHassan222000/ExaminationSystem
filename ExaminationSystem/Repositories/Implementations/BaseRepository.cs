using ExaminationSystem.Data;
using ExaminationSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ExaminationSystem.Repositories.Implementations;

public class BaseRepository<T> where T : BaseModel
{
    protected readonly Context _context;
    protected DbSet<T> _dbSet;
    public BaseRepository()
    {
        _context = new Context();
        _dbSet = _context.Set<T>();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public IQueryable<T> GetAll()
    {
        var query = _dbSet.Where(e => !e.IsDeleted);

        return query;
    }

    public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;

        if (includes != null && includes.Any())
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.FirstOrDefaultAsync(e => e.ID == id && !e.IsDeleted);
    }

    public IQueryable<T> QueryById(int id)
    {
        var query = _dbSet.Where(m => m.ID == id);

        return query;
    }

    public async Task<T?> FindAsync(Expression<Func<T, bool>> criteria,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;

        if (includes != null && includes.Any())
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.FirstOrDefaultAsync(criteria);
    }

    public async Task<T?> FindAsync(
        Expression<Func<T, bool>> criteria,
        Func<IQueryable<T>,IQueryable<T>>? includes = null) 
    {
        IQueryable<T> query = _dbSet;

        if (includes != null)
            query = includes(query);

        var result = await query.FirstOrDefaultAsync(criteria);

        return result;
    }


    public async Task<int> UpdateAsync(
        Expression<Func<T, bool>> predicate,
        Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls)
    {
        var numUpdated = await _dbSet.Where(predicate).ExecuteUpdateAsync(setPropertyCalls);

        return numUpdated;
    }

    public async Task DeleteAsync(T entity)
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

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> criteria)
    {
        return await _dbSet.AnyAsync(criteria);
    }

}
