using ComplaintService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComplaintService.Infrastructure.Persistence;

public class ComplaintDbContext(DbContextOptions<ComplaintDbContext> options) : DbContext(options)
{
    public DbSet<ComplaintGroup> ComplaintGroups => Set<ComplaintGroup>();
    public DbSet<ComplaintTicket> ComplaintTickets => Set<ComplaintTicket>();
    public DbSet<ComplaintAction> ComplaintActions => Set<ComplaintAction>();
    public DbSet<ComplaintEscalation> ComplaintEscalations => Set<ComplaintEscalation>();
    public DbSet<ComplaintHistory> ComplaintHistories => Set<ComplaintHistory>();
    public DbSet<ComplaintTask> ComplaintTasks => Set<ComplaintTask>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ComplaintDbContext).Assembly);
    }
}
