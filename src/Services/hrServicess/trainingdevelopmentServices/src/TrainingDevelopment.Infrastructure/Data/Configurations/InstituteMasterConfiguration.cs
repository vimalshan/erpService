using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingDevelopment.Domain.Entities;

namespace TrainingDevelopment.Infrastructure.Data.Configurations;

public class InstituteMasterConfiguration : IEntityTypeConfiguration<InstituteMaster>
{
    public void Configure(EntityTypeBuilder<InstituteMaster> builder)
    {
        builder.ToTable("INSTITUTE_MASTER");

        builder.HasKey(x => x.InstituteCode);
        builder.Property(x => x.InstituteCode).HasColumnName("INSTITUTE_CODE").HasColumnType("decimal(22,0)");
        builder.Property(x => x.InstituteName).HasColumnName("INSTITUTE_NAME").HasMaxLength(100);
        builder.Property(x => x.Address1).HasColumnName("INSTITUTE_ADD1").HasMaxLength(100);
        builder.Property(x => x.Address2).HasColumnName("INSTITUTE_ADD2").HasMaxLength(100);
        builder.Property(x => x.City).HasColumnName("INSTITUTE_CITY").HasMaxLength(50);
        builder.Property(x => x.State).HasColumnName("INSTITUTE_STATE").HasMaxLength(50);
        builder.Property(x => x.Pin).HasColumnName("INSTITUTE_PIN").HasMaxLength(50);
        builder.Property(x => x.Phone).HasColumnName("INSTITUTE_PHONE").HasMaxLength(50);
        builder.Property(x => x.Fax).HasColumnName("INSTITUTE_FAX").HasMaxLength(50);
        builder.Property(x => x.Email).HasColumnName("INSTITUTE_EMAIL").HasMaxLength(50);
        builder.Property(x => x.Url).HasColumnName("INSTITUTE_URL").HasMaxLength(50);
        builder.Property(x => x.InstituteType).HasColumnName("INSTITUTE_TYPE").HasMaxLength(50);
        builder.Property(x => x.CampusRecruit).HasColumnName("INSTITUTE_CAMPUSRECRUIT").HasMaxLength(1).IsRequired();
        builder.Property(x => x.InstituteClass).HasColumnName("INSTITUTE_CLASS").HasMaxLength(3);
        builder.Property(x => x.LastModifiedBy).HasColumnName("INSTITUTE_MODIFIEDBY").HasColumnType("decimal(22,0)");
        builder.Property(x => x.LastModifiedOn).HasColumnName("INSTITUTE_MODIFIEDON").HasColumnType("datetime2(3)");

        builder.Ignore(x => x.DomainEvents);
    }
}
