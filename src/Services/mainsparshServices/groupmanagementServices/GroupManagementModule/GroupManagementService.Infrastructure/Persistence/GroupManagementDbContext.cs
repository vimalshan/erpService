using Microsoft.EntityFrameworkCore;
using GroupManagementService.Domain.Entities;
using GroupManagementService.Infrastructure.Persistence.Configurations;

namespace GroupManagementService.Infrastructure.Persistence
{
    public class GroupManagementDbContext : DbContext
    {
        public GroupManagementDbContext(DbContextOptions<GroupManagementDbContext> options) : base(options)
        {
        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMenuMap> GroupMenuMaps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new GroupMenuMapConfiguration());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
