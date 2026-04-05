using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Persistence.Configurations;

public class ApprovalStepConfiguration : IEntityTypeConfiguration<ApprovalStep>
{
    public void Configure(EntityTypeBuilder<ApprovalStep> builder)
    {
        builder.ToTable("APPROVAL_STEP");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("STEP_ID")
            .UseIdentityColumn(1, 1);

        builder.Property(x => x.WorkflowId).HasColumnName("WORKFLOW_ID").IsRequired();
        builder.Property(x => x.StepLevel).HasColumnName("STEP_LEVEL").IsRequired();
        builder.Property(x => x.ApproverId).HasColumnName("APPROVER_ID").IsRequired();
        builder.Property(x => x.StepStatus).HasColumnName("STEP_STATUS").HasMaxLength(20).HasDefaultValue("PENDING");
        builder.Property(x => x.StepRemarks).HasColumnName("STEP_REMARKS").HasMaxLength(500);
        builder.Property(x => x.ActedOn).HasColumnName("ACTED_ON").HasColumnType("datetime2(3)");
        builder.Property(x => x.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("CREATED_ON").HasColumnType("datetime2(3)").IsRequired();

        builder.HasIndex(x => x.WorkflowId).HasDatabaseName("IX_APPROVAL_STEP_WORKFLOW");
        builder.HasIndex(x => x.ApproverId).HasDatabaseName("IX_APPROVAL_STEP_APPROVER");
        builder.HasIndex(x => x.StepStatus).HasDatabaseName("IX_APPROVAL_STEP_STATUS");
    }
}
