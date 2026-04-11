using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Persistence.Configurations;

public sealed class TravelBatchSubBreakConfiguration : IEntityTypeConfiguration<TravelBatchSubBreak>
{
    public void Configure(EntityTypeBuilder<TravelBatchSubBreak> builder)
    {
        builder.ToTable("TRAVEL_BATCHSUBBRK");
        builder.HasKey(x => x.BatchBrkId);

        builder.Property(x => x.BatchBrkId).HasColumnName("BATCHBRK_ID").HasMaxLength(255).ValueGeneratedNever();
        builder.Property(x => x.BatchSubId).HasColumnName("BATCHBRK_SUBID").HasMaxLength(255);
        builder.Property(x => x.VendorId).HasColumnName("BATCHBRK_VENDORID").HasMaxLength(255);
        builder.Property(x => x.VendorSiteId).HasColumnName("BATCHBRK_VENDORSITEID").HasMaxLength(255);
        builder.Property(x => x.JvId).HasColumnName("BATCHBRK_JVID").HasMaxLength(255);

        builder.Ignore(x => x.DomainEvents);
    }
}
