using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitmentService.Domain.Entities;
using RecruitmentService.Domain.ValueObjects;

namespace RecruitmentService.Infrastructure.Persistence.Configurations;

public class VacancyConfiguration : IEntityTypeConfiguration<Vacancy>
{
    public void Configure(EntityTypeBuilder<Vacancy> builder)
    {
        builder.ToTable("VACANCY_MAIN");

        builder.HasKey(v => v.VacancyId);
        builder.Property(v => v.VacancyId).HasColumnName("VACANCY_ID").HasColumnType("DECIMAL(38)");
        builder.Property(v => v.VacancyUnit).HasColumnName("VACANCY_UNIT").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(v => v.VacancyGrade).HasColumnName("VACANCY_GRADE").HasColumnType("DECIMAL(38)").IsRequired();
        builder.Property(v => v.VacancyPositionId).HasColumnName("VACANCY_POSITIONID").HasColumnType("DECIMAL(38)").IsRequired();
        builder.Property(v => v.VacancyName).HasColumnName("VACANCY_NAME").HasMaxLength(65).IsRequired();
        builder.Property(v => v.VacancyReporting).HasColumnName("VACANCY_REPORTING").HasMaxLength(65);
        builder.Property(v => v.VacancyLocation).HasColumnName("VACANCY_LOCATION").HasColumnType("DECIMAL(38)").IsRequired();
        builder.Property(v => v.VacancyProcess).HasColumnName("VACANCY_PROCESS").HasColumnType("DECIMAL(38)").IsRequired();
        builder.Property(v => v.VacancyAge).HasColumnName("VACANCY_AGE").HasMaxLength(65).IsRequired();
        builder.Property(v => v.VacancyExperience).HasColumnName("VACANCY_EXPERIENCE").HasMaxLength(4000).IsRequired();
        builder.Property(v => v.VacancyQualification).HasColumnName("VACANCY_QUALIFICATION").HasMaxLength(4000).IsRequired();
        builder.Property(v => v.VacancyNarration1).HasColumnName("VACANCY_NARRATION1").HasMaxLength(4000);
        builder.Property(v => v.VacancyNarration2).HasColumnName("VACANCY_NARRATION2").HasMaxLength(4000);
        builder.Property(v => v.VacancyNarration3).HasColumnName("VACANCY_NARRATION3").HasMaxLength(4000);
        builder.Property(v => v.VacancyNarration4).HasColumnName("VACANCY_NARRATION4").HasMaxLength(1000);
        builder.Property(v => v.VacancyAttachment).HasColumnName("VACANCY_ATTACHMENT").HasMaxLength(65);
        builder.Property(v => v.VacancyLastDate).HasColumnName("VACANCY_LASTDATE").HasColumnType("DATETIME2(3)");
        builder.Property(v => v.AdvertiseIntranet).HasColumnName("VACANCY_ADINTRAFLAG")
            .HasConversion(v => v ? "Y" : "N", s => s == "Y").HasColumnType("CHAR(1)");
        builder.Property(v => v.IntranetFromDate).HasColumnName("VACANCY_ADINTRAFRODATE").HasColumnType("DATETIME2(3)");
        builder.Property(v => v.IntranetToDate).HasColumnName("VACANCY_ADINTRATODATE").HasColumnType("DATETIME2(3)");
        builder.Property(v => v.AdvertiseInternet).HasColumnName("VACANCY_ADINTERFLAG")
            .HasConversion(v => v ? "Y" : "N", s => s == "Y").HasColumnType("CHAR(1)");
        builder.Property(v => v.InternetFromDate).HasColumnName("VACANCY_ADINTERFRODATE").HasColumnType("DATETIME2(3)");
        builder.Property(v => v.InternetToDate).HasColumnName("VACANCY_ADINTERTODATE").HasColumnType("DATETIME2(3)");
        builder.Property(v => v.PostedBy).HasColumnName("VACANCY_POSTBY").HasColumnType("DECIMAL(38)");
        builder.Property(v => v.PostedDate).HasColumnName("VACANCY_POSTDATE").HasColumnType("DATETIME2(3)");
        builder.Property(v => v.ModifiedBy).HasColumnName("VACANCY_MODBY").HasColumnType("DECIMAL(38)");
        builder.Property(v => v.ModifiedDate).HasColumnName("VACANCY_MODDATE").HasColumnType("DATETIME2(3)");
        builder.Property(v => v.LiveStatus).HasColumnName("VACANCY_LIVESTATUS")
            .HasConversion(s => s.ToCode(), c => VacancyStatusExtensions.FromCode(c)).HasColumnType("CHAR(1)");
        builder.Property(v => v.Remarks).HasColumnName("VACANCY_REMARKS").HasMaxLength(4000);
        builder.Property(v => v.InternalReferralAllowed).HasColumnName("VACANCY_INTREFRFLAG")
            .HasConversion(v => v ? "Y" : "N", s => s == "Y").HasColumnType("CHAR(1)");
        builder.Property(v => v.InternalReferralEmail).HasColumnName("VACANCY_INTREFMAILID").HasMaxLength(65);
        builder.Property(v => v.VacancyUnitId).HasColumnName("VACANCY_UNITID").HasColumnType("DECIMAL(22,0)");
        builder.Property(v => v.VacancyType).HasColumnName("VACANCY_TYPE").HasMaxLength(3);
        builder.Property(v => v.GradeList).HasColumnName("VACANCY_GRADELIST").HasMaxLength(4000);
        builder.Property(v => v.GradeType).HasColumnName("VACANCY_GRADETYPE").HasMaxLength(5);
        builder.Property(v => v.NumberOfOpenings).HasColumnName("VACANCY_NOS").HasColumnType("DECIMAL(22,0)");
        builder.Property(v => v.CtcFrom).HasColumnName("VACANCY_CTCFROM").HasColumnType("DECIMAL(38)");
        builder.Property(v => v.CtcTo).HasColumnName("VACANCY_CTCTO").HasColumnType("DECIMAL(38)");
        builder.Property(v => v.Designation).HasColumnName("VACANCY_DESIGNATION").HasMaxLength(100);
        builder.Property(v => v.AllowDownloadForm).HasColumnName("VACANCY_DOWNLOADFORM")
            .HasConversion(v => v ? "Y" : "N", s => s == "Y").HasColumnType("CHAR(1)");
        builder.Property(v => v.ApplicationFormFileName).HasColumnName("VACANCY_APPLICATIONFORM").HasMaxLength(200);
        builder.Property(v => v.AllowUploadResume).HasColumnName("VACANCY_UPLOADRESUME")
            .HasConversion(v => v ? "Y" : "N", s => s == "Y").HasColumnType("CHAR(1)");
        builder.Property(v => v.InternalReferralCloseDate).HasColumnName("VACANCY_INTREFCLSDATE").HasColumnType("DATETIME2(3)");
        builder.Property(v => v.DisabilityFlag).HasColumnName("VACANCY_DISABILITYFLAG")
            .HasConversion(v => v ? "Y" : "N", s => s == "Y").HasColumnType("CHAR(1)");
        builder.Property(v => v.DisabilityLimit).HasColumnName("VACANCY_DISABILITYLIMIT").HasMaxLength(500);

        builder.Ignore(v => v.Applications);
        builder.Ignore(v => v.DomainEvents);
    }
}
