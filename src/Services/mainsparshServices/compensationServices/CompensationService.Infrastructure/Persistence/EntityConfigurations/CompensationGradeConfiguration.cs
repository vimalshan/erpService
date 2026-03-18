using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CompensationService.Domain.Entities;
using CompensationService.Domain.ValueObjects;

namespace CompensationService.Infrastructure.Persistence.EntityConfigurations;

/// <summary>
/// Entity configuration for CompensationGrade
/// </summary>
public class CompensationGradeConfiguration : IEntityTypeConfiguration<CompensationGrade>
{
    public void Configure(EntityTypeBuilder<CompensationGrade> builder)
    {
        builder.ToTable("COMP_GRADE");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("GRADE_ID");

        // Value Objects
        builder.OwnsOne(x => x.GradeCode, nav =>
        {
            nav.Property(x => x.Value).HasColumnName("GRADE_CODE").HasMaxLength(50);
            nav.HasIndex(x => x.Value).IsUnique();
        });

        builder.OwnsOne(x => x.SalaryStructure, nav =>
        {
            nav.Property(x => x.BaseSalary).HasColumnName("BASE_SALARY").HasPrecision(19, 2);
            nav.Property(x => x.HraPercentage).HasColumnName("HRA_PERCENTAGE").HasPrecision(5, 2);
            nav.Property(x => x.DaPercentage).HasColumnName("DA_PERCENTAGE").HasPrecision(5, 2);
        });

        builder.OwnsOne(x => x.Status, nav =>
        {
            nav.Property(x => x.Value).HasColumnName("GRADE_STATUS").HasMaxLength(1);
        });

        builder.Property(x => x.GradeName)
            .HasColumnName("GRADE_NAME")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.GradeLevel)
            .HasColumnName("GRADE_LEVEL")
            .IsRequired();

        builder.Property(x => x.EffectiveFrom)
            .HasColumnName("EFFECTIVE_FROM")
            .IsRequired();

        builder.Property(x => x.EffectiveTo)
            .HasColumnName("EFFECTIVE_TO");

        builder.Property(x => x.CreatedBy)
            .HasColumnName("CREATED_BY")
            .IsRequired();

        builder.Property(x => x.CreatedOn)
            .HasColumnName("CREATED_ON")
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.Property(x => x.UpdatedBy)
            .HasColumnName("UPDATED_BY");

        builder.Property(x => x.UpdatedOn)
            .HasColumnName("UPDATED_ON");

        builder.Property(x => x.Version)
            .IsConcurrencyToken();

        // Indexes
        builder.HasIndex(x => new { x.Version }).HasDatabaseName("IX_COMP_GRADE_STATUS");
        builder.HasIndex(x => x.GradeLevel).HasDatabaseName("IX_COMP_GRADE_LEVEL");
        builder.HasIndex(x => new { x.EffectiveFrom, x.EffectiveTo }).HasDatabaseName("IX_COMP_GRADE_EFFECTIVE");
    }
}
