using ArchiveService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArchiveService.Infrastructure.Persistence.Configurations;

public class ArchivedToolKitTransactionConfiguration : IEntityTypeConfiguration<ArchivedToolKitTransaction>
{
    public void Configure(EntityTypeBuilder<ArchivedToolKitTransaction> builder)
    {
        builder.ToTable("TOOLKIT_TRANSACTION_DUP");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ToolkitId).HasColumnName("TOOLKIT_ID");
        builder.Property(e => e.ToolkitNameId).HasColumnName("TOOLKIT_NAME_ID");
        builder.Property(e => e.EngineerId).HasColumnName("ENGINEER_ID").HasMaxLength(15);
        builder.Property(e => e.IssuerId).HasColumnName("ISSUER_ID").HasMaxLength(15);
        builder.Property(e => e.Quantity).HasColumnName("QUANTITY");
        builder.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(20);
        builder.Property(e => e.Remarks).HasColumnName("REMARKS").HasMaxLength(20);
        builder.Property(e => e.AdditionalRemarks).HasColumnName("ADDITIONAL_REMARKS").HasMaxLength(200);
        builder.Property(e => e.EnteredOn).HasColumnName("ENTERED_ON");
        builder.Property(e => e.EnteredBy).HasColumnName("ENTERED_BY").HasMaxLength(15);
        builder.Property(e => e.ChangedOn).HasColumnName("CHANGED_ON");
        builder.Property(e => e.ChangedBy).HasColumnName("CHANGED_BY").HasMaxLength(15);
    }
}
