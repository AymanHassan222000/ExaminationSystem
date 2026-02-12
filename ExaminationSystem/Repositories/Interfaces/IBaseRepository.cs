using System.Linq.Expressions;

namespace ExaminationSystem.Repositories.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    IQueryable<T> GetAll();
    IQueryable<T> GetById(int id);
    IQueryable<T> Find(Expression<Func<T, bool>> criteria);
    Task<bool> UpdateIncludeAsync(T entity, params string[] modifiedParams);
    Task HardDeleteAsync(T entity);
    Task<int> SoftDeleteAsync(int id, int instructorID);
    Task<bool> AnyAsync(Expression<Func<T, bool>> criteria);
}
