using ArchiveService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArchiveService.Infrastructure.Persistence.Configurations;

public class ArchivedServiceOrderDetailConfiguration : IEntityTypeConfiguration<ArchivedServiceOrderDetail>
{
    public void Configure(EntityTypeBuilder<ArchivedServiceOrderDetail> builder)
    {
        builder.ToTable("SERVICE_ORDER_DET_DUP");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
        builder.Property(e => e.SernoDell).HasColumnName("SERNO_DELL").HasMaxLength(12);
        builder.Property(e => e.PartNo).HasColumnName("PART_NO").HasMaxLength(50);
        builder.Property(e => e.Quantity).HasColumnName("QUANTITY").HasMaxLength(15);
        builder.Property(e => e.UniqueId).HasColumnName("UNIQUE_ID").HasMaxLength(10);
        builder.Property(e => e.PartStatus).HasColumnName("PART_STATUS").HasMaxLength(10);
        builder.Property(e => e.EnteredOn).HasColumnName("ENTERED_ON");
        builder.Property(e => e.EnteredBy).HasColumnName("ENTERED_BY").HasMaxLength(15);
        builder.Property(e => e.ChangedOn).HasColumnName("CHANGED_ON");
        builder.Property(e => e.ChangedBy).HasColumnName("CHANGED_BY").HasMaxLength(15);
    }
}
