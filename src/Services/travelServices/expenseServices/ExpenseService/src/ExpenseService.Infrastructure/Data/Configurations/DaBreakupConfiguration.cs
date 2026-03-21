using ExpenseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseService.Infrastructure.Data.Configurations;

public class DaBreakupConfiguration : IEntityTypeConfiguration<DaBreakup>
{
    public void Configure(EntityTypeBuilder<DaBreakup> builder)
    {
        builder.ToTable("DA_BREAKUP");
        builder.HasKey(e => e.SerialNumber);

        builder.Property(e => e.RequestId).HasColumnName("DA_REQ_ID");
        builder.Property(e => e.SerialNumber).HasColumnName("DA_SRL_NUM").ValueGeneratedNever();
        builder.Property(e => e.FromDate).HasColumnName("DA_FRO_DAT");
        builder.Property(e => e.ToDate).HasColumnName("DA_TO_DAT");
        builder.Property(e => e.TypeCode).HasColumnName("DA_TYP_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.Hours).HasColumnName("DA_HRS").HasColumnType("decimal(19,0)");

        builder.Ignore(e => e.DomainEvents);
    }
}
