using LovService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LovService.Infrastructure.Data.Configurations;

public class LovMasterConfiguration : IEntityTypeConfiguration<LovMaster>
{
    public void Configure(EntityTypeBuilder<LovMaster> builder)
    {
        builder.ToTable("LOV_MASTER");
        builder.HasKey(x => x.LovId);
        builder.Property(x => x.LovId).HasColumnName("LOV_ID").ValueGeneratedNever();
        builder.Property(x => x.LovTypeId).HasColumnName("LOV_TYPE_ID").IsRequired();
        builder.Property(x => x.LovName).HasColumnName("LOV_NAME").HasMaxLength(30).IsRequired();
        builder.Property(x => x.LovUpdatedBy).HasColumnName("LOV_UPDATED_BY").IsRequired();
        builder.Property(x => x.LovUpdatedOn).HasColumnName("LOV_UPDATED_ON").HasColumnType("datetime2(3)").IsRequired();

        builder.HasIndex(x => x.LovTypeId).HasDatabaseName("IDX_LOV_MASTER_TYPEID");
    }
}
