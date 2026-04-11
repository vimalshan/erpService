using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelService.Domain.Entities.Forex;

namespace TravelService.Infrastructure.Persistence.Configurations;

public class ForexDetailConfiguration : IEntityTypeConfiguration<ForexDetail>
{
    public void Configure(EntityTypeBuilder<ForexDetail> builder)
    {
        builder.ToTable("TOURPLAN_FOREXDET");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("FOREX_ID").HasMaxLength(255);
        builder.Property(x => x.ForexRequestId).HasColumnName("FOREX_REQID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.SourceCurrencyValue).HasColumnName("FOREX_SRCVALUE").HasPrecision(18, 4);
        builder.Property(x => x.Currency).HasColumnName("FOREX_CURRENCY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForexValue).HasColumnName("FOREX_VALUE").HasPrecision(18, 4);
        builder.Property(x => x.ExchangeRate).HasColumnName("FOREX_EXGRATE").HasPrecision(18, 4);
        builder.Property(x => x.ExchangeValue).HasColumnName("FOREX_EXGVALUE").HasPrecision(18, 4);
        builder.Property(x => x.PayMode).HasColumnName("FOREX_PAYMODE").HasMaxLength(255);
        builder.Ignore(x => x.DomainEvents);
    }
}
