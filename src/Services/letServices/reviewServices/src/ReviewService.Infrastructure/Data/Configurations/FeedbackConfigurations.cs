using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReviewService.Domain.Entities;

namespace ReviewService.Infrastructure.Data.Configurations;

public class CourseFeedMainConfiguration : IEntityTypeConfiguration<CourseFeedMain>
{
    public void Configure(EntityTypeBuilder<CourseFeedMain> builder)
    {
        builder.ToTable("COURSE_FEEDMAIN");
        builder.HasKey(x => new { x.FdUsrId, x.FdCrsId });
        builder.Property(x => x.FdCrsId).HasColumnName("FD_CRS_ID");
        builder.Property(x => x.FdUsrId).HasColumnName("FD_USR_ID").HasMaxLength(255);
        builder.Property(x => x.FdRevDat).HasColumnName("FD_REV_DAT").HasColumnType("datetime2(3)");
        builder.Property(x => x.FdGenRem).HasColumnName("FD_GEN_REM").HasMaxLength(255).IsRequired();
        builder.Property(x => x.FdReqNum).HasColumnName("FD_REQ_NUM");
        builder.Property(x => x.FdModDat).HasColumnName("FD_MOD_DAT").HasColumnType("datetime2(3)");
        builder.Property(x => x.FdSrlNum).HasColumnName("FD_SRL_NUM");

        builder.HasMany(x => x.FeedSubs)
            .WithOne(x => x.FeedMain)
            .HasForeignKey(x => x.FdReqNum)
            .HasPrincipalKey(x => x.FdReqNum)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class CourseFeedSubConfiguration : IEntityTypeConfiguration<CourseFeedSub>
{
    public void Configure(EntityTypeBuilder<CourseFeedSub> builder)
    {
        builder.ToTable("COURSE_FEEDSUB");
        builder.HasKey(x => new { x.FdReqNum, x.FdReqSrl, x.FdSrlNum });
        builder.Property(x => x.FdReqNum).HasColumnName("FD_REQ_NUM");
        builder.Property(x => x.FdReqSrl).HasColumnName("FD_REQ_SRL");
        builder.Property(x => x.FdSrlNum).HasColumnName("FD_SRL_NUM");
        builder.Property(x => x.FdTypCod).HasColumnName("FD_TYP_COD");
        builder.Property(x => x.FdTypNum).HasColumnName("FD_TYP_NUM");
        builder.Property(x => x.FdTypDes).HasColumnName("FD_TYP_DES").HasMaxLength(65);
    }
}

public class CourseFeedbackMainConfiguration : IEntityTypeConfiguration<CourseFeedbackMain>
{
    public void Configure(EntityTypeBuilder<CourseFeedbackMain> builder)
    {
        builder.ToTable("COURSE_FEEDBACKMAIN");
        builder.HasKey(x => new { x.FdFedNum, x.FdNomNum });
        builder.Property(x => x.FdFedNum).HasColumnName("FD_FED_NUM");
        builder.Property(x => x.FdNomNum).HasColumnName("FD_NOM_NUM");
        builder.Property(x => x.FdStsCod).HasColumnName("FD_STS_COD").HasColumnType("CHAR(1)");
        builder.Property(x => x.FdFedDat).HasColumnName("FD_FED_DAT").HasColumnType("datetime2(3)");
        builder.Property(x => x.FdModDat).HasColumnName("FD_MOD_DAT").HasColumnType("datetime2(3)");
        builder.Property(x => x.FdFinRat).HasColumnName("FD_FIN_RAT");
        builder.Property(x => x.FdRemLin1).HasColumnName("FD_REM_LIN1").HasMaxLength(255);
        builder.Property(x => x.FdRemLin2).HasColumnName("FD_REM_LIN2").HasMaxLength(255);
        builder.Property(x => x.FdRemLin3).HasColumnName("FD_REM_LIN3").HasMaxLength(255);
        builder.Property(x => x.FdRevSrl).HasColumnName("FD_REV_SRL").HasColumnType("decimal(38,0)");
        builder.Property(x => x.FdCancelRem).HasColumnName("FD_CANCEL_REM").HasMaxLength(255);
        builder.Property(x => x.FdReqNum).HasColumnName("FD_REQ_NUM");
        builder.Property(x => x.FdRemLin9).HasColumnName("FD_REM_LIN9").HasMaxLength(255);
        builder.Property(x => x.FdRemLin4).HasColumnName("FD_REM_LIN4").HasMaxLength(255);
        builder.Property(x => x.FdRemLin5).HasColumnName("FD_REM_LIN5").HasMaxLength(255);
        builder.Property(x => x.FdRemLin6).HasColumnName("FD_REM_LIN6");
        builder.Property(x => x.FdRemLin7).HasColumnName("FD_REM_LIN7").HasMaxLength(255);
        builder.Property(x => x.FdRemLin8).HasColumnName("FD_REM_LIN8").HasMaxLength(255);
    }
}

public class CourseFeedbackSubConfiguration : IEntityTypeConfiguration<CourseFeedbackSub>
{
    public void Configure(EntityTypeBuilder<CourseFeedbackSub> builder)
    {
        builder.ToTable("COURSE_FEEDBACKSUB");
        builder.HasKey(x => new { x.FdFedNum, x.FdFedTyp });
        builder.Property(x => x.FdFedNum).HasColumnName("FD_FED_NUM");
        builder.Property(x => x.FdFedTyp).HasColumnName("FD_FED_TYP");
        builder.Property(x => x.FdRatNum).HasColumnName("FD_RAT_NUM");
        builder.Property(x => x.FdRemMrk).HasColumnName("FD_REM_MRK").HasMaxLength(4000);
    }
}
