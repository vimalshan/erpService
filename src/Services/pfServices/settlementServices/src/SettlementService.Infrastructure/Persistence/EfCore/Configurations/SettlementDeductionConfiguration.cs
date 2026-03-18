using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SettlementService.Domain.Entities;

namespace SettlementService.Infrastructure.Persistence.EfCore.Configurations;

public class SettlementDeductionConfiguration : IEntityTypeConfiguration<SettlementDeduction>
{
    public void Configure(EntityTypeBuilder<SettlementDeduction> builder)
    {
        builder.ToTable("SET_DEDUCTION");
        builder.HasKey(e => e.SetDedId);

        builder.Property(e => e.SetDedId).HasColumnName("SET_DED_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.SetNum).HasColumnName("SET_NUM");
        builder.Property(e => e.DedType).HasColumnName("DED_TYPE").HasMaxLength(50);
        builder.Property(e => e.DedAmount).HasColumnName("DED_AMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON").HasPrecision(3);

        builder.Ignore(e => e.DomainEvents);
    }
}
