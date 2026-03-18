using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReviewService.Domain.Entities;

namespace ReviewService.Infrastructure.Data.Configurations;

public class CourseReviewMainConfiguration : IEntityTypeConfiguration<CourseReviewMain>
{
    public void Configure(EntityTypeBuilder<CourseReviewMain> builder)
    {
        builder.ToTable("COURSE_REVIEWMAIN");
        builder.HasKey(x => x.RvCrsId);
        builder.Property(x => x.RvCrsId).HasColumnName("RV_CRS_ID").ValueGeneratedNever();
        builder.Property(x => x.RvUsrId).HasColumnName("RV_USR_ID").HasColumnType("CHAR(1)");
        builder.Property(x => x.RvRevDat).HasColumnName("RV_REV_DAT").HasColumnType("datetime2(3)");
        builder.Property(x => x.RvGenRem).HasColumnName("RV_GEN_REM").HasMaxLength(255);
        builder.Property(x => x.RqSupUsr).HasColumnName("RQ_SUP_USR").HasMaxLength(255);
        builder.Property(x => x.RvSrlNum).HasColumnName("RV_SRL_NUM").HasColumnType("CHAR(1)");
        builder.Property(x => x.RvSupRem).HasColumnName("RV_SUP_REM").HasColumnType("CHAR(1)");
        builder.Property(x => x.RvRatPer).HasColumnName("RV_RAT_PER").HasColumnType("CHAR(1)");
        builder.Property(x => x.RvFilNam).HasColumnName("RV_FIL_NAM").HasColumnType("CHAR(1)");
        builder.Property(x => x.RvNxtDat).HasColumnName("RV_NXT_DAT").HasColumnType("datetime2(3)");
        builder.Property(x => x.RvOrgDat).HasColumnName("RV_ORG_DAT").HasColumnType("datetime2(3)");

        builder.HasMany(x => x.ReviewSubs)
            .WithOne(x => x.ReviewMain)
            .HasForeignKey(x => x.RvCrsId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class CourseReviewSubConfiguration : IEntityTypeConfiguration<CourseReviewSub>
{
    public void Configure(EntityTypeBuilder<CourseReviewSub> builder)
    {
        builder.ToTable("COURSE_REVIEWSUB");
        builder.HasKey(x => new { x.RvCrsId, x.RvUsrId });
        builder.Property(x => x.RvCrsId).HasColumnName("RV_CRS_ID");
        builder.Property(x => x.RvUsrId).HasColumnName("RV_USR_ID").HasColumnType("CHAR(1)");
        builder.Property(x => x.RvSrlNum).HasColumnName("RV_SRL_NUM");
        builder.Property(x => x.RvTypCod).HasColumnName("RV_TYP_COD").HasColumnType("CHAR(1)");
        builder.Property(x => x.RvTypNum).HasColumnName("RV_TYP_NUM");
    }
}

public class FeedMastConfiguration : IEntityTypeConfiguration<FeedMast>
{
    public void Configure(EntityTypeBuilder<FeedMast> builder)
    {
        builder.ToTable("FEED_MAST");
        builder.HasKey(x => x.FdTypCod);
        builder.Property(x => x.FdTypCod).HasColumnName("FD_TYP_COD").ValueGeneratedNever();
        builder.Property(x => x.FdTypNam).HasColumnName("FD_TYP_NAM").HasMaxLength(500).IsRequired();
        builder.Property(x => x.FdNumTyp).HasColumnName("FD_NUM_TYP").HasColumnType("CHAR(1)");
        builder.Property(x => x.FdEvlCod).HasColumnName("FD_EVL_COD").HasMaxLength(10);
    }
}

public class FeedEvalMastConfiguration : IEntityTypeConfiguration<FeedEvalMast>
{
    public void Configure(EntityTypeBuilder<FeedEvalMast> builder)
    {
        builder.ToTable("FEED_EVALMAST");
        builder.HasKey(x => x.FdEvlTyp);
        builder.Property(x => x.FdEvlTyp).HasColumnName("FD_EVL_TYP").ValueGeneratedNever();
        builder.Property(x => x.FdEvlDes).HasColumnName("FD_EVL_DES").HasMaxLength(2000);
        builder.Property(x => x.FdWgtNum).HasColumnName("FD_WGT_NUM").HasColumnType("decimal(19,0)");
    }
}

public class TrainerFeedConfiguration : IEntityTypeConfiguration<TrainerFeed>
{
    public void Configure(EntityTypeBuilder<TrainerFeed> builder)
    {
        builder.ToTable("TRAINER_FEED");
        builder.HasKey(x => new { x.TrGrpCod, x.TrFedNum });
        builder.Property(x => x.TrGrpCod).HasColumnName("TR_GRP_COD");
        builder.Property(x => x.TrFedNum).HasColumnName("TR_FED_NUM");
        builder.Property(x => x.TrSrlNum).HasColumnName("TR_SRL_NUM");
        builder.Property(x => x.TrQtnGrp).HasColumnName("TR_QTN_GRP").HasMaxLength(255);
        builder.Property(x => x.TrGrpNum).HasColumnName("TR_GRP_NUM");
        builder.Property(x => x.TrWgtNum).HasColumnName("TR_WGT_NUM");
        builder.Property(x => x.TrEffDat).HasColumnName("TR_EFF_DAT").HasColumnType("datetime2(3)");
        builder.Property(x => x.TrClsDat).HasColumnName("TR_CLS_DAT").HasColumnType("datetime2(3)");
    }
}
