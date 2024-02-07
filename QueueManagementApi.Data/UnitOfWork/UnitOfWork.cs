using QueueManagementApi.Core.Interfaces;
using QueueManagementApi.Infrastructure.Repositories;
using System.Collections;
using QueueManagementApi.Infrastructure.Data;
using QueueManagementApi.Core;

namespace QueueManagementApi.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly QueueManagementDbContext _context;
    private Hashtable? _repositories;

    public UnitOfWork(QueueManagementDbContext context)
    {
        _context = context;
    }

    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        _repositories ??= new();

        var type = typeof(T).Name;
        

        if (_repositories.ContainsKey(type)) return (IRepository<T>)_repositories[type]!;

        var repositoryType = typeof(GenericRepository<>);
        var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
        _repositories.Add(type, repositoryInstance);

        return (IRepository<T>)_repositories[type]!;
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
