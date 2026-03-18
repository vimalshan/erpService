using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustService.Domain.Entities;

namespace TrustService.Infrastructure.Persistence.Configurations;

public class TrustFundTypeConfiguration : IEntityTypeConfiguration<TrustFundType>
{
    public void Configure(EntityTypeBuilder<TrustFundType> builder)
    {
        builder.ToTable("TRUST_FUND_TYPE");

        builder.HasKey(f => new { f.FundTrustCode, f.FundType });

        builder.Property(f => f.FundTrustCode).HasColumnName("FUND_TRUST_CODE").HasMaxLength(3).IsFixedLength();
        builder.Property(f => f.FundType).HasColumnName("FUND_TYPE").HasMaxLength(3).IsFixedLength();
        builder.Property(f => f.FundName).HasColumnName("FUND_NAME").HasMaxLength(65).IsRequired();
        builder.Property(f => f.FundPrefix).HasColumnName("FUND_PREFIX").HasMaxLength(65).IsRequired();
        builder.Property(f => f.FundStatus).HasColumnName("FUND_STATUS").HasMaxLength(1).IsFixedLength().HasDefaultValue("A");

        builder.HasIndex(f => new { f.FundTrustCode, f.FundStatus }).HasDatabaseName("IDX_TRUST_FUND_TYPE_TRUST");

        builder.Ignore(f => f.DomainEvents);
    }
}
