using Microsoft.EntityFrameworkCore;
using CardManagement.Application.Common.Interfaces;
using CardManagement.Domain.Entities;
using CardManagement.Infrastructure.Persistence.Configurations;

namespace CardManagement.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<GuestCardMaster> GuestCardMasters => Set<GuestCardMaster>();
    public DbSet<CanteenCardMap> CanteenCardMaps => Set<CanteenCardMap>();
    public DbSet<CardSettlement> CardSettlements => Set<CardSettlement>();
    public DbSet<GuestCardMasterHistory> GuestCardMasterHistories => Set<GuestCardMasterHistory>();

    IQueryable<GuestCardMaster> IApplicationDbContext.GuestCardMasters => GuestCardMasters;
    IQueryable<CanteenCardMap> IApplicationDbContext.CanteenCardMaps => CanteenCardMaps;
    IQueryable<CardSettlement> IApplicationDbContext.CardSettlements => CardSettlements;
    IQueryable<GuestCardMasterHistory> IApplicationDbContext.GuestCardMasterHistories => GuestCardMasterHistories;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GuestCardMasterConfiguration());
        modelBuilder.ApplyConfiguration(new CanteenCardMapConfiguration());
        modelBuilder.ApplyConfiguration(new CardSettlementConfiguration());
        modelBuilder.ApplyConfiguration(new GuestCardMasterHistoryConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
