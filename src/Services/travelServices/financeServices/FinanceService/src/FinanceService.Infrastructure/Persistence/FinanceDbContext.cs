using FinanceService.Application.Common.Interfaces;
using FinanceService.Domain.Common;
using FinanceService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Infrastructure.Persistence;

public class FinanceDbContext : DbContext, IFinanceDbContext
{
    private readonly IMediator _mediator;

    public FinanceDbContext(DbContextOptions<FinanceDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<ApInvoice> ApInvoices => Set<ApInvoice>();
    public DbSet<ApInvoiceLine> ApInvoiceLines => Set<ApInvoiceLine>();
    public DbSet<PaymentTerm> PaymentTerms => Set<PaymentTerm>();
    public DbSet<PaymentDetail> PaymentDetails => Set<PaymentDetail>();
    public DbSet<TravelBatchMain> TravelBatchMains => Set<TravelBatchMain>();
    public DbSet<TravelBatchSub> TravelBatchSubs => Set<TravelBatchSub>();
    public DbSet<JvPostingDetail> JvPostingDetails => Set<JvPostingDetail>();
    public DbSet<PayJv> PayJvs => Set<PayJv>();
    public DbSet<PayOtherDetail> PayOtherDetails => Set<PayOtherDetail>();
    public DbSet<TravelAccount> TravelAccounts => Set<TravelAccount>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinanceDbContext).Assembly);
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
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            entry.Entity.ClearDomainEvents();
        }

        return result;
    }
}
