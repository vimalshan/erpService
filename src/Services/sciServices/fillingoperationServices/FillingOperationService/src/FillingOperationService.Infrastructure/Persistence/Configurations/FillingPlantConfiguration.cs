using FillingOperationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FillingOperationService.Infrastructure.Persistence.Configurations;

public class FillingPlantConfiguration : IEntityTypeConfiguration<FillingPlant>
{
    public void Configure(EntityTypeBuilder<FillingPlant> builder)
    {
        builder.ToTable("FILLING_PLANT");
        builder.HasKey(x => x.FillingPlantId);
        builder.Property(x => x.FillingPlantId).HasColumnName("FILLING_PLANT_ID").ValueGeneratedOnAdd();
        builder.Property(x => x.CompanyUnitId).HasColumnName("COMPANY_UNIT_ID").IsRequired();
        builder.Property(x => x.FillingPlantName).HasColumnName("FILLING_PLANT_NAME").HasMaxLength(40).IsRequired();
        builder.Property(x => x.Location).HasColumnName("LOCATION").HasMaxLength(20).IsRequired();
        builder.Property(x => x.SciUserIdCreated).HasColumnName("SCI_USER_ID_CREATED").IsRequired();
        builder.Property(x => x.CreationDate).HasColumnName("CREATION_DATE").IsRequired();
        builder.Property(x => x.SciUserIdModified).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE");

        builder.Ignore(x => x.Id);
        builder.Ignore(x => x.DomainEvents);

        builder.HasMany(x => x.FillingLines)
               .WithOne()
               .HasForeignKey(fl => fl.FillingPlantId);

        builder.Navigation(x => x.FillingLines)
               .HasField("_fillingLines")
               .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
