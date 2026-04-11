using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelService.Domain.Entities.Forex;

namespace TravelService.Infrastructure.Persistence.Configurations;

public class ForexChequeDetailConfiguration : IEntityTypeConfiguration<ForexChequeDetail>
{
    public void Configure(EntityTypeBuilder<ForexChequeDetail> builder)
    {
        builder.ToTable("TOURPLAN_FOREXCHQDET");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("FOREX_CHQDETID").HasMaxLength(255);
        builder.Property(x => x.ForexRequestId).HasColumnName("FOREX_REQID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ChequeNo).HasColumnName("FOREX_CHQNO").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ChequeDate).HasColumnName("FOREX_CHQDATE");
        builder.Property(x => x.BankName).HasColumnName("FOREX_BANKNAME").HasMaxLength(200).IsRequired();
        builder.Ignore(x => x.DomainEvents);
    }
}
