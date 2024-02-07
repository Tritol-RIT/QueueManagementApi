using System.Linq.Expressions;

namespace QueueManagementApi.Core.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    IQueryable<T> GetAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    Task<T?> FindById(int id);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task BulkInsertAsync(IEnumerable<T> entities);
    void BulkUpdateAsync(IEnumerable<T> entities);
}