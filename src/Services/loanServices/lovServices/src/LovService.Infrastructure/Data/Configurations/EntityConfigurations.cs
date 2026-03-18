using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LovService.Domain.Entities;
using LovService.Domain.ValueObjects;

namespace LovService.Infrastructure.Data.Configurations;

public class LovTypeMastConfiguration : IEntityTypeConfiguration<LovTypeMast>
{
    public void Configure(EntityTypeBuilder<LovTypeMast> builder)
    {
        builder.ToTable("LOV_TYPEMAST");

        builder.HasKey(x => x.LovTypeId);
        builder.Property(x => x.LovTypeId).HasColumnName("LOV_TYPEID").ValueGeneratedNever();
        builder.Property(x => x.LovTypeName).HasColumnName("LOV_TYPENAME").HasMaxLength(100).IsRequired();
        builder.Property(x => x.LovOrgId).HasColumnName("LOV_ORGID").IsRequired();

        builder.Property(x => x.LovCategory)
            .HasColumnName("LOV_CATEGORY")
            .HasMaxLength(1)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => LovCategory.From(v[0]));

        builder.HasMany(x => x.LovMasters)
            .WithOne(x => x.LovType)
            .HasForeignKey(x => x.LovTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class LovMasterConfiguration : IEntityTypeConfiguration<LovMaster>
{
    public void Configure(EntityTypeBuilder<LovMaster> builder)
    {
        builder.ToTable("LOV_MASTER");

        builder.HasKey(x => x.LovId);
        builder.Property(x => x.LovId).HasColumnName("LOV_ID").ValueGeneratedNever();
        builder.Property(x => x.LovTypeId).HasColumnName("LOV_TYPEID").IsRequired();
        builder.Property(x => x.LovName).HasColumnName("LOV_NAME").HasMaxLength(65).IsRequired();
        builder.Property(x => x.LovCreatedOn).HasColumnName("LOV_CREATEDON").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(x => x.LovCreatedBy).HasColumnName("LOV_CREATEDBY").IsRequired();
        builder.Property(x => x.LovUpdatedBy).HasColumnName("LOV_UPDATEDBY").IsRequired();
        builder.Property(x => x.LovUpdatedOn).HasColumnName("LOV_UPDATEDON").HasColumnType("datetime2(3)").IsRequired();

        builder.HasIndex(x => x.LovTypeId).HasDatabaseName("IDX_LOV_MASTER_LOV_TYPEID");
    }
}

public class ProgramLovMastConfiguration : IEntityTypeConfiguration<ProgramLovMast>
{
    public void Configure(EntityTypeBuilder<ProgramLovMast> builder)
    {
        builder.ToTable("PROGRAMLOV_MAST");

        builder.HasKey(x => new { x.PrlovCode, x.PrlovTypeCode });
        builder.Property(x => x.PrlovTypeCode).HasColumnName("PRLOV_TYPECODE").HasMaxLength(20).IsRequired();
        builder.Property(x => x.PrlovCode).HasColumnName("PRLOV_CODE").HasMaxLength(5).IsRequired();
        builder.Property(x => x.PrlovName).HasColumnName("PRLOV_NAME").HasMaxLength(200).IsRequired();
    }
}
