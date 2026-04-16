using ContractService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContractService.Infrastructure.Data;

public class ContractDomainDbContext : DbContext
{
    public ContractDomainDbContext(DbContextOptions<ContractDomainDbContext> options) : base(options) { }

    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<ContractServiceEntity> ContractServices => Set<ContractServiceEntity>();
    public DbSet<ContractSite> ContractSites => Set<ContractSite>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contract>(e =>
        {
            e.ToTable("Contracts"); e.HasKey(x => x.ContractId);
            e.Property(x => x.ContractNumber).HasMaxLength(100).IsRequired();
            e.Property(x => x.ContractName).HasMaxLength(200).IsRequired();
            e.Property(x => x.ContractType).HasMaxLength(100);
            e.Property(x => x.Status).HasMaxLength(50).HasDefaultValue("Active");
            e.Property(x => x.TotalValue).HasColumnType("decimal(12,2)");
            e.Property(x => x.Currency).HasMaxLength(3).HasDefaultValue("USD");
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.Property(x => x.CreatedDate).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.ModifiedDate).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.SignedByClient).HasMaxLength(100);
            e.Property(x => x.SignedByDNV).HasMaxLength(100);
            e.Property(x => x.ContractPath).HasMaxLength(500);
            e.Property(x => x.Notes).HasMaxLength(1000);
            e.Property(x => x.AutoRenewal).HasDefaultValue(false);
            e.HasIndex(x => x.ContractNumber).IsUnique();
            e.HasIndex(x => x.CompanyId); e.HasIndex(x => x.Status);
            e.HasIndex(x => x.StartDate); e.HasIndex(x => x.EndDate); e.HasIndex(x => x.IsActive);
            e.Ignore(x => x.DomainEvents);
            e.HasMany(x => x.ContractServices).WithOne(x => x.Contract).HasForeignKey(x => x.ContractId).OnDelete(DeleteBehavior.Cascade);
            e.HasMany(x => x.ContractSites).WithOne(x => x.Contract).HasForeignKey(x => x.ContractId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ContractServiceEntity>(e =>
        {
            e.ToTable("ContractServices"); e.HasKey(x => x.ContractServiceId);
            e.Property(x => x.Quantity).HasDefaultValue(1);
            e.Property(x => x.UnitPrice).HasColumnType("decimal(10,2)");
            e.Property(x => x.TotalPrice).HasColumnType("decimal(12,2)");
            e.Property(x => x.Currency).HasMaxLength(3).HasDefaultValue("USD");
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.Property(x => x.Status).HasMaxLength(50).HasDefaultValue("Active");
            e.Property(x => x.Notes).HasMaxLength(500);
            e.HasIndex(x => x.ContractId); e.HasIndex(x => x.ServiceId); e.HasIndex(x => x.IsActive);
            e.HasIndex(x => new { x.ContractId, x.ServiceId }).IsUnique();
        });

        modelBuilder.Entity<ContractSite>(e =>
        {
            e.ToTable("ContractSites"); e.HasKey(x => x.ContractSiteId);
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.Property(x => x.Status).HasMaxLength(50).HasDefaultValue("Active");
            e.Property(x => x.Notes).HasMaxLength(500);
            e.HasIndex(x => x.ContractId); e.HasIndex(x => x.SiteId); e.HasIndex(x => x.IsActive);
            e.HasIndex(x => new { x.ContractId, x.SiteId }).IsUnique();
        });
    }
}
