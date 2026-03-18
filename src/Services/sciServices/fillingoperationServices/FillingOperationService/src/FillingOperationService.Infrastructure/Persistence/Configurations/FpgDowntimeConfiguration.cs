using FillingOperationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FillingOperationService.Infrastructure.Persistence.Configurations;

public class FpgDowntimeConfiguration : IEntityTypeConfiguration<FpgDowntime>
{
    public void Configure(EntityTypeBuilder<FpgDowntime> builder)
    {
        builder.ToTable("FPG_DOWNTIME");
        builder.HasKey(x => x.FpgId);
        builder.Property(x => x.FpgId).HasColumnName("FPG_ID").ValueGeneratedOnAdd();
        builder.Property(x => x.FillingPointGroupId).HasColumnName("FILLING_POINT_GROUP_ID");
        builder.Property(x => x.StartDateTime).HasColumnName("START_DATE_TIME").IsRequired();
        builder.Property(x => x.EndDateTime).HasColumnName("END_DATE_TIME").IsRequired();
        builder.Property(x => x.NoOfFillingPoints).HasColumnName("NO_OF_FILLING_POINTS").HasMaxLength(4);
        builder.Property(x => x.DowntimeType).HasColumnName("DOWNTIME_TYPE").HasMaxLength(255);
        builder.Property(x => x.SciUserIdCreated).HasColumnName("SCI_USER_ID_CREATED");
        builder.Property(x => x.CreationDate).HasColumnName("CREATION_DATE").IsRequired();
        builder.Property(x => x.SciUserIdModified).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE");

        builder.Ignore(x => x.Id);
        builder.Ignore(x => x.DomainEvents);
    }
}
