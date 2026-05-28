using System.Linq.Expressions;

namespace ExaminationSystem.DAL.Repositories;

public interface IGeneralRepository<T> where T : class
{
    Task<bool> AddAsync(T entity);
    Task<bool> AddRangeAsync(IList<T> entities);
    Task<bool> RemoveRangeAsync(IList<T> entities);
    IQueryable<T> GetQueryable();
    IQueryable<T> Get(Expression<Func<T, bool>> predicate);
    IQueryable<T> GetById(int id);
    Task<bool> UpdateIncludeAsync(T entity, params string[] modifiedParams);
    Task HardDeleteAsync(T entity);
    Task<bool> SoftDeleteAsync(T entity);
    Task<bool> AnyAsync(Expression<Func<T, bool>> criteria);
    Task<bool> UpdateAsync(T entity);
}
