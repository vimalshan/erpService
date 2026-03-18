using FillingOperationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FillingOperationService.Infrastructure.Persistence.Configurations;

public class FlSwitchoverTimeConfiguration : IEntityTypeConfiguration<FlSwitchoverTime>
{
    public void Configure(EntityTypeBuilder<FlSwitchoverTime> builder)
    {
        builder.ToTable("FL_SWITCHOVER_TIME");
        builder.HasKey(x => new { x.FillingLineId, x.FromMainProductId, x.ToMainProductId });
        builder.Property(x => x.FillingLineId).HasColumnName("FILLING_LINE_ID");
        builder.Property(x => x.FromMainProductId).HasColumnName("FROM_MAIN_PRODUCT_ID");
        builder.Property(x => x.ToMainProductId).HasColumnName("TO_MAIN_PRODUCT_ID");
        builder.Property(x => x.TimeInHours).HasColumnName("TIME_IN_HOURS").IsRequired();
        builder.Property(x => x.SciUserIdCreated).HasColumnName("SCI_USER_ID_CREATED").IsRequired();
        builder.Property(x => x.CreationDate).HasColumnName("CREATION_DATE").IsRequired();
        builder.Property(x => x.SciUserIdModified).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE");

        builder.Ignore(x => x.Id);
        builder.Ignore(x => x.DomainEvents);
    }
}
