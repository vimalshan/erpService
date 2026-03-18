using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeAttendance.Domain.Entities;

namespace TimeAttendance.Infrastructure.Persistence.Configurations;

public class AbsenteeismDetailConfiguration : IEntityTypeConfiguration<AbsenteeismDetail>
{
    public void Configure(EntityTypeBuilder<AbsenteeismDetail> builder)
    {
        builder.ToTable("ABSENTEEISM_DET");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ABS_ID")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.UnitId).HasColumnName("ABS_UNITID").IsRequired();
        builder.Property(x => x.Year).HasColumnName("ABS_YEAR").IsRequired();
        builder.Property(x => x.Month).HasColumnName("ABS_MONTH").IsRequired();
        builder.Property(x => x.TotalManDays).HasColumnName("ABS_TOTMANDAYS").IsRequired();
        builder.Property(x => x.AbsentManDays).HasColumnName("ABS_ABSMANDAYS").IsRequired();
        builder.Property(x => x.GradeCategory)
            .HasColumnName("ABS_GRADECAT")
            .HasMaxLength(3)
            .IsRequired()
            .IsFixedLength();
        builder.Property(x => x.FunctionId).HasColumnName("ABS_FUNCTIONID").IsRequired();
        builder.Property(x => x.AgeId).HasColumnName("ABS_AGEID").IsRequired();
        builder.Property(x => x.ExperienceId).HasColumnName("ABS_EXPERIENCEID").IsRequired();
        builder.Property(x => x.Gender)
            .HasColumnName("ABS_GENDER")
            .HasMaxLength(1)
            .IsFixedLength()
            .IsRequired();
        builder.Property(x => x.InternalExperienceId).HasColumnName("ABS_INTEXPID").IsRequired();
        builder.Property(x => x.TotalExperienceId).HasColumnName("ABS_TOTEXPID").IsRequired();

        // Audit fields not in original schema — stored as shadow properties or ignored
        builder.Property(x => x.CreatedAt).HasColumnName("CREATED_AT").HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.CreatedBy).HasColumnName("CREATED_BY").HasMaxLength(100).HasDefaultValue(string.Empty);
        builder.Property(x => x.LastModifiedAt).HasColumnName("LAST_MODIFIED_AT").IsRequired(false);
        builder.Property(x => x.LastModifiedBy).HasColumnName("LAST_MODIFIED_BY").HasMaxLength(100).IsRequired(false);

        builder.Ignore(x => x.AbsenteeismRate);
        builder.Ignore(x => x.DomainEvents);

        builder.HasIndex(x => new { x.UnitId, x.Year, x.Month })
            .HasDatabaseName("IX_ABSENTEEISM_DET_UNIT_PERIOD");
    }
}
