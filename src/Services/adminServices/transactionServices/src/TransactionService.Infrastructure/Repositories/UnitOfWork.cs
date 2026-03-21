namespace TransactionService.Infrastructure.Repositories;

using MediatR;
using TransactionService.Domain.Common;
using TransactionService.Domain.Interfaces;
using TransactionService.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TransactionDbContext _context;
    private readonly IMediator _mediator;
    private readonly Dictionary<string, object> _repositories = new();
    private bool _disposed;

    public UnitOfWork(TransactionDbContext context, IMediator mediator)
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

    public async Task<int> CompleteAsync(CancellationToken ct = default)
    {
        await DispatchDomainEventsAsync(ct);
        return await _context.SaveChangesAsync(ct);
    }

    private async Task DispatchDomainEventsAsync(CancellationToken ct)
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
            await _mediator.Publish(domainEvent, ct);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _context.Dispose();
            _disposed = true;
        }
    }
}
