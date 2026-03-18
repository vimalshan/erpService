using CourseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseService.Infrastructure.Data.Configurations;

public class CourseScheduleConfiguration : IEntityTypeConfiguration<CourseSchedule>
{
    public void Configure(EntityTypeBuilder<CourseSchedule> builder)
    {
        builder.ToTable("COURSE_SCHEDULE");
        builder.HasKey(s => s.ScheduleSerialNumber);
        builder.Property(s => s.ScheduleSerialNumber).HasColumnName("CS_SCH_SRL").ValueGeneratedNever();
        builder.Property(s => s.CourseId).HasColumnName("CS_CRS_ID").IsRequired();
        builder.Property(s => s.ScheduleDate).HasColumnName("CS_SCH_DAT").IsRequired();
        builder.Property(s => s.StartTime).HasColumnName("CS_STR_TIM").HasMaxLength(5).IsRequired();
        builder.Property(s => s.EndTime).HasColumnName("CS_END_TIM").HasMaxLength(5).IsRequired();
        builder.Property(s => s.LocationName).HasColumnName("CS_LOC_NAM").HasMaxLength(65).IsRequired();
        builder.Property(s => s.TrainerName).HasColumnName("CS_TRN_NAM").HasMaxLength(65).IsRequired();
        builder.Ignore(s => s.DomainEvents);
        builder.HasIndex(s => s.CourseId).HasDatabaseName("IDX_COURSE_SCHEDULE_CRS_ID");
    }
}

public class CourseParticipantConfiguration : IEntityTypeConfiguration<CourseParticipant>
{
    public void Configure(EntityTypeBuilder<CourseParticipant> builder)
    {
        builder.ToTable("COURSE_PARTICIPANT_MGT");
        // Composite key: CourseId + UserCode as the natural identity of a registration
        builder.HasKey(p => new { p.CourseId, p.UserCode });
        builder.Property(p => p.CourseId).HasColumnName("CS_CRS_ID");
        builder.Property(p => p.NominationNumber).HasColumnName("CS_NOM_NUM");
        builder.Property(p => p.UserCode).HasColumnName("CS_USR_COD").HasMaxLength(255).IsRequired();
        builder.Property(p => p.CancellationDate).HasColumnName("CS_CAN_DAT");
        builder.Property(p => p.CancellationRemark).HasColumnName("CS_CAN_REM").HasMaxLength(255);
        builder.Property(p => p.EnrollmentDate).HasColumnName("CS_ENR_DAT");
        builder.Property(p => p.ApprovalStatus).HasColumnName("CS_APPR_APPROV").HasMaxLength(1);
        builder.Property(p => p.CancelApproval).HasColumnName("CS_APR_CANCEL").HasMaxLength(1);
        builder.Property(p => p.UserPin).HasColumnName("CS_USR_PIN");
        builder.Property(p => p.ApproverCode).HasColumnName("CS_APPR_COD").HasMaxLength(255);
        builder.Property(p => p.ApproverPin).HasColumnName("CS_APPR_PIN");
        builder.Property(p => p.NominationStatus).HasColumnName("CS_NOM_STS");
        builder.Property(p => p.RequestNumber).HasColumnName("CS_REQNUM");
        builder.Property(p => p.Type).HasColumnName("CS_TYPE").HasMaxLength(1);
        builder.Property(p => p.CourseDescription).HasColumnName("CS_CRS_DESC").HasMaxLength(255);
        builder.Property(p => p.TrainingDate).HasColumnName("CS_TRAINING_DATE").HasMaxLength(255);
        builder.Property(p => p.StartDate).HasColumnName("CS_STARTDAT");
        builder.Property(p => p.EndDate).HasColumnName("CS_ENDATE");
        builder.Property(p => p.AttendanceStatus).HasColumnName("CS_ATTEN").HasMaxLength(1);
        builder.Ignore(p => p.DomainEvents);
        builder.HasIndex(p => p.CourseId).HasDatabaseName("IDX_COURSE_PARTICIPANT_CRS_ID");
        builder.HasIndex(p => p.UserCode).HasDatabaseName("IDX_COURSE_PARTICIPANT_USR_COD");
    }
}

public class CourseBandConfiguration : IEntityTypeConfiguration<CourseBand>
{
    public void Configure(EntityTypeBuilder<CourseBand> builder)
    {
        builder.ToTable("COURSE_BAND");
        builder.HasKey(b => b.CourseBandCourseId);
        builder.Property(b => b.CourseBandCourseId).HasColumnName("COURSEBAND_COURSEID");
        builder.Property(b => b.BandId).HasColumnName("COURSEBAND_ID");
        builder.Ignore(b => b.DomainEvents);
    }
}

public class CourseCostConfiguration : IEntityTypeConfiguration<CourseCost>
{
    public void Configure(EntityTypeBuilder<CourseCost> builder)
    {
        builder.ToTable("COURSE_COST");
        builder.HasKey(c => c.CourseId);
        builder.Property(c => c.CourseId).HasColumnName("CS_CRS_ID");
        builder.Property(c => c.CostCode).HasColumnName("CS_CST_COD");
        builder.Property(c => c.CostAmount).HasColumnName("CS_CST_AMT");
        builder.Property(c => c.CostType).HasColumnName("CS_CST_TYP").HasMaxLength(1);
        builder.Property(c => c.Remark).HasColumnName("CS_REM_MRK").HasMaxLength(200);
        builder.Property(c => c.UnitCode).HasColumnName("CS_UNT_COD").HasMaxLength(6);
        builder.Ignore(c => c.DomainEvents);
    }
}

public class CourseModelConfiguration : IEntityTypeConfiguration<CourseModel>
{
    public void Configure(EntityTypeBuilder<CourseModel> builder)
    {
        builder.ToTable("COURSE_MODEL");
        builder.HasKey(m => new { m.CourseId, m.SkillNumber });
        builder.Property(m => m.CourseId).HasColumnName("MD_CRS_ID");
        builder.Property(m => m.SkillNumber).HasColumnName("MD_SKL_NUM");
        builder.Property(m => m.LevelNumber).HasColumnName("MD_LVL_NUM");
        builder.Property(m => m.SkillGroup).HasColumnName("MD_SKL_GRP").HasMaxLength(3);
        builder.Ignore(m => m.DomainEvents);
    }
}
