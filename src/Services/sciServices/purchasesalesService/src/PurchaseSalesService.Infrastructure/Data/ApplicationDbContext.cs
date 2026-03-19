using Microsoft.EntityFrameworkCore;
using PurchaseSalesService.Application.Common.Interfaces;
using PurchaseSalesService.Domain.Common;
using PurchaseSalesService.Domain.Entities;

namespace PurchaseSalesService.Infrastructure.Data;

public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<PurchaseDetail> PurchaseDetails => Set<PurchaseDetail>();
    public DbSet<SaleMain> SaleMains => Set<SaleMain>();
    public DbSet<SaleSub> SaleSubs => Set<SaleSub>();
    public DbSet<LogPurchaseDetail> LogPurchaseDetails => Set<LogPurchaseDetail>();
    public DbSet<LogSaleMain> LogSaleMains => Set<LogSaleMain>();
    public DbSet<LogSaleSub> LogSaleSubs => Set<LogSaleSub>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var entities = ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = entities
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        entities.ForEach(e => e.Entity.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        // Note: In production, dispatch domain events via MediatR here
        return result;
    }
}
