using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Persistence.Configurations;

public class ApprovalWorkflowConfiguration : IEntityTypeConfiguration<ApprovalWorkflow>
{
    public void Configure(EntityTypeBuilder<ApprovalWorkflow> builder)
    {
        builder.ToTable("APPROVAL_WORKFLOW");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("WORKFLOW_ID")
            .UseIdentityColumn(1, 1);

        builder.Property(x => x.WorkflowCode).HasColumnName("WORKFLOW_CODE").HasMaxLength(100).IsRequired();
        builder.Property(x => x.EntityType).HasColumnName("ENTITY_TYPE").HasMaxLength(50).IsRequired();
        builder.Property(x => x.EntityId).HasColumnName("ENTITY_ID").IsRequired();
        builder.Property(x => x.EmployeeId).HasColumnName("EMPLOYEE_ID").IsRequired();
        builder.Property(x => x.WorkflowStatus).HasColumnName("WORKFLOW_STATUS").HasMaxLength(20).HasDefaultValue("SUBMITTED");
        builder.Property(x => x.CurrentApprovalLevel).HasColumnName("CURRENT_APPROVAL_LEVEL").HasDefaultValue(1);
        builder.Property(x => x.CurrentApproverId).HasColumnName("CURRENT_APPROVER_ID").IsRequired();
        builder.Property(x => x.MaxApprovalLevels).HasColumnName("MAX_APPROVAL_LEVELS").HasDefaultValue(1);
        builder.Property(x => x.Remarks).HasColumnName("REMARKS").HasMaxLength(500);
        builder.Property(x => x.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("CREATED_ON").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(x => x.UpdatedBy).HasColumnName("UPDATED_BY");
        builder.Property(x => x.UpdatedOn).HasColumnName("UPDATED_ON").HasColumnType("datetime2(3)");

        builder.HasIndex(x => x.WorkflowCode).IsUnique().HasDatabaseName("UC_APPROVAL_WORKFLOW_CODE");
        builder.HasIndex(x => x.EntityType).HasDatabaseName("IX_APPROVAL_WORKFLOW_ENTITY_TYPE");
        builder.HasIndex(x => new { x.EntityType, x.EntityId }).HasDatabaseName("IX_APPROVAL_WORKFLOW_ENTITY");
        builder.HasIndex(x => x.EmployeeId).HasDatabaseName("IX_APPROVAL_WORKFLOW_EMPLOYEE");
        builder.HasIndex(x => x.WorkflowStatus).HasDatabaseName("IX_APPROVAL_WORKFLOW_STATUS");
        builder.HasIndex(x => x.CurrentApproverId).HasDatabaseName("IX_APPROVAL_WORKFLOW_APPROVER");

        builder.HasMany(x => x.Steps)
            .WithOne(x => x.Workflow)
            .HasForeignKey(x => x.WorkflowId)
            .HasConstraintName("FK_APPROVAL_STEP_WORKFLOW");
    }
}
