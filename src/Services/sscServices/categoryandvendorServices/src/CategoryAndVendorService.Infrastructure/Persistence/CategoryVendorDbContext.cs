using CategoryAndVendorService.Domain.Common;
using CategoryAndVendorService.Domain.Entities;
using CategoryAndVendorService.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CategoryAndVendorService.Infrastructure.Persistence;

public class CategoryVendorDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public DbSet<MainCategory> MainCategories => Set<MainCategory>();
    public DbSet<SubCategory> SubCategories => Set<SubCategory>();
    public DbSet<VendorDocument> VendorDocuments => Set<VendorDocument>();
    public DbSet<VendorDocumentFile> VendorDocumentFiles => Set<VendorDocumentFile>();
    public DbSet<SupportDocument> SupportDocuments => Set<SupportDocument>();
    public DbSet<SupportDocumentAttachment> SupportDocumentAttachments => Set<SupportDocumentAttachment>();
    public DbSet<SupportDocumentCounter> SupportDocumentCounters => Set<SupportDocumentCounter>();

    public CategoryVendorDbContext(DbContextOptions<CategoryVendorDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryVendorDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<Entity>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        foreach (var entry in ChangeTracker.Entries<Entity>())
        {
            entry.Entity.ClearDomainEvents();
        }

        return result;
    }
}
