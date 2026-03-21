namespace TransactionService.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;

public sealed class LocationAdminConfiguration : IEntityTypeConfiguration<LocationAdmin>
{
    public void Configure(EntityTypeBuilder<LocationAdmin> builder)
    {
        builder.ToTable("SP_LOCATION_ADMIN");
        builder.HasKey(a => new { a.LocationId, a.EmpSysId });
        builder.Property(a => a.LocationId).HasColumnName("LA_LOCATION_ID");
        builder.Property(a => a.EmpSysId).HasColumnName("LA_EMP_SYSID");
        builder.Property(a => a.EffectiveDate).HasColumnName("LA_EFFECTIVE_DATE");
        builder.Property(a => a.ClosureDate).HasColumnName("LA_CLOSURE_DATE");
        builder.Property(a => a.UpdatedBy).HasColumnName("LA_UPDATED_BY");
        builder.Property(a => a.UpdatedOn).HasColumnName("LA_UPDATED_ON");

        builder.Ignore(a => a.DomainEvents);
    }
}
