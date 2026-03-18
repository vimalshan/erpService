using Microsoft.EntityFrameworkCore;
using CashManagement.Domain.Entities;
using MediatR;

namespace CashManagement.Infrastructure.Persistence;

public class CashManagementDbContext : DbContext
{
    private readonly IMediator _mediator;

    public CashManagementDbContext(DbContextOptions<CashManagementDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<CashUnit> CashUnits => Set<CashUnit>();
    public DbSet<CashTransaction> CashTransactions => Set<CashTransaction>();
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    public DbSet<BankTransaction> BankTransactions => Set<BankTransaction>();
    public DbSet<ChequeRegister> ChequeRegisters => Set<ChequeRegister>();
    public DbSet<ChequeRegisterAudit> ChequeRegisterAudits => Set<ChequeRegisterAudit>();
    public DbSet<BankReconciliation> BankReconciliations => Set<BankReconciliation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CashManagementDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        await DispatchDomainEventsAsync(cancellationToken);
        return result;
    }

    private async Task DispatchDomainEventsAsync(CancellationToken ct)
    {
        var entities = ChangeTracker.Entries<Domain.Common.BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var events = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in events)
            await _mediator.Publish(domainEvent, ct);
    }
}
