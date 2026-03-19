using FilingAndArchiveService.Application.Common.Interfaces;
using FilingAndArchiveService.Domain.Entities;
using FilingAndArchiveService.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FilingAndArchiveService.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<FileMaster> FileMasters => Set<FileMaster>();
    public DbSet<FilingCounter> FilingCounters => Set<FilingCounter>();
    public DbSet<FilingDocPrint> FilingDocPrints => Set<FilingDocPrint>();
    public DbSet<FilingDocErrorList> FilingDocErrorLists => Set<FilingDocErrorList>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
