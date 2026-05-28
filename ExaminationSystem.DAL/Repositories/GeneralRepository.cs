using ExaminationSystem.DAL.Context;
using ExaminationSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace ExaminationSystem.DAL.Repositories;

public class GeneralRepository<T> : IGeneralRepository<T> where T : BaseModel
{
    protected readonly ExaminationSystemDbContext _context;
    protected DbSet<T> _dbSet;
    public GeneralRepository(ExaminationSystemDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<bool> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return await _context.SaveChangesAsync() > 0;
    }
    public async Task<bool> AddRangeAsync(IList<T> entities)
    {
        _dbSet.AddRange(entities);
        return  await _context.SaveChangesAsync() > 0;
    }
    public async Task<bool> RemoveRangeAsync(IList<T> entities)
    {
        _dbSet.RemoveRange(entities);
        return await _context.SaveChangesAsync() > 0;
    }

    public IQueryable<T> GetQueryable() =>
           _dbSet.Where(e => !e.IsDeleted);

    public IQueryable<T> Get(Expression<Func<T,bool>> predicate) => 
        GetQueryable().Where(predicate);

    public IQueryable<T> GetById(int id) => 
           _dbSet.Where(m => m.ID == id && !m.IsDeleted);

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

    public async Task<bool> SoftDeleteAsync(T entity)
    {
        entity.IsDeleted = true;
        var result = await UpdateIncludeAsync(entity,nameof(entity.IsDeleted));
        return result;
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> criteria) =>
           await _dbSet.AnyAsync(criteria);

    public async Task<bool> UpdateAsync(T question) 
    {
        _dbSet.Update(question);

        return await _context.SaveChangesAsync() > 0;
    }

}
