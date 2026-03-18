using FillingOperationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FillingOperationService.Infrastructure.Persistence.Configurations;

public class FillingLineConfiguration : IEntityTypeConfiguration<FillingLine>
{
    public void Configure(EntityTypeBuilder<FillingLine> builder)
    {
        builder.ToTable("FILLING_LINE");
        builder.HasKey(x => x.FillingLineId);
        builder.Property(x => x.FillingLineId).HasColumnName("FILLING_LINE_ID").ValueGeneratedOnAdd();
        builder.Property(x => x.FillingPlantId).HasColumnName("FILLING_PLANT_ID").IsRequired();
        builder.Property(x => x.FillingLineName).HasColumnName("FILLING_LINE_NAME").HasMaxLength(30).IsRequired();
        builder.Property(x => x.NoOfFillingPoints).HasColumnName("NO_OF_FILLING_POINTS").IsRequired();
        builder.Property(x => x.PackageTypeId).HasColumnName("PACKAGE_TYPE_ID");
        builder.Property(x => x.IsClosed).HasColumnName("ISCLOSED").HasMaxLength(1);
        builder.Property(x => x.SciUserIdCreated).HasColumnName("SCI_USER_ID_CREATED").IsRequired();
        builder.Property(x => x.CreationDate).HasColumnName("CREATION_DATE").IsRequired();
        builder.Property(x => x.SciUserIdModified).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE");

        builder.Ignore(x => x.Id);
        builder.Ignore(x => x.DomainEvents);

        builder.HasMany(x => x.FillingPointGroups)
               .WithOne()
               .HasForeignKey(fpg => fpg.FillingLineId);

        builder.Navigation(x => x.FillingPointGroups)
               .HasField("_pointGroups")
               .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
