using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReviewService.Domain.Entities;

namespace ReviewService.Infrastructure.Data.Configurations;

public class ReviewMainConfiguration : IEntityTypeConfiguration<ReviewMain>
{
    public void Configure(EntityTypeBuilder<ReviewMain> builder)
    {
        builder.ToTable("REVIEW_MAIN");
        builder.HasKey(x => x.RevSrlNum);
        builder.Property(x => x.RevSrlNum).HasColumnName("REV_SRL_NUM").ValueGeneratedNever();
        builder.Property(x => x.RevFedNum).HasColumnName("REV_FED_NUM");
        builder.Property(x => x.RevRemMrk1).HasColumnName("REV_REM_MRK1").HasMaxLength(4000);
        builder.Property(x => x.RevRemMrk2).HasColumnName("REV_REM_MRK2").HasMaxLength(4000);
        builder.Property(x => x.RevRemMrk3).HasColumnName("REV_REM_MRK3").HasMaxLength(4000);
        builder.Property(x => x.RevRemMrk4).HasColumnName("REV_REM_MRK4").HasMaxLength(4000);
        builder.Property(x => x.RevRemMrk5).HasColumnName("REV_REM_MRK5").HasMaxLength(4000);
        builder.Property(x => x.RevRemMrk6).HasColumnName("REV_REM_MRK6").HasMaxLength(4000);
        builder.Property(x => x.RevRemMrk7).HasColumnName("REV_REM_MRK7").HasMaxLength(4000);
        builder.Property(x => x.RevRemMrk8).HasColumnName("REV_REM_MRK8").HasMaxLength(4000);
        builder.Property(x => x.RevRemMrk9).HasColumnName("REV_REM_MRK9").HasMaxLength(4000);
        builder.Property(x => x.RevRemMrk10).HasColumnName("REV_REM_MRK10").HasMaxLength(4000);
        builder.Property(x => x.RevEntDate).HasColumnName("REV_ENT_DATE").HasMaxLength(2000);
        builder.Property(x => x.RevStatus).HasColumnName("REV_STATUS").HasColumnType("CHAR(1)");
        builder.Property(x => x.RevNextDate).HasColumnName("REV_NEXT_DATE").HasColumnType("datetime2(3)");

        builder.HasMany(x => x.ReviewSubs)
            .WithOne(x => x.ReviewMain)
            .HasForeignKey(x => x.RevMainSrl)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ReviewSubConfiguration : IEntityTypeConfiguration<ReviewSub>
{
    public void Configure(EntityTypeBuilder<ReviewSub> builder)
    {
        builder.ToTable("REVIEW_SUB");
        builder.HasKey(x => new { x.RevMainSrl, x.RevRevNum });
        builder.Property(x => x.RevMainSrl).HasColumnName("REV_MAIN_SRL");
        builder.Property(x => x.RevRevNum).HasColumnName("REV_REV_NUM");
        builder.Property(x => x.RevNextStatus).HasColumnName("REV_NEXT_STATUS").HasColumnType("CHAR(1)");
        builder.Property(x => x.RevDate).HasColumnName("REV_DATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.RevBy).HasColumnName("REV_BY");
        builder.Property(x => x.RevRemMrk).HasColumnName("REV_REM_MRK").HasMaxLength(4000);
        builder.Property(x => x.RevStatus).HasColumnName("REV_STATUS").HasMaxLength(10);
        builder.Property(x => x.RevProgRem).HasColumnName("REV_PROG_REM").HasMaxLength(4000);
    }
}

public class ReviewMastConfiguration : IEntityTypeConfiguration<ReviewMast>
{
    public void Configure(EntityTypeBuilder<ReviewMast> builder)
    {
        builder.ToTable("REVIEW_MAST");
        builder.HasKey(x => x.RvTypCod);
        builder.Property(x => x.RvTypCod).HasColumnName("RV_TYP_COD").HasColumnType("CHAR(3)");
        builder.Property(x => x.RvTypNam).HasColumnName("RV_TYP_NAM").HasMaxLength(65).IsRequired();
        builder.Property(x => x.RvGrpCod).HasColumnName("RV_GRP_COD").HasMaxLength(65).IsRequired();
    }
}

public class ReviewSkillConfiguration : IEntityTypeConfiguration<ReviewSkill>
{
    public void Configure(EntityTypeBuilder<ReviewSkill> builder)
    {
        builder.ToTable("REVIEW_SKILL");
        builder.HasKey(x => x.SkReqId);
        builder.Property(x => x.SkReqId).HasColumnName("SK_REQ_ID").ValueGeneratedNever();
        builder.Property(x => x.SkSrlNum).HasColumnName("SK_SRL_NUM");
        builder.Property(x => x.SkActNum).HasColumnName("SK_ACT_NUM");
        builder.Property(x => x.SkRevNum).HasColumnName("SK_REV_NUM");
        builder.Property(x => x.SkSklCod).HasColumnName("SK_SKL_COD");
        builder.Property(x => x.SkLvlNum).HasColumnName("SK_LVL_NUM");
        builder.Property(x => x.SkRatPer).HasColumnName("SK_RAT_PER").HasColumnType("decimal(38,0)");
        builder.Property(x => x.SkRemMrk).HasColumnName("SK_REM_MRK").HasMaxLength(255).IsRequired();
    }
}
