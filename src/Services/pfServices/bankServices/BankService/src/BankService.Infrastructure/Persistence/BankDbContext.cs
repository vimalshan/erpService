using BankService.Domain.Common;
using BankService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankService.Infrastructure.Persistence;

public class BankDbContext(DbContextOptions<BankDbContext> options, IMediator mediator) : DbContext(options)
{
    public DbSet<BankMaster> BankMasters => Set<BankMaster>();
    public DbSet<ChequeDetail> ChequeDetails => Set<ChequeDetail>();
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    public DbSet<ChequeRegister> ChequeRegisters => Set<ChequeRegister>();
    public DbSet<PaymentReconciliation> PaymentReconciliations => Set<PaymentReconciliation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BankDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<BaseEntity>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            entry.Entity.ClearDomainEvents();
        }

        return result;
    }
}
