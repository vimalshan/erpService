using LoanService.Domain.Common;
using LoanService.Domain.Interfaces;
using MediatR;

namespace LoanService.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly LoanDbContext _context;
    private readonly IMediator _mediator;

    public ILoanRepository Loans { get; }

    public UnitOfWork(LoanDbContext context, ILoanRepository loans, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
        Loans = loans;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // Dispatch domain events before saving
        var entities = _context.ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, ct);

        return await _context.SaveChangesAsync(ct);
    }
}
