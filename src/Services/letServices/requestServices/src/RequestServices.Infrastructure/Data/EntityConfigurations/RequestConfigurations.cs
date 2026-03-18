using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RequestServices.Domain.Entities;

namespace RequestServices.Infrastructure.Data.EntityConfigurations;

public class RequestMainConfiguration : IEntityTypeConfiguration<RequestMain>
{
    public void Configure(EntityTypeBuilder<RequestMain> builder)
    {
        builder.ToTable("REQUEST_MAIN");

        builder.HasKey(e => e.RequestId);
        builder.Property(e => e.RequestId)    .HasColumnName("RQ_REQ_ID").ValueGeneratedNever();
        builder.Property(e => e.EmployeeUser) .HasColumnName("RQ_EMP_USR").HasMaxLength(25).IsRequired();
        builder.Property(e => e.RequestDate)  .HasColumnName("RQ_REQ_DAT").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(e => e.SupervisorUser).HasColumnName("RQ_SUP_USR").HasMaxLength(25).IsRequired();

        builder.HasMany(e => e.SubRequests)
               .WithOne(s => s.RequestMain)
               .HasForeignKey(s => s.RequestId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.EmployeeUser) .HasDatabaseName("IDX_REQUEST_MAIN_EMP_USR");
        builder.HasIndex(e => e.SupervisorUser).HasDatabaseName("IDX_REQUEST_MAIN_SUP_USR");
    }
}

public class RequestSubConfiguration : IEntityTypeConfiguration<RequestSub>
{
    public void Configure(EntityTypeBuilder<RequestSub> builder)
    {
        builder.ToTable("REQUEST_SUB");

        builder.HasKey(e => e.SerialNumber);
        builder.Property(e => e.RequestId)         .HasColumnName("RQ_REQ_ID").IsRequired();
        builder.Property(e => e.SerialNumber)      .HasColumnName("RQ_SRL_NUM").ValueGeneratedNever();
        builder.Property(e => e.RequestDate)       .HasColumnName("RQ_REQ_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.ModifiedDate)      .HasColumnName("RQ_MOD_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.ModifiedUser)      .HasColumnName("RQ_MOD_USR").HasColumnType("char(1)");
        builder.Property(e => e.RequestSource)     .HasColumnName("RQ_REQ_SRC").HasColumnType("char(1)");
        builder.Property(e => e.ModuleTraining)    .HasColumnName("RQ_MOD_TRN").HasColumnType("char(1)");
        builder.Property(e => e.GoalDesignation)   .HasColumnName("RQ_GOL_DES").HasColumnType("char(1)");
        builder.Property(e => e.StatusCode)        .HasColumnName("RQ_STS_COD").HasColumnType("char(1)");
        builder.Property(e => e.TrainingNeed)      .HasColumnName("RQ_TRN_NED").HasMaxLength(255);
        builder.Property(e => e.CancellationDate)  .HasColumnName("RQ_CAN_DAT").HasColumnType("datetime2(3)").IsRequired(false);
        builder.Property(e => e.CancellationRemark).HasColumnName("RQ_CAN_REM").HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.MentorUser)        .HasColumnName("RQ_MEN_USR").HasMaxLength(255);
        builder.Property(e => e.MentorRemark)      .HasColumnName("RQ_MEN_REM").HasMaxLength(255);
        builder.Property(e => e.CourseId)          .HasColumnName("RQ_CRS_ID");
        builder.Property(e => e.ApprovalNumber)    .HasColumnName("RQ_APP_NUM");
        builder.Property(e => e.ReviewDays)        .HasColumnName("RQ_REV_DYS");
        builder.Property(e => e.ReviewUser)        .HasColumnName("RQ_REV_USR").HasMaxLength(255);
        builder.Property(e => e.ReviewModule)      .HasColumnName("RQ_REV_MOD").HasMaxLength(255);
        builder.Property(e => e.StartDate)         .HasColumnName("RQ_STR_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.EndDate)           .HasColumnName("RQ_END_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.RefRequestId)      .HasColumnName("RQ_REF_REQ");
        builder.Property(e => e.RefSerialNumber)   .HasColumnName("RQ_REF_SRL");
        builder.Property(e => e.SupervisorUser)    .HasColumnName("RQ_SUP_USR").HasMaxLength(255);
        builder.Property(e => e.EnteredUser)       .HasColumnName("RQ_ENT_USR").HasMaxLength(255);
        builder.Property(e => e.EnteredMode)       .HasColumnName("RQ_ENT_MOD").HasColumnType("char(1)");
        builder.Property(e => e.ApprovalTime)      .HasColumnName("RQ_APP_TIM");
        builder.Property(e => e.BusinessBenefit)   .HasColumnName("RQ_BUS_BEN").HasMaxLength(255);
        builder.Property(e => e.ExpectedCompetency).HasColumnName("RQ_EXP_CNP").HasMaxLength(255);
        builder.Property(e => e.CourseDescription) .HasColumnName("RQ_CRS_DES").HasMaxLength(255);
        builder.Property(e => e.CourseAvailability).HasColumnName("RQ_CRS_AVL").HasColumnType("char(1)");

        builder.HasIndex(e => e.RequestId).HasDatabaseName("IDX_REQUEST_SUB_REQ_ID");
    }
}

public class RequestNewConfiguration : IEntityTypeConfiguration<RequestNew>
{
    public void Configure(EntityTypeBuilder<RequestNew> builder)
    {
        builder.ToTable("REQUEST_NEW");

        builder.HasKey(e => e.SerialNumber);
        builder.Property(e => e.RequestId)          .HasColumnName("RQ_REQ_ID");
        builder.Property(e => e.SerialNumber)       .HasColumnName("RQ_SRL_NUM").ValueGeneratedNever();
        builder.Property(e => e.SkillName)          .HasColumnName("RQ_SKL_NAM").HasMaxLength(10);
        builder.Property(e => e.LevelNumber)        .HasColumnName("RQ_LVL_NUM");
        builder.Property(e => e.FunctionDescription).HasColumnName("RQ_FNC_DES").HasMaxLength(10);
        builder.Property(e => e.CategoryCode)       .HasColumnName("RQ_CAT_COD").HasMaxLength(10);
        builder.Property(e => e.SkillType)          .HasColumnName("RQ_SKL_TYP").HasMaxLength(10);
        builder.Property(e => e.StatusCode)         .HasColumnName("RQ_STS_COD").HasMaxLength(10);
        builder.Property(e => e.CourseId)           .HasColumnName("RQ_CRS_ID");
    }
}

public class RequestActionConfiguration : IEntityTypeConfiguration<RequestAction>
{
    public void Configure(EntityTypeBuilder<RequestAction> builder)
    {
        builder.ToTable("REQUEST_ACTION");

        builder.HasKey(e => e.SerialNumber);
        builder.Property(e => e.RequestId)             .HasColumnName("RQ_REQ_ID").IsRequired(false);
        builder.Property(e => e.SerialNumber)          .HasColumnName("RQ_SRL_NUM").ValueGeneratedNever();
        builder.Property(e => e.ActionNumber)          .HasColumnName("RQ_ACT_NUM").IsRequired(false);
        builder.Property(e => e.KeyExperience)         .HasColumnName("RQ_KEY_EXP").HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.UsageExperience)       .HasColumnName("RQ_USG_EXP").HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.TimeExperience)        .HasColumnName("RQ_TIM_EXP").HasColumnType("decimal(38,0)").IsRequired(false);
        builder.Property(e => e.SupervisorExperience)  .HasColumnName("RQ_SUP_EXP").HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.CancellationDate)      .HasColumnName("RQ_CAN_DAT").HasColumnType("datetime2(3)").IsRequired(false);
        builder.Property(e => e.EntryDate)             .HasColumnName("RQ_ENT_DAT").HasColumnType("datetime2(3)").IsRequired(false);
        builder.Property(e => e.EntryUser)             .HasColumnName("RQ_ENT_USR").HasColumnType("char(1)").IsRequired(false);
        builder.Property(e => e.ReviewUser)            .HasColumnName("RQ_REV_USR").HasColumnType("char(1)").IsRequired(false);
        builder.Property(e => e.ReviewDate)            .HasColumnName("RQ_REV_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.ReviewNotes)           .HasColumnName("RQ_REV_NOS").IsRequired(false);
        builder.Property(e => e.CourseId)              .HasColumnName("RQ_CRS_ID").IsRequired(false);
        builder.Property(e => e.ActionFlag)            .HasColumnName("RQ_ACT_FLG").HasColumnType("char(1)").IsRequired(false);
        builder.Property(e => e.CancellationRemark)    .HasColumnName("RQ_CAN_REM").HasMaxLength(255).IsRequired(false);
    }
}

public class RequestAppConfiguration : IEntityTypeConfiguration<RequestApp>
{
    public void Configure(EntityTypeBuilder<RequestApp> builder)
    {
        builder.ToTable("REQUEST_APP");

        builder.HasKey(e => new { e.RequestId, e.SerialNumber });
        builder.Property(e => e.RequestId)     .HasColumnName("RQ_REQ_ID");
        builder.Property(e => e.SerialNumber)  .HasColumnName("RQ_SRL_NUM");
        builder.Property(e => e.ApprovalDate)  .HasColumnName("RQ_APP_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.ApprovalNumber).HasColumnName("RQ_APP_NUM");
        builder.Property(e => e.ApprovalRemark).HasColumnName("RQ_APP_REM").HasMaxLength(200);
        builder.Property(e => e.ApprovalUser)  .HasColumnName("RQ_APP_USR").HasMaxLength(20);

        builder.HasIndex(e => e.RequestId).HasDatabaseName("IDX_REQUEST_APP_REQ_ID");
    }
}
