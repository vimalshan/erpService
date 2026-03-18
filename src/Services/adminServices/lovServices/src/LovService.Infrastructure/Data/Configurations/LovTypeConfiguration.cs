using LovService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LovService.Infrastructure.Data.Configurations;

public class LovTypeConfiguration : IEntityTypeConfiguration<LovType>
{
    public void Configure(EntityTypeBuilder<LovType> builder)
    {
        builder.ToTable("LOV_TYPE");
        builder.HasKey(x => x.LovTypeId);
        builder.Property(x => x.LovTypeId).HasColumnName("LOV_TYPE_ID").ValueGeneratedNever();
        builder.Property(x => x.LovTypeName).HasColumnName("LOV_TYPE_NAME").HasMaxLength(30).IsRequired();

        builder.HasMany(x => x.LovMasters)
               .WithOne(x => x.LovType)
               .HasForeignKey(x => x.LovTypeId)
               .HasConstraintName("FK_LOV_MASTER_TYPE");
    }
}
