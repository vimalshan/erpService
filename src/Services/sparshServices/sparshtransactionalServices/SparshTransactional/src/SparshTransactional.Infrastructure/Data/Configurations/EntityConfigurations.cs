using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SparshTransactional.Domain.Entities;

namespace SparshTransactional.Infrastructure.Data.Configurations;

public class ScholarshipMasterConfiguration : IEntityTypeConfiguration<ScholarshipMaster>
{
    public void Configure(EntityTypeBuilder<ScholarshipMaster> builder)
    {
        builder.ToTable("SCHOLARSHIP_MASTER");
        builder.HasKey(e => e.ScholarshipId);

        builder.Property(e => e.ScholarshipId).HasColumnName("SCHOLARSHIP_ID").UseIdentityColumn();
        builder.Property(e => e.ScholarshipName).HasColumnName("SCHOLARSHIP_NAME").HasMaxLength(200).IsRequired();
        builder.Property(e => e.ScholarshipDescription).HasColumnName("SCHOLARSHIP_DESCRIPTION").HasMaxLength(500);
        builder.Property(e => e.ScholarshipType).HasColumnName("SCHOLARSHIP_TYPE").HasColumnType("char(1)");
        builder.Property(e => e.CoveragePercent).HasColumnName("SCHOLARSHIP_COVERAGE_PERCENT").HasColumnType("decimal(5,2)");
        builder.Property(e => e.MaxAmount).HasColumnName("SCHOLARSHIP_MAX_AMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.Status).HasColumnName("SCHOLARSHIP_STATUS").HasColumnType("char(1)").IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON");
        builder.Property(e => e.UpdatedBy).HasColumnName("UPDATED_BY");
        builder.Property(e => e.UpdatedOn).HasColumnName("UPDATED_ON");

        builder.HasIndex(e => e.Status).HasDatabaseName("IX_SCHOLARSHIP_MASTER_STATUS");

        builder.HasMany(e => e.EligibilityCriteria).WithOne(c => c.Scholarship)
            .HasForeignKey(c => c.ScholarshipId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(e => e.Applications).WithOne(a => a.Scholarship)
            .HasForeignKey(a => a.ScholarshipId).OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class EligibilityCriteriaConfiguration : IEntityTypeConfiguration<EligibilityCriteria>
{
    public void Configure(EntityTypeBuilder<EligibilityCriteria> builder)
    {
        builder.ToTable("SCHOLARSHIP_ELIGIBILITY_CRITERIA");
        builder.HasKey(e => e.CriteriaId);

        builder.Property(e => e.CriteriaId).HasColumnName("CRITERIA_ID").UseIdentityColumn();
        builder.Property(e => e.ScholarshipId).HasColumnName("SCHOLARSHIP_ID");
        builder.Property(e => e.CriteriaName).HasColumnName("CRITERIA_NAME").HasMaxLength(200).IsRequired();
        builder.Property(e => e.CriteriaDescription).HasColumnName("CRITERIA_DESCRIPTION").HasMaxLength(500);
        builder.Property(e => e.MinScore).HasColumnName("MIN_SCORE").HasColumnType("decimal(5,2)");
        builder.Property(e => e.MaxFamilyIncome).HasColumnName("MAX_FAMILY_INCOME").HasColumnType("decimal(19,0)");
        builder.Property(e => e.EligibilityStatus).HasColumnName("ELIGIBILITY_STATUS").HasColumnType("char(1)").IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON");

        builder.HasIndex(e => e.ScholarshipId).HasDatabaseName("IX_ELIGIBILITY_SCHOLARSHIP");

        builder.Ignore(e => e.DomainEvents);
    }
}

public class ScholarshipApplicationConfiguration : IEntityTypeConfiguration<ScholarshipApplication>
{
    public void Configure(EntityTypeBuilder<ScholarshipApplication> builder)
    {
        builder.ToTable("SCHOLARSHIP_APPLICATION");
        builder.HasKey(e => e.ApplicationId);

        builder.Property(e => e.ApplicationId).HasColumnName("APPLICATION_ID").UseIdentityColumn();
        builder.Property(e => e.StudentId).HasColumnName("EMP_STUDENT_ID");
        builder.Property(e => e.ScholarshipId).HasColumnName("SCHOLARSHIP_ID");
        builder.Property(e => e.ApplicationDate).HasColumnName("APPLICATION_DATE");
        builder.Property(e => e.FamilyIncome).HasColumnName("FAMILY_INCOME").HasColumnType("decimal(19,0)");
        builder.Property(e => e.ApplicationStatus).HasColumnName("APPLICATION_STATUS").HasColumnType("char(1)").IsRequired();
        builder.Property(e => e.ApprovedAmount).HasColumnName("APPROVED_AMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.ApprovedBy).HasColumnName("APPROVED_BY");
        builder.Property(e => e.RejectionReason).HasColumnName("REJECTION_REASON").HasMaxLength(500);
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON");
        builder.Property(e => e.UpdatedOn).HasColumnName("UPDATED_ON");

        builder.HasIndex(e => e.ApplicationStatus).HasDatabaseName("IX_APPLICATION_STATUS");
        builder.HasIndex(e => e.StudentId).HasDatabaseName("IX_APPLICATION_STUDENT");
        builder.HasIndex(e => e.ScholarshipId).HasDatabaseName("IX_APPLICATION_SCHOLARSHIP");

        builder.HasMany(e => e.Disbursements).WithOne(d => d.Application)
            .HasForeignKey(d => d.ApplicationId).OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class ScholarshipDisbursementConfiguration : IEntityTypeConfiguration<ScholarshipDisbursement>
{
    public void Configure(EntityTypeBuilder<ScholarshipDisbursement> builder)
    {
        builder.ToTable("SCHOLARSHIP_DISBURSEMENT");
        builder.HasKey(e => e.DisbursementId);

        builder.Property(e => e.DisbursementId).HasColumnName("DISBURSEMENT_ID").UseIdentityColumn();
        builder.Property(e => e.ApplicationId).HasColumnName("APPLICATION_ID");
        builder.Property(e => e.StudentId).HasColumnName("STUDENT_ID");
        builder.Property(e => e.ScholarshipId).HasColumnName("SCHOLARSHIP_ID");
        builder.Property(e => e.DisbursementAmount).HasColumnName("DISBURSEMENT_AMOUNT").HasColumnType("decimal(19,0)").IsRequired();
        builder.Property(e => e.DisbursementDate).HasColumnName("DISBURSEMENT_DATE");
        builder.Property(e => e.DisbursementStatus).HasColumnName("DISBURSEMENT_STATUS").HasColumnType("char(1)").IsRequired();
        builder.Property(e => e.PaymentReference).HasColumnName("PAYMENT_REFERENCE").HasMaxLength(100);
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON");
        builder.Property(e => e.UpdatedOn).HasColumnName("UPDATED_ON");

        builder.HasIndex(e => e.DisbursementStatus).HasDatabaseName("IX_DISBURSEMENT_STATUS");
        builder.HasIndex(e => e.ApplicationId).HasDatabaseName("IX_DISBURSEMENT_APPLICATION");

        builder.Ignore(e => e.DomainEvents);
    }
}
