using Microsoft.EntityFrameworkCore;
using CurrencyManagement.Application.Common.Interfaces;
using CurrencyManagement.Domain.Entities;
using CurrencyManagement.Infrastructure.Persistence.Configurations;

namespace CurrencyManagement.Infrastructure.Persistence;

/// <summary>
/// EF Core DbContext for Currency Management
/// </summary>
public class CurrencyDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Currency> Currencies { get; set; } = null!;
    public DbSet<ExchangeRate> ExchangeRates { get; set; } = null!;
    public DbSet<OrganizationCurrencyMapping> OrganizationCurrencyMappings { get; set; } = null!;

    public CurrencyDbContext(DbContextOptions<CurrencyDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Apply configurations
        modelBuilder.ApplyConfiguration(new CurrencyConfiguration());
        modelBuilder.ApplyConfiguration(new ExchangeRateConfiguration());
        modelBuilder.ApplyConfiguration(new OrganizationCurrencyMappingConfiguration());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        await DispatchDomainEventsAsync();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEventsAsync()
    {
        var entities = ChangeTracker
            .Entries<CurrencyManagement.Domain.Common.BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var events = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entities.ForEach(e => e.ClearDomainEvents());

        // In a complete implementation, dispatch these events to MediatR or event bus
        // For now, we're just clearing them to prevent re-processing
        await Task.CompletedTask;
    }
}
