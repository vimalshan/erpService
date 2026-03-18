using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TdsService.Domain.Entities;
using TdsService.Domain.ValueObjects;

namespace TdsService.Infrastructure.Persistence.Configurations;

public sealed class TdsFileConfiguration : IEntityTypeConfiguration<TdsFile>
{
    public void Configure(EntityTypeBuilder<TdsFile> builder)
    {
        builder.ToTable("TDSFILE_DETAILS");

        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id)
            .HasColumnName("FILE_ID")
            .ValueGeneratedNever();

        builder.Property(f => f.FileName)
            .HasColumnName("FILE_NAME")
            .HasMaxLength(100)
            .IsRequired();

        // PanNumber value object — VARCHAR(15)
        builder.OwnsOne(f => f.PanNumber, pan =>
        {
            pan.Property(p => p.Value)
                .HasColumnName("PAN_NO")
                .HasMaxLength(15);
            pan.HasIndex(p => p.Value)
                .HasDatabaseName("IDX_TDSFILE_PANNO");
        });

        // EmailStatus — stored as VARCHAR(1) 'Y'/'N'
        builder.Property(f => f.EmailStatus)
            .HasColumnName("EMAIL_STATUS")
            .HasMaxLength(1)
            .HasConversion(
                v => v.ToDbValue(),
                v => EmailStatusExtensions.FromDbValue(v));

        // FileType stored as VARCHAR(3)
        builder.OwnsOne(f => f.FileType, ft =>
        {
            ft.Property(t => t.Value)
                .HasColumnName("FILE_TYPE")
                .HasMaxLength(3);
        });

        // Audit fields — not in original schema but added for tracking
        builder.Property(f => f.BlobStorageUri)
            .HasColumnName("BLOB_URI")
            .HasMaxLength(2048)
            .IsRequired(false);

        builder.Property(f => f.CreatedAt)
            .HasColumnName("CREATED_AT")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(f => f.UpdatedAt)
            .HasColumnName("UPDATED_AT")
            .IsRequired(false);
    }
}
