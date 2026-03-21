using ArchiveService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArchiveService.Infrastructure.Persistence.Configurations;

public class ArchivedToolKitConfiguration : IEntityTypeConfiguration<ArchivedToolKit>
{
    public void Configure(EntityTypeBuilder<ArchivedToolKit> builder)
    {
        builder.ToTable("TOOL_KIT_DUP");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
        builder.Property(e => e.KitCode).HasColumnName("KIT_CODE").HasMaxLength(10);
        builder.Property(e => e.AppPassword).HasColumnName("APP_PASSWORD").HasMaxLength(10);
        builder.Property(e => e.InstPassword).HasColumnName("INST_PASSWORD").HasMaxLength(10);
        builder.Property(e => e.ImeiNo).HasColumnName("IMEI_NO").HasMaxLength(50);
        builder.Property(e => e.EngineerId).HasColumnName("ENGINEER_ID").HasMaxLength(15);
        builder.Property(e => e.Flag).HasColumnName("FLAG").HasMaxLength(5);
        builder.Property(e => e.EnteredOn).HasColumnName("ENTERED_ON");
        builder.Property(e => e.EnteredBy).HasColumnName("ENTERED_BY").HasMaxLength(15);
        builder.Property(e => e.ChangedOn).HasColumnName("CHANGED_ON");
        builder.Property(e => e.ChangedBy).HasColumnName("CHANGED_BY").HasMaxLength(15);

        builder.HasMany(e => e.Transactions)
            .WithOne()
            .HasForeignKey(t => t.ToolkitId)
            .HasPrincipalKey(e => e.Id);
    }
}
