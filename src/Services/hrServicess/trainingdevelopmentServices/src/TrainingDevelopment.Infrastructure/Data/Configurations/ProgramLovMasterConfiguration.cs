using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingDevelopment.Domain.Entities;

namespace TrainingDevelopment.Infrastructure.Data.Configurations;

public class ProgramLovMasterConfiguration : IEntityTypeConfiguration<ProgramLovMaster>
{
    public void Configure(EntityTypeBuilder<ProgramLovMaster> builder)
    {
        builder.ToTable("PROGRAMLOV_MAST");

        builder.HasKey(x => x.TypeCode);
        builder.Property(x => x.TypeCode).HasColumnName("PRLOV_TYPECODE").HasMaxLength(20);
        builder.Property(x => x.Code).HasColumnName("PRLOV_CODE").HasMaxLength(5).IsRequired();
        builder.Property(x => x.Name).HasColumnName("PRLOV_NAME").HasMaxLength(200).IsRequired();

        builder.Ignore(x => x.DomainEvents);
    }
}
