using Microsoft.EntityFrameworkCore;
using RequestServices.Domain.Entities;
using RequestServices.Domain.Interfaces;

namespace RequestServices.Infrastructure.Data;

public class RequestDbContext(DbContextOptions<RequestDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<RequestMain>   RequestMain   => Set<RequestMain>();
    public DbSet<RequestSub>    RequestSub    => Set<RequestSub>();
    public DbSet<RequestNew>    RequestNew    => Set<RequestNew>();
    public DbSet<RequestAction> RequestAction => Set<RequestAction>();
    public DbSet<RequestApp>    RequestApp    => Set<RequestApp>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RequestDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
        => base.SaveChangesAsync(ct);
}
