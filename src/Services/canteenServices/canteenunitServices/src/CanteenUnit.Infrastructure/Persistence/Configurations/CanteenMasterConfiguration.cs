using CanteenUnit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CanteenUnit.Infrastructure.Persistence.Configurations;

public class CanteenMasterConfiguration : IEntityTypeConfiguration<CanteenMaster>
{
    public void Configure(EntityTypeBuilder<CanteenMaster> builder)
    {
        builder.ToTable("CANTEEN_MASTER");
        builder.HasKey(e => e.CnComCod);
        builder.Property(e => e.CnComCod).HasColumnName("CN_COM_COD").HasColumnType("DECIMAL(38,0)").IsRequired();
        builder.Property(e => e.CnCanNum).HasColumnName("CN_CAN_NUM");
        builder.Property(e => e.CnCanFro).HasColumnName("CN_CAN_FRO");
        builder.Property(e => e.CnCanTo).HasColumnName("CN_CAN_TO");
        builder.Property(e => e.CnLivFlg).HasColumnName("CN_LIV_FLG").HasMaxLength(1);
        builder.Property(e => e.CnEntUsr).HasColumnName("CN_ENT_USR").HasColumnType("DECIMAL(38,0)");
        builder.Property(e => e.CnEntDat).HasColumnName("CN_ENT_DAT");
        builder.Property(e => e.CnRemMrk).HasColumnName("CN_REM_MRK").HasMaxLength(200);

        builder.HasIndex(e => e.CnCanNum).HasDatabaseName("IDX_CANTEEN_MASTER_CN_CAN_NUM");

        builder.Ignore(e => e.Categories);
        builder.Ignore(e => e.GradeCategories);
    }
}
