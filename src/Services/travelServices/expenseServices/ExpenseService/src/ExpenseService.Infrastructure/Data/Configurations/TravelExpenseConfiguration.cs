using ExpenseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseService.Infrastructure.Data.Configurations;

public class TravelExpenseConfiguration : IEntityTypeConfiguration<TravelExpense>
{
    public void Configure(EntityTypeBuilder<TravelExpense> builder)
    {
        builder.ToTable("TRAVEL_EXPENSE");
        builder.HasKey(e => new { e.RequestNumber, e.SerialNumber });

        builder.Property(e => e.RequestNumber).HasColumnName("TR_REQ_NUM");
        builder.Property(e => e.SerialNumber).HasColumnName("TR_SRL_NUM");
        builder.Property(e => e.ExpenseCode).HasColumnName("TR_EXP_COD");
        builder.Property(e => e.CurrencyType).HasColumnName("TR_CUR_TYP").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.EligibleAmount).HasColumnName("TR_ELG_AMT");
        builder.Property(e => e.BudgetAmount).HasColumnName("TR_BUD_AMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.CompanyExpense).HasColumnName("TR_ACT_UNT").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.SelfExpense).HasColumnName("TR_ACT_SLF").HasColumnType("decimal(19,0)");
        builder.Property(e => e.VarianceAmount).HasColumnName("TR_VAR_AMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.ExpenseRemarks).HasColumnName("TR_EXP_REM").HasMaxLength(200);
        builder.Property(e => e.TransactionNumber).HasColumnName("TR_TRN_NUM");
        builder.Property(e => e.ExpenseAnnexure).HasColumnName("TR_EXP_ANX").HasColumnType("decimal(19,0)");

        builder.HasMany(e => e.Allocations)
            .WithOne(a => a.TravelExpense)
            .HasForeignKey(a => new { a.RequestNumber, a.ExpenseSerialNumber });

        builder.HasMany(e => e.SubDetails)
            .WithOne(s => s.TravelExpense)
            .HasForeignKey(s => new { s.RequestNumber, s.SerialNumber });

        builder.Ignore(e => e.DomainEvents);
    }
}
