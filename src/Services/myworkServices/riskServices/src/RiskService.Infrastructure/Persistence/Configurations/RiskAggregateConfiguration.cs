using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RiskService.Domain.Aggregates;

namespace RiskService.Infrastructure.Persistence.Configurations;

public class RiskAggregateConfiguration : IEntityTypeConfiguration<RiskAggregate>
{
    public void Configure(EntityTypeBuilder<RiskAggregate> builder)
    {
        builder.ToTable("RISK_MASTER");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("RISK_ID");
        builder.Property(r => r.ApplicableTo).HasColumnName("RISK_APPLICABLETO").HasColumnType("char(1)");
        builder.Property(r => r.OrganizationId).HasColumnName("RISK_ORGID");
        builder.Property(r => r.BusinessId).HasColumnName("RISK_BUSID");
        builder.Property(r => r.DivisionId).HasColumnName("RISK_DIVISIONID");
        builder.Property(r => r.UnitId).HasColumnName("RISK_UNITID");
        builder.Property(r => r.FunctionId).HasColumnName("RISK_FUNCTIONID");
        builder.Property(r => r.EventTitle).HasColumnName("RISK_EVENTTITLE").HasMaxLength(500);
        builder.Property(r => r.Description).HasColumnName("RISK_DESC").HasMaxLength(4000);
        builder.Property(r => r.TypeId).HasColumnName("RISK_TYPEID");
        builder.Property(r => r.ImpactId).HasColumnName("RISK_IMPACTID");
        builder.Property(r => r.ProbabilityId).HasColumnName("RISK_PROBID");
        builder.Property(r => r.RatingId).HasColumnName("RISK_RATEID");
        builder.Property(r => r.ResidualImpactId).HasColumnName("RISK_RESIMPACTID");
        builder.Property(r => r.ResidualProbabilityId).HasColumnName("RISK_RESPROBID");
        builder.Property(r => r.ResidualRatingId).HasColumnName("RISK_RESRATEID");
        builder.Property(r => r.ResponseId).HasColumnName("RISK_RESPID");
        builder.Property(r => r.MitigationFlag).HasColumnName("RISK_MITFLAG").HasColumnType("char(1)");
        builder.Property(r => r.OwnerId).HasColumnName("RISK_OWNER");
        builder.Property(r => r.ApprovalStatus).HasColumnName("RISK_APPSTATUS").HasColumnType("char(1)");
        builder.Property(r => r.CancelDate).HasColumnName("RISK_CANCELDATE");
        builder.Property(r => r.CancelReason).HasColumnName("RISK_CANCELREASON").HasMaxLength(500);
        builder.Property(r => r.CreatedBy).HasColumnName("RISK_CREATEDBY");
        builder.Property(r => r.CreatedOn).HasColumnName("RISK_CREATEDON");
        builder.Property(r => r.ModifiedBy).HasColumnName("RISK_MODIFIEDBY");
        builder.Property(r => r.ModifiedOn).HasColumnName("RISK_MODIFIEDON");
        builder.Property(r => r.AssessmentId).HasColumnName("RISK_ASSESSMENTID");
        builder.Property(r => r.ReviewedImpactId).HasColumnName("RISK_REVIMPACTID");
        builder.Property(r => r.ReviewedProbabilityId).HasColumnName("RISK_REVPROBID");
        builder.Property(r => r.ReviewedRiskRatingId).HasColumnName("RISK_REVRISKRATID");

        builder.HasOne(r => r.Type).WithMany().HasForeignKey(r => r.TypeId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(r => r.Impact).WithMany().HasForeignKey(r => r.ImpactId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(r => r.Probability).WithMany().HasForeignKey(r => r.ProbabilityId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(r => r.Rating).WithMany().HasForeignKey(r => r.RatingId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(r => r.Response).WithMany().HasForeignKey(r => r.ResponseId).OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(r => r.Causes).WithOne().HasForeignKey(c => c.RiskId);
        builder.HasMany(r => r.Controls).WithOne().HasForeignKey(c => c.RiskId);
        builder.HasMany(r => r.ImpactMaps).WithOne().HasForeignKey(i => i.RiskId);
        builder.HasMany(r => r.Events).WithOne().HasForeignKey(e => e.RiskId);
        builder.HasMany(r => r.Monitors).WithOne().HasForeignKey(m => m.RiskId);
        builder.HasMany(r => r.FunctionDetails).WithOne().HasForeignKey(f => f.RiskId);
        builder.HasMany(r => r.UnitDetails).WithOne().HasForeignKey(u => u.RiskId);
        builder.HasMany(r => r.Approvals).WithOne().HasForeignKey(a => a.RiskId);
        builder.HasMany(r => r.Mitigations).WithOne().HasForeignKey(m => m.RiskId);

        builder.Ignore(r => r.DomainEvents);
    }
}
