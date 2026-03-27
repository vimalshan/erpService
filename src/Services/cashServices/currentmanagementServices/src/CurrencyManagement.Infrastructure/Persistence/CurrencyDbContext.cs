using Microsoft.EntityFrameworkCore;
using MediatR;
using CurrencyManagement.Application.Common.Interfaces;
using CurrencyManagement.Domain.Entities;
using CurrencyManagement.Infrastructure.Persistence.Configurations;

namespace CurrencyManagement.Infrastructure.Persistence;

/// <summary>
/// EF Core DbContext for Currency Management
/// </summary>
public class CurrencyDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator? _mediator;

    public DbSet<Currency> Currencies { get; set; } = null!;
    public DbSet<ExchangeRate> ExchangeRates { get; set; } = null!;
    public DbSet<OrganizationCurrencyMapping> OrganizationCurrencyMappings { get; set; } = null!;

    public CurrencyDbContext(DbContextOptions<CurrencyDbContext> options, IMediator? mediator = null) : base(options)
    {
        _mediator = mediator;
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
        await DispatchDomainEventsAsync(cancellationToken);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
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

        if (_mediator != null)
        {
            foreach (var domainEvent in events)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }
        }
    }
}
