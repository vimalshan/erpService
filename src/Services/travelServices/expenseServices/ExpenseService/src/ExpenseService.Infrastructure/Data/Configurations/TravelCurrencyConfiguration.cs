using ExpenseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseService.Infrastructure.Data.Configurations;

public class TravelCurrencyConfiguration : IEntityTypeConfiguration<TravelCurrency>
{
    public void Configure(EntityTypeBuilder<TravelCurrency> builder)
    {
        builder.ToTable("TRAVEL_CURRENCY");
        builder.HasKey(e => new { e.RequestNumber, e.SerialNumber });

        builder.Property(e => e.RequestNumber).HasColumnName("TC_REQ_NUM");
        builder.Property(e => e.SerialNumber).HasColumnName("TC_SRL_NO");
        builder.Property(e => e.CurrencyCode).HasColumnName("TC_CUR_COD").HasMaxLength(5);
        builder.Property(e => e.CashAmount).HasColumnName("TC_CSH_AMT");
        builder.Property(e => e.TravellerChequeAmount).HasColumnName("TC_TC_AMT");
        builder.Property(e => e.DenominationFlag).HasColumnName("TC_DNM_FLG").HasMaxLength(1).IsFixedLength();
        builder.Property(e => e.DenominationText).HasColumnName("TC_DNM_TXT").HasMaxLength(2000);

        builder.Ignore(e => e.DomainEvents);
    }
}
