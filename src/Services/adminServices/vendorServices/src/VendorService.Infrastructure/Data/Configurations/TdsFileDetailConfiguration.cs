using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VendorService.Domain.Entities;

namespace VendorService.Infrastructure.Data.Configurations;

internal sealed class TdsFileDetailConfiguration : IEntityTypeConfiguration<TdsFileDetail>
{
    public void Configure(EntityTypeBuilder<TdsFileDetail> builder)
    {
        builder.ToTable("TDSFILE_DETAILS");

        builder.HasKey(f => f.FileId);
        builder.Property(f => f.FileId).HasColumnName("FILE_ID").ValueGeneratedNever();
        builder.Property(f => f.FileName).HasColumnName("FILE_NAME").HasMaxLength(100);
        builder.Property(f => f.PanNo).HasColumnName("PAN_NO").HasMaxLength(15);
        builder.Property(f => f.EmailStatus).HasColumnName("EMAIL_STATUS").HasMaxLength(1);
        builder.Property(f => f.FileType).HasColumnName("FILE_TYPE").HasMaxLength(3);

        builder.Ignore(f => f.DomainEvents);
    }
}
