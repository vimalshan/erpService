using FillingOperationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FillingOperationService.Infrastructure.Persistence.Configurations;

public class FillingLineProductMapConfiguration : IEntityTypeConfiguration<FillingLineProductMap>
{
    public void Configure(EntityTypeBuilder<FillingLineProductMap> builder)
    {
        builder.ToTable("FILLING_LINE_PRODUCT_MAP");
        builder.HasKey(x => new { x.FillingLineId, x.MainProductId });
        builder.Property(x => x.FillingLineId).HasColumnName("FILLING_LINE_ID");
        builder.Property(x => x.MainProductId).HasColumnName("MAIN_PRODUCT_ID").IsRequired();
        builder.Property(x => x.SciUserIdModified).HasColumnName("SCI_USER_ID_MODIFIED").IsRequired();
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE").IsRequired();

        builder.Ignore(x => x.Id);
        builder.Ignore(x => x.DomainEvents);
    }
}
