using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitmentService.Domain.Entities;
using RecruitmentService.Domain.ValueObjects;

namespace RecruitmentService.Infrastructure.Persistence.Configurations;

public class ApplicationHistoryConfiguration : IEntityTypeConfiguration<ApplicationHistory>
{
    public void Configure(EntityTypeBuilder<ApplicationHistory> builder)
    {
        builder.ToTable("APPLICATION_HISTORY");

        builder.HasKey(a => a.AppId);
        builder.Property(a => a.AppId).HasColumnName("APP_ID").HasColumnType("DECIMAL(38)");
        builder.Property(a => a.AppSl).HasColumnName("APP_SL").HasColumnType("DECIMAL(38)");
        builder.Property(a => a.AppUnit).HasColumnName("APP_UNIT").HasColumnType("CHAR(3)");
        builder.Property(a => a.AppVacancyId).HasColumnName("APP_VACANCYID").HasColumnType("DECIMAL(38)");
        builder.Property(a => a.Status).HasColumnName("APP_STATUS")
            .HasConversion(s => s.ToCode(), c => ApplicationStatusExtensions.FromCode(c))
            .HasColumnType("CHAR(2)");
        builder.Property(a => a.Remarks).HasColumnName("APP_REMARKS").HasMaxLength(4000);
        builder.Property(a => a.UpdatedBy).HasColumnName("APP_UPDATEDBY").HasColumnType("DECIMAL(22,0)");
        builder.Property(a => a.UpdatedOn).HasColumnName("APP_UPDATEDON").HasColumnType("DATETIME2(3)");

        builder.Ignore(a => a.DomainEvents);

        builder.HasMany(a => a.Qualifications)
            .WithOne()
            .HasForeignKey(q => q.AppId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Trainings)
            .WithOne()
            .HasForeignKey(t => t.AppId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ApplicationQualificationConfiguration : IEntityTypeConfiguration<ApplicationQualification>
{
    public void Configure(EntityTypeBuilder<ApplicationQualification> builder)
    {
        builder.ToTable("APPLICATION_QUALIFICATION");
        builder.HasKey(q => new { q.AppId, q.AppQualId });
        builder.Property(q => q.AppId).HasColumnName("APP_ID").HasColumnType("DECIMAL(38)");
        builder.Property(q => q.AppQualId).HasColumnName("APP_QUAL_ID").HasColumnType("DECIMAL(38)");
        builder.Property(q => q.QualCode).HasColumnName("APP_QUAL_CODE").HasColumnType("DECIMAL(38)");
        builder.Property(q => q.QualDescription).HasColumnName("APP_QUAL_DESC").HasMaxLength(65);
        builder.Property(q => q.YearFrom).HasColumnName("APP_QUAL_YEARFRO").HasColumnType("CHAR(7)");
        builder.Property(q => q.YearTo).HasColumnName("APP_QUAL_YEARTO").HasColumnType("CHAR(7)");
        builder.Property(q => q.InstitutionCode).HasColumnName("APP_QUAL_INST_CODE").HasColumnType("CHAR(3)");
        builder.Property(q => q.InstitutionDescription).HasColumnName("APP_QUAL_INST_DESC").HasMaxLength(65);
        builder.Property(q => q.EducationType).HasColumnName("APP_QUAL_EDU_TYPE").HasColumnType("CHAR(1)");
        builder.Property(q => q.SpecializationCode).HasColumnName("APP_QUAL_SPE_CODE").HasColumnType("DECIMAL(38)");
        builder.Property(q => q.SpecializationDescription).HasColumnName("APP_QUAL_SPE_DESC").HasMaxLength(65);
        builder.Property(q => q.Percentage).HasColumnName("APP_QUAL_PERCENTAGE").HasMaxLength(10);
        builder.Property(q => q.DegreeCode).HasColumnName("APP_QUAL_DEGREE_CODE").HasColumnType("DECIMAL(38)");
        builder.Property(q => q.DegreeDescription).HasColumnName("APP_QUAL_DEGREE_DESC").HasMaxLength(65);
        builder.Property(q => q.InstitutionOthers).HasColumnName("APP_QUAL_INST_OTHERS").HasMaxLength(100);
    }
}

public class ApplicationTrainingConfiguration : IEntityTypeConfiguration<ApplicationTraining>
{
    public void Configure(EntityTypeBuilder<ApplicationTraining> builder)
    {
        builder.ToTable("APPLICATION_TRAINING");
        builder.HasKey(t => new { t.AppId, t.TrainingId });
        builder.Property(t => t.AppId).HasColumnName("APP_ID").HasColumnType("DECIMAL(38)");
        builder.Property(t => t.TrainingId).HasColumnName("APP_TRAINING_ID").HasColumnType("DECIMAL(38)");
        builder.Property(t => t.Title).HasColumnName("APP_TRAINING_TITLE").HasMaxLength(2000);
        builder.Property(t => t.Duration).HasColumnName("APP_TRAINING_DURATION").HasMaxLength(2000);
        builder.Property(t => t.Institute).HasColumnName("APP_TRAINING_INSTITUTE").HasMaxLength(2000);
        builder.Property(t => t.Location).HasColumnName("APP_TRAINING_LOCATION").HasMaxLength(2000);
    }
}
