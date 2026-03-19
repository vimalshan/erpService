using HRDocumentService.Domain.Common;
using HRDocumentService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRDocumentService.Infrastructure.Persistence;

public class HRDocumentDbContext(
    DbContextOptions<HRDocumentDbContext> options,
    IMediator mediator)
    : DbContext(options)
{
    public DbSet<HRDocument> HRDocuments => Set<HRDocument>();
    public DbSet<HRDocumentFile> HRDocumentFiles => Set<HRDocumentFile>();
    public DbSet<HRDocumentReceipt> HRDocumentReceipts => Set<HRDocumentReceipt>();
    public DbSet<HRDocumentCounter> HRDocumentCounters => Set<HRDocumentCounter>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HRDocumentDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var domainEvents = ChangeTracker.Entries<BaseEntity>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(ct);

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, ct);
        }

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            entry.Entity.ClearDomainEvents();
        }

        return result;
    }
}
