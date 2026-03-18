using FillingOperationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FillingOperationService.Infrastructure.Persistence.Configurations;

public class FillingCapacityConfiguration : IEntityTypeConfiguration<FillingCapacity>
{
    public void Configure(EntityTypeBuilder<FillingCapacity> builder)
    {
        builder.ToTable("FILLING_CAPACITY");
        builder.HasKey(x => new { x.FillingPointGroupId, x.MainProductId, x.PackageTypeId });
        builder.Property(x => x.FillingPointGroupId).HasColumnName("FILLING_POINT_GROUP_ID");
        builder.Property(x => x.MainProductId).HasColumnName("MAIN_PRODUCT_ID");
        builder.Property(x => x.PackageTypeId).HasColumnName("PACKAGE_TYPE_ID");
        builder.Property(x => x.ItemCapacityId).HasColumnName("ITEM_CAPACITY_ID").IsRequired();
        builder.Property(x => x.CapacityPerShift).HasColumnName("CAPACITY_PER_SHIFT").IsRequired();
        builder.Property(x => x.UsagePriority).HasColumnName("USAGE_PRIORITY").IsRequired();
        builder.Property(x => x.SciUserIdCreated).HasColumnName("SCI_USERID_CREATED").IsRequired();
        builder.Property(x => x.CreationDate).HasColumnName("CREATION_DATE").IsRequired();
        builder.Property(x => x.SciUserIdModified).HasColumnName("SCI_USERID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE");

        builder.Ignore(x => x.Id);
        builder.Ignore(x => x.DomainEvents);
    }
}
