using DeductionService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeductionService.Infrastructure.Persistence.Configurations;

public class DeductionAccessConfiguration : IEntityTypeConfiguration<DeductionAccess>
{
    public void Configure(EntityTypeBuilder<DeductionAccess> builder)
    {
        builder.ToTable("DEDUCTION_ACCESS");
        builder.HasNoKey();

        builder.Property(x => x.AccessNumber).HasColumnName("DE_UNT_ACC");
        builder.Property(x => x.UnitCode).HasColumnName("DE_COM_COD");
        builder.Property(x => x.DeductionType).HasColumnName("DE_DED_TYP").HasColumnType("CHAR(3)");
        builder.Property(x => x.SystemId).HasColumnName("DE_SYS_ID").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.EnteredByUserId).HasColumnName("DE_ENT_USR").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.EnteredOn).HasColumnName("DE_ENT_ON").HasPrecision(3);
        builder.Property(x => x.ClosedOn).HasColumnName("DE_CLS_DAT").HasPrecision(3);

        builder.Ignore(x => x.DomainEvents);
        builder.Ignore(x => x.IsActive);
    }
}
