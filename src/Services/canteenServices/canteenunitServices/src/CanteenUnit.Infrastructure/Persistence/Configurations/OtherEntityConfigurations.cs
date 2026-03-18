using CanteenUnit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CanteenUnit.Infrastructure.Persistence.Configurations;

public class CanteenMasterCatConfiguration : IEntityTypeConfiguration<CanteenMasterCat>
{
    public void Configure(EntityTypeBuilder<CanteenMasterCat> builder)
    {
        builder.ToTable("CANTEEN_MASTER_CAT");
        builder.Property<long>("Id").ValueGeneratedOnAdd().HasColumnName("ID");
        builder.HasKey("Id");
        builder.Property(e => e.CnComCod).HasColumnName("CN_COM_COD");
        builder.Property(e => e.CnCanNum).HasColumnName("CN_CAN_NUM");
        builder.Property(e => e.CnGrdTyp).HasColumnName("CN_GRD_TYP").HasMaxLength(1);
    }
}

public class CanteenMasterGradeCatConfiguration : IEntityTypeConfiguration<CanteenMasterGradeCat>
{
    public void Configure(EntityTypeBuilder<CanteenMasterGradeCat> builder)
    {
        builder.ToTable("CANTEEN_MASTER_GRADECAT");
        builder.Property<long>("Id").ValueGeneratedOnAdd().HasColumnName("ID");
        builder.HasKey("Id");
        builder.Property(e => e.CnCanSeq).HasColumnName("CN_CAN_SEQ");
        builder.Property(e => e.CnComCod).HasColumnName("CN_COM_COD").HasColumnType("DECIMAL(38,0)");
        builder.Property(e => e.CnCanNum).HasColumnName("CN_CAN_NUM");
        builder.Property(e => e.CnCanFro).HasColumnName("CN_CAN_FRO");
        builder.Property(e => e.CnCanTo).HasColumnName("CN_CAN_TO");
        builder.Property(e => e.CnLivFlg).HasColumnName("CN_LIV_FLG").HasMaxLength(1);
        builder.Property(e => e.CnGrdCat).HasColumnName("CN_GRD_CAT").HasMaxLength(3);
    }
}

public class CanteenUnitAccessConfiguration : IEntityTypeConfiguration<CanteenUnitAccess>
{
    public void Configure(EntityTypeBuilder<CanteenUnitAccess> builder)
    {
        builder.ToTable("CANTEEN_UNIT_ACCESS");
        builder.HasKey(e => e.UnUntAcc);
        builder.Property(e => e.UnUntAcc).HasColumnName("UN_UNT_ACC").IsRequired();
        builder.Property(e => e.UnComCod).HasColumnName("UN_COM_COD");
        builder.Property(e => e.UnUsrId).HasColumnName("UN_USR_ID");
        builder.Property(e => e.UnEntUsr).HasColumnName("UN_ENT_USR");
        builder.Property(e => e.UnEntOn).HasColumnName("UN_ENT_ON");
        builder.Property(e => e.UnClsDat).HasColumnName("UN_CLS_DAT");
    }
}

public class GenCounterConfiguration : IEntityTypeConfiguration<GenCounter>
{
    public void Configure(EntityTypeBuilder<GenCounter> builder)
    {
        builder.ToTable("GEN_COUNTER");
        builder.HasKey(e => e.GnTrnTyp);
        builder.Property(e => e.GnTrnTyp).HasColumnName("GN_TRN_TYP").HasMaxLength(3).IsRequired();
        builder.Property(e => e.GnTrnNum).HasColumnName("GN_TRN_NUM");
        builder.Property(e => e.GnTrnDes).HasColumnName("GN_TRN_DES").HasMaxLength(200);
    }
}
