using CertificateService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CertificateService.Infrastructure.Data;

public class CertificateDomainDbContext : DbContext
{
    public CertificateDomainDbContext(DbContextOptions<CertificateDomainDbContext> options) : base(options) { }

    public DbSet<Certificate> Certificates => Set<Certificate>();
    public DbSet<CertificateServiceEntity> CertificateServices => Set<CertificateServiceEntity>();
    public DbSet<CertificateSite> CertificateSites => Set<CertificateSite>();
    public DbSet<CertificateAdditionalScope> CertificateAdditionalScopes => Set<CertificateAdditionalScope>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Certificate>(e =>
        {
            e.ToTable("Certificates"); e.HasKey(x => x.CertificateId);
            e.Ignore(x => x.DomainEvents);
            e.HasMany(x => x.CertificateServices).WithOne(x => x.Certificate).HasForeignKey(x => x.CertificateId);
            e.HasMany(x => x.CertificateSites).WithOne(x => x.Certificate).HasForeignKey(x => x.CertificateId);
            e.HasMany(x => x.AdditionalScopes).WithOne(x => x.Certificate).HasForeignKey(x => x.CertificateId);
        });
        modelBuilder.Entity<CertificateServiceEntity>(e => { e.ToTable("CertificateServices"); e.HasKey(x => x.CertificateServiceId); });
        modelBuilder.Entity<CertificateSite>(e => { e.ToTable("CertificateSites"); e.HasKey(x => x.CertificateSiteId); });
        modelBuilder.Entity<CertificateAdditionalScope>(e => { e.ToTable("CertificateAdditionalScopes"); e.HasKey(x => x.CertificateAdditionalScopeId); });
    }
}
