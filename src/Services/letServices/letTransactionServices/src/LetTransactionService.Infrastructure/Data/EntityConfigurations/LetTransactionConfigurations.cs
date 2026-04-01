using LetTransactionService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetTransactionService.Infrastructure.Data.EntityConfigurations;

public class LetMainConfiguration : IEntityTypeConfiguration<LetMain>
{
    public void Configure(EntityTypeBuilder<LetMain> builder)
    {
        builder.ToTable("LET_MAIN");

        builder.HasKey(e => e.RequestNumber);
        builder.Property(e => e.RequestNumber)          .HasColumnName("REQ_NUM").ValueGeneratedNever();
        builder.Property(e => e.FinancialYearSerialNo)  .HasColumnName("FINYEAR_SRLNO").IsRequired();
        builder.Property(e => e.EmployeeUserId)         .HasColumnName("EMP_USERID").HasMaxLength(25).IsRequired();
        builder.Property(e => e.SupervisorUserId)       .HasColumnName("SUP_USERID").HasMaxLength(25).IsRequired(false);
        builder.Property(e => e.RequestDate)            .HasColumnName("REQ_DATE").HasColumnType("datetime2(3)").IsRequired(false);

        builder.HasMany(e => e.SubEntries)
               .WithOne(s => s.LetMain)
               .HasForeignKey(s => s.RequestNumber)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.EmployeeUserId).HasDatabaseName("IDX_LET_MAIN_EMP_USERID");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class LetSubConfiguration : IEntityTypeConfiguration<LetSub>
{
    public void Configure(EntityTypeBuilder<LetSub> builder)
    {
        builder.ToTable("LET_SUB");

        builder.HasKey(e => e.SerialNumber);
        builder.Property(e => e.RequestNumber)              .HasColumnName("LS_REQ_NUM");
        builder.Property(e => e.SerialNumber)               .HasColumnName("LS_SRL_NUM").ValueGeneratedNever();
        builder.Property(e => e.ModifiedDate)               .HasColumnName("LS_MOD_DAT").HasColumnType("datetime2(3)").IsRequired(false);
        builder.Property(e => e.ModifiedUser)               .HasColumnName("LS_MOD_USER").HasMaxLength(25).IsRequired(false);
        builder.Property(e => e.PreferredModeDev)           .HasColumnName("LS_PREF_MODDEV").HasColumnType("char(1)").IsRequired(false);
        builder.Property(e => e.ActionTaken)                .HasColumnName("LS_ACT_TAKEN").HasMaxLength(200).IsRequired(false);
        builder.Property(e => e.CourseId)                   .HasColumnName("LS_CRS_ID").IsRequired(false);
        builder.Property(e => e.TrainingProgramBhr)         .HasColumnName("LS_TRNPRG_BHR").HasMaxLength(200).IsRequired(false);
        builder.Property(e => e.ImpactBenefitProcess)       .HasColumnName("LS_IMPBEN_PRO").HasMaxLength(200).IsRequired(false);
        builder.Property(e => e.MeasureCompetency)          .HasColumnName("LS_MEASURE_CP").HasMaxLength(200).IsRequired(false);
        builder.Property(e => e.MidYearReviewerName)        .HasColumnName("LS_MIDYER_REVNAM").HasMaxLength(200).IsRequired(false);
        builder.Property(e => e.MidYearReviewerDate)        .HasColumnName("LS_MIDYER_REVDAT").HasMaxLength(200).IsRequired(false);
        builder.Property(e => e.MidYearReviewerRemark)      .HasColumnName("LS_MIDYER_REVREM").HasMaxLength(200).IsRequired(false);
        builder.Property(e => e.AnnualReviewerName)         .HasColumnName("LS_ANNYER_REVNAM").HasMaxLength(200).IsRequired(false);
        builder.Property(e => e.AnnualReviewerDate)         .HasColumnName("LS_ANNYER_REVDAT").HasMaxLength(200).IsRequired(false);
        builder.Property(e => e.AnnualReviewerRemark)       .HasColumnName("LS_ANNYER_REVREM").HasMaxLength(200).IsRequired(false);
        builder.Property(e => e.CompetencyToDevelop)        .HasColumnName("LS_COMP_DEV").IsRequired(false);
        builder.Property(e => e.DomainKnowledgeDev)         .HasColumnName("LS_DOMKNOW_DEV").HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.DomainKnowledgeDevDetail)   .HasColumnName("LS_DOMKNOW_DEV_DET").HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.ProcessDev)                 .HasColumnName("LS_PROCES_DEV").HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.ProcessDevDetail)           .HasColumnName("LS_PROCES_DEV_DET").HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.LetSubCode)                 .HasColumnName("LS_LETSUB_CODE").HasColumnType("char(1)").IsRequired(false);
        builder.Property(e => e.ReviewType)                 .HasColumnName("LS_REV_TYPE").HasMaxLength(255).IsRequired(false);

        builder.HasIndex(e => e.RequestNumber).HasDatabaseName("IDX_LET_SUB_LS_REQ_NUM");
    }
}

public class CourseFeedbackMainConfiguration : IEntityTypeConfiguration<CourseFeedbackMain>
{
    public void Configure(EntityTypeBuilder<CourseFeedbackMain> builder)
    {
        builder.ToTable("COURSE_FEEDBACKMAIN");

        builder.HasKey(e => e.FeedbackNumber);
        builder.Property(e => e.FeedbackNumber)         .HasColumnName("FD_FED_NUM").ValueGeneratedNever();
        builder.Property(e => e.NominationNumber)       .HasColumnName("FD_NOM_NUM");
        builder.Property(e => e.StatusCode)             .HasColumnName("FD_STS_COD").HasColumnType("char(1)").IsRequired(false);
        builder.Property(e => e.FeedbackDate)           .HasColumnName("FD_FED_DAT").HasColumnType("datetime2(3)").IsRequired(false);
        builder.Property(e => e.ModifiedDate)           .HasColumnName("FD_MOD_DAT").HasColumnType("datetime2(3)").IsRequired(false);
        builder.Property(e => e.OverallRating)          .HasColumnName("FD_FIN_RAT").IsRequired(false);
        builder.Property(e => e.Remarks1)               .HasColumnName("FD_REM_LIN1").HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.Remarks2)               .HasColumnName("FD_REM_LIN2").HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.Remarks3)               .HasColumnName("FD_REM_LIN3").HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.FeedbackReviewSerial)   .HasColumnName("FD_REV_SRL").HasColumnType("decimal(38,0)").IsRequired(false);
        builder.Property(e => e.CancelRemark)           .HasColumnName("FD_CANCEL_REM").HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.RequestNumber)          .HasColumnName("FD_REQ_NUM").IsRequired(false);
        builder.Property(e => e.Remarks9)               .HasColumnName("FD_REM_LIN9").HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.Remarks4)               .HasColumnName("FD_REM_LIN4").HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.Remarks5)               .HasColumnName("FD_REM_LIN5").HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.TotalManHours)          .HasColumnName("FD_REM_LIN6").IsRequired(false);
        builder.Property(e => e.Remarks7)               .HasColumnName("FD_REM_LIN7").HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.Remarks8)               .HasColumnName("FD_REM_LIN8").HasMaxLength(255).IsRequired(false);

        builder.HasMany(e => e.FeedbackDetails)
               .WithOne(d => d.FeedbackMain)
               .HasForeignKey(d => d.FeedbackNumber)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class CourseFeedbackSubConfiguration : IEntityTypeConfiguration<CourseFeedbackSub>
{
    public void Configure(EntityTypeBuilder<CourseFeedbackSub> builder)
    {
        builder.ToTable("COURSE_FEEDBACKSUB");

        builder.HasKey(e => new { e.FeedbackNumber, e.FeedbackType });
        builder.Property(e => e.FeedbackNumber) .HasColumnName("FD_FED_NUM");
        builder.Property(e => e.FeedbackType)   .HasColumnName("FD_FED_TYP");
        builder.Property(e => e.Rating)         .HasColumnName("FD_RAT_NUM");
        builder.Property(e => e.Remarks)        .HasColumnName("FD_REM_MRK").HasMaxLength(4000).IsRequired(false);
    }
}

public class ReviewMainConfiguration : IEntityTypeConfiguration<ReviewMain>
{
    public void Configure(EntityTypeBuilder<ReviewMain> builder)
    {
        builder.ToTable("REVIEW_MAIN");

        builder.HasKey(e => e.ReviewSerialNumber);
        builder.Property(e => e.ReviewSerialNumber)     .HasColumnName("REV_SRL_NUM").ValueGeneratedNever();
        builder.Property(e => e.FeedbackNumber)         .HasColumnName("REV_FED_NUM");
        builder.Property(e => e.ImplementationGoal)     .HasColumnName("REV_REM_MRK1").HasMaxLength(4000).IsRequired(false);
        builder.Property(e => e.KeyLearning)            .HasColumnName("REV_REM_MRK2").HasMaxLength(4000).IsRequired(false);
        builder.Property(e => e.KeyStepsImplementation) .HasColumnName("REV_REM_MRK3").HasMaxLength(4000).IsRequired(false);
        builder.Property(e => e.KeyOutputsExpected)     .HasColumnName("REV_REM_MRK4").HasMaxLength(4000).IsRequired(false);
        builder.Property(e => e.MeasurementProcess)     .HasColumnName("REV_REM_MRK5").HasMaxLength(4000).IsRequired(false);
        builder.Property(e => e.HelpRequiredFromHr)     .HasColumnName("REV_REM_MRK6").HasMaxLength(4000).IsRequired(false);
        builder.Property(e => e.AdditionalRemarks1)     .HasColumnName("REV_REM_MRK7").HasMaxLength(4000).IsRequired(false);
        builder.Property(e => e.AdditionalRemarks2)     .HasColumnName("REV_REM_MRK8").HasMaxLength(4000).IsRequired(false);
        builder.Property(e => e.AdditionalRemarks3)     .HasColumnName("REV_REM_MRK9").HasMaxLength(4000).IsRequired(false);
        builder.Property(e => e.AdditionalRemarks4)     .HasColumnName("REV_REM_MRK10").HasMaxLength(4000).IsRequired(false);
        builder.Property(e => e.EntryDate)              .HasColumnName("REV_ENT_DATE").HasMaxLength(2000).IsRequired(false);
        builder.Property(e => e.Status)                 .HasColumnName("REV_STATUS").HasColumnType("char(1)").IsRequired(false);
        builder.Property(e => e.NextReviewDate)         .HasColumnName("REV_NEXT_DATE").HasColumnType("datetime2(3)").IsRequired(false);

        builder.HasMany(e => e.ReviewDetails)
               .WithOne(d => d.ReviewMain)
               .HasForeignKey(d => d.ReviewMainSerial)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.FeedbackNumber).HasDatabaseName("IDX_REVIEW_MAIN_REV_FED_NUM");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ReviewSubConfiguration : IEntityTypeConfiguration<ReviewSub>
{
    public void Configure(EntityTypeBuilder<ReviewSub> builder)
    {
        builder.ToTable("REVIEW_SUB");

        builder.HasKey(e => new { e.ReviewMainSerial, e.ReviewNumber });
        builder.Property(e => e.ReviewMainSerial)   .HasColumnName("REV_MAIN_SRL");
        builder.Property(e => e.ReviewNumber)       .HasColumnName("REV_REV_NUM");
        builder.Property(e => e.NextRequired)       .HasColumnName("REV_NEXT_STATUS").HasColumnType("char(1)").IsRequired(false);
        builder.Property(e => e.ReviewDate)         .HasColumnName("REV_DATE").HasColumnType("datetime2(3)").IsRequired(false);
        builder.Property(e => e.ReviewBy)           .HasColumnName("REV_BY");
        builder.Property(e => e.Remarks)            .HasColumnName("REV_REM_MRK").HasMaxLength(4000).IsRequired(false);
        builder.Property(e => e.ReviewStatus)       .HasColumnName("REV_STATUS").HasMaxLength(10).IsRequired(false);
        builder.Property(e => e.ProgressRemarks)    .HasColumnName("REV_PROG_REM").HasMaxLength(4000).IsRequired(false);
    }
}
