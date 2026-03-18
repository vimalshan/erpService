using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScholarshipService.Domain.Entities;

namespace ScholarshipService.Infrastructure.Configurations;

public class ScholarshipAmountConfiguration : IEntityTypeConfiguration<ScholarshipAmount>
{
    public void Configure(EntityTypeBuilder<ScholarshipAmount> builder)
    {
        builder.ToTable("SCHOLARSHIP_AMOUNT");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("SCH_AMTID").ValueGeneratedNever();
        builder.Property(x => x.OrgId).HasColumnName("SCH_ORGID").IsRequired();
        builder.Property(x => x.GradeCategory).HasColumnName("SCH_GRADECAT").HasMaxLength(3).IsRequired();
        builder.Property(x => x.EligibleExam).HasColumnName("SCH_ELGIBLEEXAM").HasMaxLength(2).IsRequired();
        builder.Property(x => x.ApplicableAllGrade).HasColumnName("SCH_APPLICABLEALLGRADE").HasMaxLength(1).IsRequired();
        builder.Property(x => x.GradeId).HasColumnName("SCH_GRADEID").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.FromYear).HasColumnName("SCH_FROMYEAR").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.CloseYear).HasColumnName("SCH_CLOSEYEAR").HasColumnType("decimal(38,0)");
        builder.Property(x => x.EligibleAmount).HasColumnName("SCH_ELGIBLEAMOUNT").IsRequired();
        builder.Property(x => x.EligibleYear).HasColumnName("SCH_ELGIBLEYEAR").IsRequired();
        builder.Property(x => x.CutoffMarks).HasColumnName("SCH_CUTOFFMARKS").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("SCH_CREATEDON").HasColumnType("datetime2(3)");
        builder.Property(x => x.CreatedBy).HasColumnName("SCH_CREATEDBY");
        builder.Property(x => x.UpdatedOn).HasColumnName("SCH_UPDATEDON").HasColumnType("datetime2(3)");
        builder.Property(x => x.UpdatedBy).HasColumnName("SCH_UPDATEDBY");

        builder.HasIndex(x => new { x.GradeCategory, x.EligibleExam })
            .HasDatabaseName("IDX_SCHOLARSHIP_AMOUNT_GRADECAT");

        builder.Ignore(x => x.DomainEvents);
    }
}
