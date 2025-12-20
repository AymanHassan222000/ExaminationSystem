using ExaminationSystem.Data;
using ExaminationSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

    public async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public IQueryable<T> GetAll()
    {
        var entities = _dbSet.Where(e => !e.IsDeleted);
        return entities;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.ID == id && !e.IsDeleted);
    }

    //public async Task<>

    #region Get All
    //public IEnumerable<T> Get(
    //    Expression<Func<T, bool>>? filterExpression = null,
    //    bool OrderDescending = false,
    //    Expression<Func<T, object>>? orderBy = null
    //)
    //{
    //    IQueryable<T> query = GetAll();
    //    if (filterExpression != null)
    //    {
    //        query = query.Where(filterExpression);
    //    }

    //    if (orderBy != null)
    //    {
    //        query = query.OrderBy(orderBy);
    //    }

    //    if (OrderDescending)
    //    {
    //        query = query.OrderDescending();
    //    }

    //    return query.ToList();
    //}

    #endregion

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

    public async Task<int> SoftDeleteAsync(int id)
    {
        return await _dbSet
                    .Where(e => e.ID == id)
                    .ExecuteUpdateAsync(s =>s
                         .SetProperty(x => x.IsDeleted, true)
                         .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                    );
    }

    public async Task<bool> AnyAsync(Expression<Func<T,bool>> criteria) 
    {
        return await _dbSet.AnyAsync(criteria);
    }

    public async Task SaveChangesAsync() 
    {
        await _context.SaveChangesAsync();
    }
}
