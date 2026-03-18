using Microsoft.EntityFrameworkCore;
using Stationery.Domain.Interfaces;
using Stationery.Domain.Common;
using Dapper;
using System.Linq.Expressions;
using Stationery.Infrastructure.Persistence;
using MediatR;

namespace Stationery.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly StationeryDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(StationeryDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(long id) => await _dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) 
        => await _dbSet.Where(predicate).ToListAsync();

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

    public void Update(T entity) => _dbSet.Attach(entity).State = EntityState.Modified;

    public void Remove(T entity) => _dbSet.Remove(entity);
}

public class UnitOfWork : IUnitOfWork
{
    private readonly StationeryDbContext _context;
    private readonly IMediator _mediator;
    private readonly Dictionary<string, object> _repositories = new();

    public UnitOfWork(StationeryDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public IRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T).Name;
        if (!_repositories.ContainsKey(type))
        {
            var repositoryInstance = new Repository<T>(_context);
            _repositories.Add(type, repositoryInstance);
        }
        return (IRepository<T>)_repositories[type];
    }

    public async Task<int> CompleteAsync()
    {
        // Dispatch domain events before saving
        await DispatchDomainEventsAsync();
        return await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
    {
        var connection = _context.Database.GetDbConnection();
        return await connection.QueryAsync<T>(sql, param);
    }

    private async Task DispatchDomainEventsAsync()
    {
        var domainEntities = _context.ChangeTracker
            .Entries<IHasDomainEvents>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent);
    }

    public void Dispose() => _context.Dispose();
}
