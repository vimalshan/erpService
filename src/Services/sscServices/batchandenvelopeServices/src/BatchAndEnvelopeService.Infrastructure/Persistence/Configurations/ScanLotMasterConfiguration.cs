using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BatchAndEnvelopeService.Domain.Entities;

namespace BatchAndEnvelopeService.Infrastructure.Persistence.Configurations;

public class ScanLotMasterConfiguration : IEntityTypeConfiguration<ScanLotMaster>
{
    public void Configure(EntityTypeBuilder<ScanLotMaster> builder)
    {
        builder.ToTable("SCAN_LOTMAST");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("SCAN_LOTNO").ValueGeneratedNever();
        builder.Property(x => x.UserId).HasColumnName("SCAN_USERID").IsRequired();
        builder.Property(x => x.Status).HasColumnName("SCAN_STATUS").HasMaxLength(1).IsRequired();
        builder.Property(x => x.DeviceNo).HasColumnName("SCAN_DEVICENO").IsRequired();
        builder.Property(x => x.CloseDate).HasColumnName("SCAN_CLOSEDATE");
        builder.Property(x => x.CreatedOn).HasColumnName("SCAN_CREATEDON");
        builder.Property(x => x.DeviceId).HasColumnName("SCAN_DEVICEID");
        builder.Property(x => x.ScanFlag).HasColumnName("SCAN_FLAG").HasMaxLength(1);
        builder.Ignore(x => x.DomainEvents);
    }
}
