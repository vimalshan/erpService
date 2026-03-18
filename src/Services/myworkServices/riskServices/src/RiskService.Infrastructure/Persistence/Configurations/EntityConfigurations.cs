using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RiskService.Domain.Entities;
using RiskService.Domain.Aggregates;

namespace RiskService.Infrastructure.Persistence.Configurations;

public class RiskTypeConfiguration : IEntityTypeConfiguration<RiskType>
{
    public void Configure(EntityTypeBuilder<RiskType> builder)
    {
        builder.ToTable("RISKTYPE_MASTER");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("TYPE_ID");
        builder.Property(e => e.Name).HasColumnName("TYPE_NAME").HasMaxLength(200);
        builder.Property(e => e.CreatedBy).HasColumnName("TYPE_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("TYPE_CREATEDON");
        builder.Property(e => e.ModifiedBy).HasColumnName("TYPE_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("TYPE_MODIFIEDON");
    }
}

public class RiskImpactConfiguration : IEntityTypeConfiguration<RiskImpact>
{
    public void Configure(EntityTypeBuilder<RiskImpact> builder)
    {
        builder.ToTable("RISKIMPACT_MASTER");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("IMPACT_ID");
        builder.Property(e => e.Rank).HasColumnName("IMPACT_RANK");
        builder.Property(e => e.Name).HasColumnName("IMPACT_NAME").HasMaxLength(200);
        builder.Property(e => e.CreatedBy).HasColumnName("IMPACT_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("IMPACT_CREATEDON");
        builder.Property(e => e.ModifiedBy).HasColumnName("IMPACT_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("IMPACT_MODIFIEDON");
    }
}

public class RiskProbabilityConfiguration : IEntityTypeConfiguration<RiskProbability>
{
    public void Configure(EntityTypeBuilder<RiskProbability> builder)
    {
        builder.ToTable("RISKPROB_MASTER");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("PROB_ID");
        builder.Property(e => e.Rank).HasColumnName("PROB_RANK");
        builder.Property(e => e.Name).HasColumnName("PROB_NAME").HasMaxLength(200);
        builder.Property(e => e.Occurrence).HasColumnName("PROB_OCC").HasMaxLength(200);
        builder.Property(e => e.CreatedBy).HasColumnName("PROB_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("PROB_CREATEDON");
        builder.Property(e => e.ModifiedBy).HasColumnName("PROB_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("PROB_MODIFIEDON");
    }
}

public class RiskRatingConfiguration : IEntityTypeConfiguration<RiskRating>
{
    public void Configure(EntityTypeBuilder<RiskRating> builder)
    {
        builder.ToTable("RISKRATING_MASTER");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("RATING_ID");
        builder.Property(e => e.Rank).HasColumnName("RATING_RANK");
        builder.Property(e => e.RatingFrom).HasColumnName("RATING_FROM");
        builder.Property(e => e.RatingTo).HasColumnName("RATING_TO");
        builder.Property(e => e.Name).HasColumnName("RATING_NAME").HasMaxLength(200);
        builder.Property(e => e.CreatedBy).HasColumnName("RATING_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("RATING_CREATEDON");
        builder.Property(e => e.ModifiedBy).HasColumnName("RATING_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("RATING_MODIFIEDON");
    }
}

public class RiskResponseConfiguration : IEntityTypeConfiguration<RiskResponse>
{
    public void Configure(EntityTypeBuilder<RiskResponse> builder)
    {
        builder.ToTable("RISKRESP_MASTER");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("RESP_ID");
        builder.Property(e => e.Name).HasColumnName("RESP_NAME").HasMaxLength(200);
        builder.Property(e => e.CreatedBy).HasColumnName("RESP_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("RESP_CREATEDON");
        builder.Property(e => e.ModifiedBy).HasColumnName("RESP_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("RESP_MODIFIEDON");
    }
}

public class RiskDivisionConfiguration : IEntityTypeConfiguration<RiskDivision>
{
    public void Configure(EntityTypeBuilder<RiskDivision> builder)
    {
        builder.ToTable("RISKDIVISION_MASTER");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("RISKDIVISION_ID");
        builder.Property(e => e.Name).HasColumnName("RISKDIVISION_NAME").HasMaxLength(200);
        builder.Property(e => e.HrmsBusinessId).HasColumnName("RISKDIVISION_HRMSBUSID");
        builder.Property(e => e.CreatedBy).HasColumnName("RISKDIVISION_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("RISKDIVISION_CREATEDON");
        builder.Property(e => e.ModifiedBy).HasColumnName("RISKDIVISION_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("RISKDIVISION_MODIFIEDON");
        builder.HasMany(d => d.Units).WithOne(u => u.Division).HasForeignKey(u => u.DivisionId);
        builder.HasMany(d => d.FunctionMaps).WithOne(f => f.Division).HasForeignKey(f => f.DivisionId);
    }
}

public class RiskDivisionUnitConfiguration : IEntityTypeConfiguration<RiskDivisionUnit>
{
    public void Configure(EntityTypeBuilder<RiskDivisionUnit> builder)
    {
        builder.ToTable("RISKDIVISIONUNIT_MAP");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("DIVUNIT_MAPID");
        builder.Property(e => e.DivisionId).HasColumnName("DIVUNIT_DIVISIONID");
        builder.Property(e => e.UnitId).HasColumnName("DIVUNIT_UNITID");
        builder.Property(e => e.CreatedBy).HasColumnName("DIVUNIT_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("DIVUNIT_CREATEDON");
        builder.Property(e => e.ModifiedBy).HasColumnName("DIVUNIT_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("DIVUNIT_MODIFIEDON");
    }
}

public class RiskFunctionConfiguration : IEntityTypeConfiguration<RiskFunction>
{
    public void Configure(EntityTypeBuilder<RiskFunction> builder)
    {
        builder.ToTable("RISK_FUNCTIONMAST");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("FUNCTION_ID");
        builder.Property(e => e.Name).HasColumnName("FUNCTION_NAME").HasMaxLength(200);
        builder.Property(e => e.CreatedBy).HasColumnName("FUNCTION_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("FUNCTION_CREATEDON");
        builder.Property(e => e.ModifiedBy).HasColumnName("FUNCTION_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("FUNCTION_MODIFIEDON");
    }
}

public class RiskDivisionFunctionMapConfiguration : IEntityTypeConfiguration<RiskDivisionFunctionMap>
{
    public void Configure(EntityTypeBuilder<RiskDivisionFunctionMap> builder)
    {
        builder.ToTable("RISK_DIVISIONFUNCTIONMAP");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("DFM_MAPID");
        builder.Property(e => e.DivisionId).HasColumnName("DFM_DIVISIONID");
        builder.Property(e => e.FunctionId).HasColumnName("DFM_FUNCTIONID");
        builder.Property(e => e.CreatedBy).HasColumnName("DFM_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("DFM_CREATEDON");
        builder.Property(e => e.ModifiedBy).HasColumnName("DFM_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("DFM_MODIFIEDON");
    }
}

public class RiskCauseConfiguration : IEntityTypeConfiguration<RiskCause>
{
    public void Configure(EntityTypeBuilder<RiskCause> builder)
    {
        builder.ToTable("RISK_CAUSES");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("ROOT_ID");
        builder.Property(e => e.RiskId).HasColumnName("ROOT_RISKID");
        builder.Property(e => e.Description).HasColumnName("ROOT_DESC").HasMaxLength(2000);
        builder.Property(e => e.LastModifiedBy).HasColumnName("ROOT_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("ROOT_LASTMODIFIEDON");
        builder.Ignore(e => e.CreatedBy);
        builder.Ignore(e => e.CreatedOn);
        builder.Ignore(e => e.ModifiedBy);
        builder.Ignore(e => e.ModifiedOn);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class RiskControlConfiguration : IEntityTypeConfiguration<RiskControl>
{
    public void Configure(EntityTypeBuilder<RiskControl> builder)
    {
        builder.ToTable("RISK_CONTROLS");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("CONTROL_ID");
        builder.Property(e => e.RiskId).HasColumnName("CONTROL_RISKID");
        builder.Property(e => e.Description).HasColumnName("CONTROL_DESC").HasMaxLength(2000);
        builder.Property(e => e.FileName).HasColumnName("CONTROL_FILENAME").HasMaxLength(500);
        builder.Property(e => e.LastModifiedBy).HasColumnName("CONTROL_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("CONTROL_LASTMODIFIEDON");
        builder.Property(e => e.ImpactReductionPercent).HasColumnName("CONTROL_IMPACTREDPER");
        builder.Property(e => e.ProbabilityReductionPercent).HasColumnName("CONTROL_PROBREDPER");
        builder.Ignore(e => e.CreatedBy);
        builder.Ignore(e => e.CreatedOn);
        builder.Ignore(e => e.ModifiedBy);
        builder.Ignore(e => e.ModifiedOn);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class RiskImpactMapConfiguration : IEntityTypeConfiguration<RiskImpactMap>
{
    public void Configure(EntityTypeBuilder<RiskImpactMap> builder)
    {
        builder.ToTable("RISK_IMPACT");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("IMPMAP_ID");
        builder.Property(e => e.RiskId).HasColumnName("IMPMAP_RISKID");
        builder.Property(e => e.Description).HasColumnName("IMPMAP_DESC").HasMaxLength(2000);
        builder.Property(e => e.LastModifiedBy).HasColumnName("IMPMAP_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("IMPMAP_LASTMODIFIEDON");
        builder.Ignore(e => e.CreatedBy);
        builder.Ignore(e => e.CreatedOn);
        builder.Ignore(e => e.ModifiedBy);
        builder.Ignore(e => e.ModifiedOn);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class RiskEventConfiguration : IEntityTypeConfiguration<RiskEvent>
{
    public void Configure(EntityTypeBuilder<RiskEvent> builder)
    {
        builder.ToTable("RISK_EVENT");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("EVENT_ID");
        builder.Property(e => e.RiskId).HasColumnName("EVENT_RISKID");
        builder.Property(e => e.Description).HasColumnName("EVENT_DESCRIPTION").HasMaxLength(500);
        builder.Property(e => e.EventDate).HasColumnName("EVENT_DATE");
        builder.Property(e => e.LastModifiedBy).HasColumnName("EVENT_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("EVENT_LASTMODIFIEDON");
        builder.Ignore(e => e.CreatedBy);
        builder.Ignore(e => e.CreatedOn);
        builder.Ignore(e => e.ModifiedBy);
        builder.Ignore(e => e.ModifiedOn);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class RiskMonitorConfiguration : IEntityTypeConfiguration<RiskMonitor>
{
    public void Configure(EntityTypeBuilder<RiskMonitor> builder)
    {
        builder.ToTable("RISK_MONITOR");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("RISKMON_ID");
        builder.Property(e => e.RiskId).HasColumnName("RISKMON_RISKID");
        builder.Property(e => e.MonitoredBy).HasColumnName("RISKMON_BY").HasColumnType("char(3)");
        builder.Property(e => e.ReviewFrequency).HasColumnName("RISKMON_REVFREQUENCY").HasColumnType("char(1)");
        builder.Property(e => e.LastModifiedBy).HasColumnName("RISKMON_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("RISKMON_LASTMODIFIEDON");
        builder.Ignore(e => e.CreatedBy);
        builder.Ignore(e => e.CreatedOn);
        builder.Ignore(e => e.ModifiedBy);
        builder.Ignore(e => e.ModifiedOn);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class RiskFrequencyMapConfiguration : IEntityTypeConfiguration<RiskFrequencyMap>
{
    public void Configure(EntityTypeBuilder<RiskFrequencyMap> builder)
    {
        builder.ToTable("RISK_FREQUENCYMAP");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("FREQ_ID");
        builder.Property(e => e.RatingId).HasColumnName("FREQ_RATINGID");
        builder.Property(e => e.MonitorCode).HasColumnName("FREQ_MONITORCODE").HasColumnType("char(3)");
        builder.Property(e => e.FrequencyCode).HasColumnName("FREQ_CODE").HasColumnType("char(1)");
        builder.Property(e => e.ReviewMonth).HasColumnName("FREQ_MONTH").HasMaxLength(24);
        builder.Property(e => e.ReviewDay).HasColumnName("FREQ_DAY");
        builder.Property(e => e.CreatedBy).HasColumnName("FREQ_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("FREQ_CREATEDON");
        builder.Property(e => e.ModifiedBy).HasColumnName("FREQ_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("FREQ_MODIFIEDON");
    }
}

public class RiskUnitChampionConfiguration : IEntityTypeConfiguration<RiskUnitChampion>
{
    public void Configure(EntityTypeBuilder<RiskUnitChampion> builder)
    {
        builder.ToTable("RISKUNIT_CHAMPMAP");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("CHAMP_ID");
        builder.Property(e => e.EmployeeSysId).HasColumnName("CHAMP_EMPSYSID");
        builder.Property(e => e.ChampionType).HasColumnName("CHAMP_TYPE").HasColumnType("char(1)");
        builder.Property(e => e.OrganizationId).HasColumnName("CHAMP_ORGID");
        builder.Property(e => e.BusinessId).HasColumnName("CHAMP_BUSID");
        builder.Property(e => e.DivisionId).HasColumnName("CHAMP_DIVISIONID");
        builder.Property(e => e.UnitId).HasColumnName("CHAMP_UNITID");
        builder.Property(e => e.CreatedBy).HasColumnName("CHAMP_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("CHAMP_CREATEDON");
        builder.Property(e => e.ModifiedBy).HasColumnName("CHAMP_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("CHAMP_MODIFIEDON");
    }
}

public class RiskFunctionDetailConfiguration : IEntityTypeConfiguration<RiskFunctionDetail>
{
    public void Configure(EntityTypeBuilder<RiskFunctionDetail> builder)
    {
        builder.ToTable("RISK_FUNCTIONDET");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("FUNDET_ID");
        builder.Property(e => e.RiskId).HasColumnName("FUNDET_RiskID");
        builder.Property(e => e.FunctionId).HasColumnName("FUNDET_FUNCTIONID");
        builder.Property(e => e.LastModifiedBy).HasColumnName("FUNDET_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("FUNDET_LASTMODIFIEDON");
        builder.HasOne(e => e.Function).WithMany().HasForeignKey(e => e.FunctionId).OnDelete(DeleteBehavior.NoAction);
        builder.Ignore(e => e.CreatedBy);
        builder.Ignore(e => e.CreatedOn);
        builder.Ignore(e => e.ModifiedBy);
        builder.Ignore(e => e.ModifiedOn);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class RiskUnitDetailConfiguration : IEntityTypeConfiguration<RiskUnitDetail>
{
    public void Configure(EntityTypeBuilder<RiskUnitDetail> builder)
    {
        builder.ToTable("RISK_UNITDET");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("HRUDET_ID");
        builder.Property(e => e.RiskId).HasColumnName("HRUDET_RISKID");
        builder.Property(e => e.RiskUnitId).HasColumnName("HRUDET_RISKUNITID");
        builder.Property(e => e.LastModifiedBy).HasColumnName("HRUDET_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("HRUDET_LASTMODIFIEDON");
        builder.Ignore(e => e.CreatedBy);
        builder.Ignore(e => e.CreatedOn);
        builder.Ignore(e => e.ModifiedBy);
        builder.Ignore(e => e.ModifiedOn);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class RiskApprovalConfiguration : IEntityTypeConfiguration<RiskApproval>
{
    public void Configure(EntityTypeBuilder<RiskApproval> builder)
    {
        builder.ToTable("RISK_APPDET");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("APP_ID");
        builder.Property(e => e.RiskId).HasColumnName("APP_RISKID");
        builder.Property(e => e.ApproverEmployeeSysId).HasColumnName("APP_EMPSYSID");
        builder.Property(e => e.Status).HasColumnName("APP_STATUS").HasColumnType("char(1)");
        builder.Property(e => e.Remarks).HasColumnName("APP_REMARKS").HasMaxLength(500);
        builder.Property(e => e.LastModifiedBy).HasColumnName("APP_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("APP_LASTMODIFIEDON");
        builder.Property(e => e.ApprovalType).HasColumnName("APP_TYPE").HasColumnType("char(1)");
        builder.Ignore(e => e.CreatedBy);
        builder.Ignore(e => e.CreatedOn);
        builder.Ignore(e => e.ModifiedBy);
        builder.Ignore(e => e.ModifiedOn);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class MitigationConfiguration : IEntityTypeConfiguration<RiskMitigation>
{
    public void Configure(EntityTypeBuilder<RiskMitigation> builder)
    {
        builder.ToTable("RISK_MITIGATION");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("MIT_ID");
        builder.Property(e => e.RiskId).HasColumnName("MIT_RISKID");
        builder.Property(e => e.Action).HasColumnName("MIT_ACTION").HasMaxLength(2000);
        builder.Property(e => e.OriginalDueDate).HasColumnName("MIT_ORGDATE");
        builder.Property(e => e.DueDate).HasColumnName("MIT_DUEDATE");
        builder.Property(e => e.OwnerId).HasColumnName("MIT_OWNER");
        builder.Property(e => e.ReviewerId).HasColumnName("MIT_REVIEWER");
        builder.Property(e => e.Status).HasColumnName("MIT_STATUS").HasColumnType("char(1)");
        builder.Property(e => e.ProbabilityReduction).HasColumnName("MIT_PROBRED").HasPrecision(18, 2);
        builder.Property(e => e.ImpactReduction).HasColumnName("MIT_IMPACTRED").HasPrecision(18, 2);
        builder.Property(e => e.ApproverEmployeeSysId).HasColumnName("MIT_APPEMPSYSID");
        builder.Property(e => e.Attachment).HasColumnName("MIT_ATTACHMENT").HasMaxLength(2000);
        builder.Property(e => e.CreatedBy).HasColumnName("MIT_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("MIT_CREATEDON");
        builder.Property(e => e.ModifiedBy).HasColumnName("MIT_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("MIT_MODIFIEDON");
        builder.HasMany(m => m.Actions).WithOne().HasForeignKey(a => a.MitigationId);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class MitigationActionConfiguration : IEntityTypeConfiguration<RiskMitigationAction>
{
    public void Configure(EntityTypeBuilder<RiskMitigationAction> builder)
    {
        builder.ToTable("RISK_MITIGATIONACTION");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("ACTION_ID");
        builder.Property(e => e.MitigationId).HasColumnName("ACTION_MITID");
        builder.Property(e => e.DueDate).HasColumnName("ACTION_DUEDATE");
        builder.Property(e => e.Status).HasColumnName("ACTION_STATUS").HasColumnType("char(1)");
        builder.Property(e => e.RevisedDueDate).HasColumnName("ACTION_REVDUEDATE");
        builder.Property(e => e.ApprovalStatus).HasColumnName("ACTION_APPSTATUS").HasColumnType("char(1)");
        builder.Property(e => e.Comments).HasColumnName("ACTION_COMMENTS").HasMaxLength(500);
        builder.Property(e => e.CompletionDate).HasColumnName("ACTION_COMPLETIONDATE");
        builder.Property(e => e.CreatedBy).HasColumnName("ACTION_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("ACTION_CREATEDON");
        builder.Property(e => e.ModifiedBy).HasColumnName("ACTION_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("ACTION_MODIFIEDON");
        builder.HasMany(a => a.Approvals).WithOne().HasForeignKey(ap => ap.ActionId);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class MitigationApprovalConfiguration : IEntityTypeConfiguration<RiskMitigationApproval>
{
    public void Configure(EntityTypeBuilder<RiskMitigationApproval> builder)
    {
        builder.ToTable("RISK_MITAPPDET");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("APP_ID");
        builder.Property(e => e.ActionId).HasColumnName("APP_ACTIONID");
        builder.Property(e => e.ApproverEmployeeSysId).HasColumnName("APP_EMPSYSID");
        builder.Property(e => e.Status).HasColumnName("APP_STATUS").HasColumnType("char(1)");
        builder.Property(e => e.Remarks).HasColumnName("APP_REMARKS").HasMaxLength(50);
        builder.Property(e => e.LastModifiedBy).HasColumnName("APP_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("APP_LASTMODIFIEDON");
        builder.Ignore(e => e.CreatedBy);
        builder.Ignore(e => e.CreatedOn);
        builder.Ignore(e => e.ModifiedBy);
        builder.Ignore(e => e.ModifiedOn);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class SelfAssessmentConfiguration : IEntityTypeConfiguration<RiskSelfAssessment>
{
    public void Configure(EntityTypeBuilder<RiskSelfAssessment> builder)
    {
        builder.ToTable("RISK_SELFASSDET");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("ASS_ID");
        builder.Property(e => e.AssessmentType).HasColumnName("ASS_TYPE").HasColumnType("char(1)");
        builder.Property(e => e.TypeReferenceId).HasColumnName("ASS_TYPEREFID");
        builder.Property(e => e.MonitoredBy).HasColumnName("ASS_MONBY").HasColumnType("char(3)");
        builder.Property(e => e.DueDate).HasColumnName("ASS_DUEDATE");
        builder.Property(e => e.MeetingFlag).HasColumnName("ASS_MEETINGFLAG").HasColumnType("char(1)");
        builder.Property(e => e.Status).HasColumnName("ASS_STATUS").HasColumnType("char(1)");
        builder.Property(e => e.Reason).HasColumnName("ASS_REASON").HasMaxLength(200);
        builder.Property(e => e.AssessmentDate).HasColumnName("ASS_DATE");
        builder.Property(e => e.ReviewFlag).HasColumnName("ASS_REVIEWFLAG").HasColumnType("char(1)");
        builder.Property(e => e.NewRiskFlag).HasColumnName("ASS_NEWFLAG").HasColumnType("char(1)");
        builder.Property(e => e.NewRiskList).HasColumnName("ASS_NEWLIST").HasMaxLength(200);
        builder.Property(e => e.MitigationFlag).HasColumnName("ASS_MITFLAG").HasColumnType("char(1)");
        builder.Property(e => e.MitigationList).HasColumnName("ASS_MITLIST").HasMaxLength(200);
        builder.Property(e => e.ApprovalStatus).HasColumnName("ASS_APPSTATUS").HasColumnType("char(1)");
        builder.Property(e => e.LastModifiedBy).HasColumnName("ASS_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("ASS_LASTMODIFIEDON");
        builder.HasMany(e => e.EventAssessments).WithOne().HasForeignKey(ea => ea.AssessmentId);
        builder.HasMany(e => e.Comments).WithOne().HasForeignKey(c => c.AssessmentId);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class RiskSelfAssessmentCommentConfiguration : IEntityTypeConfiguration<RiskSelfAssessmentComment>
{
    public void Configure(EntityTypeBuilder<RiskSelfAssessmentComment> builder)
    {
        builder.ToTable("RISK_SELFASSCOMMENT");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("COM_ID");
        builder.Property(e => e.AssessmentId).HasColumnName("ASS_ID");
        builder.Property(e => e.RiskId).HasColumnName("RISK ID");
        builder.Property(e => e.Comments).HasColumnName("Comments").HasMaxLength(2000);
        builder.Property(e => e.UpdatedBy).HasColumnName("Updated On");
        builder.Property(e => e.UpdatedOn).HasColumnName("Updated By");
        builder.Ignore(e => e.CreatedBy);
        builder.Ignore(e => e.CreatedOn);
        builder.Ignore(e => e.ModifiedBy);
        builder.Ignore(e => e.ModifiedOn);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class RiskEventAssessmentConfiguration : IEntityTypeConfiguration<RiskEventAssessment>
{
    public void Configure(EntityTypeBuilder<RiskEventAssessment> builder)
    {
        builder.ToTable("RISK_EVENTASSDET");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("EVENTASS_ID");
        builder.Property(e => e.AssessmentId).HasColumnName("EVENTASS_ASSID");
        builder.Property(e => e.RiskId).HasColumnName("EVENTASS_RISKID");
        builder.Property(e => e.LastModifiedBy).HasColumnName("EVENTASS_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("EVENTASS_LASTMODIFIEDON");
    }
}
