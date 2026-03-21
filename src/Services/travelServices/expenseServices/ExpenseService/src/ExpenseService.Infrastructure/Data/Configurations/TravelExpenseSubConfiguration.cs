using ExpenseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseService.Infrastructure.Data.Configurations;

public class TravelExpenseSubConfiguration : IEntityTypeConfiguration<TravelExpenseSub>
{
    public void Configure(EntityTypeBuilder<TravelExpenseSub> builder)
    {
        builder.ToTable("TRAVEL_EXPENSESUB");
        builder.HasKey(e => new { e.RequestNumber, e.SerialNumber });

        builder.Property(e => e.RequestNumber).HasColumnName("TE_REQ_NUM");
        builder.Property(e => e.SerialNumber).HasColumnName("TE_SRL_NUM");
        builder.Property(e => e.ExpenseType).HasColumnName("TE_TYP_EXP");
        builder.Property(e => e.BillAttached).HasColumnName("TE_BILL_ATT").HasMaxLength(1).IsFixedLength();
        builder.Property(e => e.CityName).HasColumnName("TE_CIT_NAM").HasMaxLength(50);
        builder.Property(e => e.TotalAmount).HasColumnName("TE_TOT_AMT");
        builder.Property(e => e.StatusCode).HasColumnName("TE_STS_COD").HasMaxLength(1).IsFixedLength();
        builder.Property(e => e.Remarks).HasColumnName("TE_REM_TXT").HasMaxLength(200);
        builder.Property(e => e.BillDate).HasColumnName("TE_BILL_DAT");

        builder.Ignore(e => e.DomainEvents);
    }
}
