using ExpenseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseService.Infrastructure.Data.Configurations;

public class ExpSettlementConfiguration : IEntityTypeConfiguration<ExpSettlement>
{
    public void Configure(EntityTypeBuilder<ExpSettlement> builder)
    {
        builder.ToTable("EXP_SETTLEMENT");
        builder.HasNoKey();

        builder.Property(e => e.ExpenseCode).HasColumnName("EXP_COD");
        builder.Property(e => e.ExpenseName).HasColumnName("EXP_NAM").HasMaxLength(100);
        builder.Property(e => e.BudgetAmount).HasColumnName("EXP_BUD").HasColumnType("decimal(38)");
        builder.Property(e => e.CompanyAmount).HasColumnName("EXP_CMP").HasColumnType("decimal(38)");
        builder.Property(e => e.SelfAmount).HasColumnName("EXP_SLF").HasColumnType("decimal(38)");
        builder.Property(e => e.AnnexureAmount).HasColumnName("EXP_ANX").HasColumnType("decimal(38)");
        builder.Property(e => e.Remarks).HasColumnName("EXP_REM").HasMaxLength(200);
        builder.Property(e => e.Remarks1).HasColumnName("EXP_REM1").HasMaxLength(200);

        builder.Ignore(e => e.DomainEvents);
    }
}
