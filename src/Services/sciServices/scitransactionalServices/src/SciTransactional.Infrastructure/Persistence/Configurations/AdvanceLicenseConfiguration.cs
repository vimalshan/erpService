using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SciTransactional.Domain.Entities;

namespace SciTransactional.Infrastructure.Persistence.Configurations;

public sealed class AdvanceLicenseConfiguration : IEntityTypeConfiguration<AdvanceLicenseEntity>
{
    public void Configure(EntityTypeBuilder<AdvanceLicenseEntity> builder)
    {
        builder.ToTable("ADVLIC_MASTER");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("ADVLIC_ID").ValueGeneratedNever();
        builder.Property(e => e.LicenseNo).HasColumnName("ADVLIC_NO").HasMaxLength(40);
        builder.Property(e => e.FgCode).HasColumnName("ADVLIC_FG");
        builder.Property(e => e.ExportObligationAmount).HasColumnName("ADVLIC_EOAMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.ExportAmount).HasColumnName("ADVLIC_EXPAMT").HasColumnType("decimal(19,0)");

        builder.Ignore(e => e.DomainEvents);
        builder.Ignore(e => e.Entitlements);

        builder.HasData(
            new { Id = 1L, LicenseNo = "ADVL-2026-001", FgCode = (int?)10,
                ExportObligationAmount = (decimal?)500000m, ExportAmount = (decimal?)250000m },
            new { Id = 2L, LicenseNo = "ADVL-2026-002", FgCode = (int?)20,
                ExportObligationAmount = (decimal?)1000000m, ExportAmount = (decimal?)750000m },
            new { Id = 3L, LicenseNo = "ADVL-2026-003", FgCode = (int?)30,
                ExportObligationAmount = (decimal?)300000m, ExportAmount = (decimal?)null }
        );
    }
}

public sealed class AdvanceLicenseEntitlementConfiguration : IEntityTypeConfiguration<AdvanceLicenseEntitlementEntity>
{
    public void Configure(EntityTypeBuilder<AdvanceLicenseEntitlementEntity> builder)
    {
        builder.ToTable("ADVLIC_ENTITLEMENT");
        builder.HasKey(e => new { e.Id, e.EntitlementRm });

        builder.Property(e => e.Id).HasColumnName("ADVLIC_ID");
        builder.Property(e => e.EntitlementRm).HasColumnName("ADVLIC_ENTITLERM");

        builder.Ignore(e => e.DomainEvents);

        builder.HasData(
            new { Id = 1L, EntitlementRm = 100 },
            new { Id = 1L, EntitlementRm = 200 },
            new { Id = 2L, EntitlementRm = 100 },
            new { Id = 2L, EntitlementRm = 300 },
            new { Id = 3L, EntitlementRm = 150 }
        );
    }
}
