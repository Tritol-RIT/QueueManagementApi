using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using QueueManagementApi.Core;
using QueueManagementApi.Core.Interfaces;

namespace QueueManagementApi.Infrastructure.Repositories;

public class GenericRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly DbContext _context;

    public GenericRepository(DbContext context)
    {
        _context = context;
    }

    public IQueryable<T> GetAll()
    {
        return _context.Set<T>();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression);
    }

    public async Task<T?> FindById(int id)
    {
        return await _context.FindAsync<T>(id);
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public async Task BulkInsertAsync(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
    }

    public void BulkUpdateAsync(IEnumerable<T> entities)
    {
        _context.Set<T>().UpdateRange(entities);
    }
}