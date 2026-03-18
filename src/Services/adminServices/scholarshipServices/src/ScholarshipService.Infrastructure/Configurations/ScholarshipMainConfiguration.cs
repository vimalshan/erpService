using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScholarshipService.Domain.Entities;

namespace ScholarshipService.Infrastructure.Configurations;

public class ScholarshipMainConfiguration : IEntityTypeConfiguration<ScholarshipMain>
{
    public void Configure(EntityTypeBuilder<ScholarshipMain> builder)
    {
        builder.ToTable("SCHOLARSHIP_MAIN");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("SCH_ID").ValueGeneratedNever();
        builder.Property(x => x.EmployeeSysId).HasColumnName("SCH_EMPSYSID").IsRequired();
        builder.Property(x => x.GradeId).HasColumnName("SCH_GRADEID").IsRequired();
        builder.Property(x => x.DependentId).HasColumnName("SCH_DEPENDID").IsRequired();
        builder.Property(x => x.ChildName).HasColumnName("SCH_CHILDNAME").HasMaxLength(100).IsRequired();
        builder.Property(x => x.LastSchool).HasColumnName("SCH_LASTSCHOOL").HasMaxLength(100).IsRequired();
        builder.Property(x => x.LastYearOfSchool).HasColumnName("SCH_LASTYEAROFSCHOOL").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.LastExam).HasColumnName("SCH_LASTEXAM").HasMaxLength(2).IsRequired();
        builder.Property(x => x.CgpaFlag).HasColumnName("SCH_CGPAFLAG").HasMaxLength(1).IsRequired();
        builder.Property(x => x.MarksPercentage).HasColumnName("SCH_MARKSPER").HasColumnType("decimal(19,0)").IsRequired();
        builder.Property(x => x.MarksGpa).HasColumnName("SCH_MARKSGPA").HasColumnType("decimal(19,0)").IsRequired();
        builder.Property(x => x.MarksFile).HasColumnName("SCH_MARKSFILE").HasMaxLength(100).IsRequired();
        builder.Property(x => x.CourseName).HasColumnName("SCH_COURSENAME").HasMaxLength(100).IsRequired();
        builder.Property(x => x.CourseJoinYear).HasColumnName("SCH_COURSEJOINYEAR").IsRequired();
        builder.Property(x => x.CourseJoinMonth).HasColumnName("SCH_COURSEJOINMONTH").HasColumnType("decimal(20,0)").IsRequired();
        builder.Property(x => x.CourseDuration).HasColumnName("SCH_COURSEDURATION").IsRequired();
        builder.Property(x => x.AdmissionReceiptFile).HasColumnName("SCH_ADMRECPTFILE").HasMaxLength(100);
        builder.Property(x => x.PaymentMode).HasColumnName("SCH_PAYMODE").HasMaxLength(3);
        builder.Property(x => x.ChildAccountNumber).HasColumnName("SCH_CHILDACCNO").HasMaxLength(20);
        builder.Property(x => x.ChildBankIfsc).HasColumnName("SCH_CHILLDBANKIFSC").HasMaxLength(12);
        builder.Property(x => x.ChildBankMicr).HasColumnName("SCH_CHILLDBANKMICR").HasMaxLength(12);
        builder.Property(x => x.EntryStatus).HasColumnName("SCH_ENTRYSTATUS").HasMaxLength(1);
        builder.Property(x => x.Source).HasColumnName("SCH_SOURCE").HasMaxLength(1).IsRequired();
        builder.Property(x => x.DisbursementAmount).HasColumnName("SCH_DISBAMOUNT").HasColumnType("decimal(19,0)").IsRequired();
        builder.Property(x => x.DisbursementFrequency).HasColumnName("SCH_DISBFREQ").HasMaxLength(1).IsRequired();
        builder.Property(x => x.LiveStatus).HasColumnName("SCH_LIVESTATUS").HasMaxLength(1).IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("SCH_CREATEDON").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(x => x.CreatedBy).HasColumnName("SCH_CREATEDBY").IsRequired();
        builder.Property(x => x.UpdatedOn).HasColumnName("SCH_UPDATEDON").HasColumnType("datetime2(3)");
        builder.Property(x => x.UpdatedBy).HasColumnName("SCH_UPDATEDBY");
        builder.Property(x => x.ApprovalBy).HasColumnName("SCH_APPROVALBY");
        builder.Property(x => x.ApprovalOn).HasColumnName("SCH_APPROVALON").HasColumnType("datetime2(3)");
        builder.Property(x => x.ApprovalRemarks).HasColumnName("SCH_APPREMARKS").HasMaxLength(200);
        builder.Property(x => x.StopReason).HasColumnName("SCH_STOPREASON").HasMaxLength(200);
        builder.Property(x => x.StopDate).HasColumnName("SCH_STOPDATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.StopEnteredOn).HasColumnName("SCH_STOPENTEREDON").HasColumnType("datetime2(3)");
        builder.Property(x => x.StopEnteredBy).HasColumnName("SCH_STOPENTEREDBY");
        builder.Property(x => x.IsOffline).HasColumnName("SCH_OFFLINE").HasMaxLength(1).IsRequired();
        builder.Property(x => x.OfflineYear).HasColumnName("SCH_OFFLINEYEAR");

        builder.HasMany(x => x.Details)
            .WithOne()
            .HasForeignKey(d => d.MainId)
            .HasConstraintName("FK_SCHOLARSHIP_DETAIL_MAIN");

        builder.Ignore(x => x.DomainEvents);
    }
}
