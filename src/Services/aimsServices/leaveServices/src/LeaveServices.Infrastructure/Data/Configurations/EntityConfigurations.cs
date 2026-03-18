using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LeaveServices.Domain.Entities;

namespace LeaveServices.Infrastructure.Data.Configurations;

public sealed class LeaveMasterConfiguration : IEntityTypeConfiguration<LeaveMaster>
{
    public void Configure(EntityTypeBuilder<LeaveMaster> builder)
    {
        builder.ToTable("LEAVE_MASTER");
        builder.HasKey(x => x.LeaveId);
        builder.Property(x => x.LeaveId).HasColumnName("LEAVE_ID").ValueGeneratedNever();
        builder.Ignore(x => x.Id);
        builder.Property(x => x.LeaveDescription).HasColumnName("LEAVE_DESCRIPTION").HasMaxLength(255).IsRequired();
        builder.Property(x => x.LeaveGenderSpecific).HasColumnName("LEAVE_GENDERSPECIFIC").HasColumnType("CHAR(1)").IsRequired();
        builder.Property(x => x.LeaveApplicableForAll).HasColumnName("LEAVE_APPLICABLEFORALL").HasColumnType("CHAR(1)").IsRequired();
        builder.Property(x => x.LeaveMaxDaysPL).HasColumnName("LEAVE_MAXDAYSPL").IsRequired();
        builder.Property(x => x.LeaveEncashable).HasColumnName("LEAVE_ENCASHABLE").HasColumnType("CHAR(1)").IsRequired();
        builder.Property(x => x.LeaveCarryForward).HasColumnName("LEAVE_CARRYFORWARD").HasColumnType("CHAR(1)").IsRequired();
        builder.Property(x => x.LeaveLastModifiedBy).HasColumnName("LEAVE_LASTMODIFIEDBY").IsRequired();
        builder.Property(x => x.LeaveLastModifiedOn).HasColumnName("LEAVE_LASTMODIFIEDON").HasColumnType("DATETIME2(3)").IsRequired();
        builder.HasIndex(x => x.LeaveDescription).IsUnique().HasDatabaseName("UQ_LEAVE_DESC");

        builder.HasMany(x => x.LeaveDetailsList).WithOne(x => x.LeaveMaster)
            .HasForeignKey(x => x.LeaveId).HasConstraintName("FK_LEAVE_DETAILS_MASTER");
        builder.HasMany(x => x.LeaveCreditList).WithOne(x => x.LeaveMaster)
            .HasForeignKey(x => x.CreditLeaveId).HasConstraintName("FK_CREDIT_LEAVE");
        builder.HasMany(x => x.LeaveRulesList).WithOne(x => x.LeaveMaster)
            .HasForeignKey(x => x.RuleLeaveId).HasConstraintName("FK_RULE_LEAVE");
    }
}

public sealed class LeaveDetailsConfiguration : IEntityTypeConfiguration<LeaveDetails>
{
    public void Configure(EntityTypeBuilder<LeaveDetails> builder)
    {
        builder.ToTable("LEAVE_DETAILS");
        builder.HasKey(x => x.LeaveDetailId);
        builder.Property(x => x.LeaveDetailId).HasColumnName("LEAVE_DETAILID").ValueGeneratedNever();
        builder.Ignore(x => x.Id);
        builder.Property(x => x.LeaveEmpSysId).HasColumnName("LEAVE_EMPSYSID").IsRequired();
        builder.Property(x => x.LeaveAppFrom).HasColumnName("LEAVE_APPFROM").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(x => x.LeaveAppTo).HasColumnName("LEAVE_APPTO").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(x => x.LeaveAppType).HasColumnName("LEAVE_APPTYPE").HasMaxLength(10).IsRequired();
        builder.Property(x => x.LeaveId).HasColumnName("LEAVE_ID").IsRequired();
        builder.Property(x => x.LeaveTimeUnitId).HasColumnName("LEAVE_TIMEUNITID").IsRequired();
        builder.Property(x => x.LeaveAppStatus).HasColumnName("LEAVE_APPSTATUS").HasColumnType("CHAR(1)").IsRequired();
        builder.Property(x => x.LeaveAppliedDays).HasColumnName("LEAVE_APPLIEDDAYS").HasPrecision(5, 2).IsRequired();
        builder.Property(x => x.LeaveReason).HasColumnName("LEAVE_REASON").HasMaxLength(500);
        builder.Property(x => x.LeaveEnteredOn).HasColumnName("LEAVE_ENTEREDON").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(x => x.LeaveEnteredBy).HasColumnName("LEAVE_ENTEREDBY").IsRequired();
        builder.Property(x => x.LeaveLastModifiedBy).HasColumnName("LEAVE_LASTMODIFIEDBY").IsRequired();
        builder.Property(x => x.LeaveLastModifiedOn).HasColumnName("LEAVE_LASTMODIFIEDON").HasColumnType("DATETIME2(3)").IsRequired();

        builder.HasMany(x => x.Approvals).WithOne(x => x.LeaveDetails)
            .HasForeignKey(x => x.LeaveAprDetailId).HasConstraintName("FK_LEAVEAPR_DETAILS");
    }
}

public sealed class LeaveCreditConfiguration : IEntityTypeConfiguration<LeaveCredit>
{
    public void Configure(EntityTypeBuilder<LeaveCredit> builder)
    {
        builder.ToTable("LEAVE_CREDIT");
        builder.HasKey(x => x.CreditId);
        builder.Property(x => x.CreditId).HasColumnName("CREDIT_ID").ValueGeneratedNever();
        builder.Ignore(x => x.Id);
        builder.Property(x => x.CreditEmpSysId).HasColumnName("CREDIT_EMPSYSID").IsRequired();
        builder.Property(x => x.CreditLeaveId).HasColumnName("CREDIT_LEAVEID").IsRequired();
        builder.Property(x => x.CreditLeaveFlag).HasColumnName("CREDIT_LEAVEFLAG").HasColumnType("CHAR(1)").IsRequired();
        builder.Property(x => x.CreditYear).HasColumnName("CREDIT_YEAR").IsRequired();
        builder.Property(x => x.CreditOpening).HasColumnName("CREDIT_OPENING").HasPrecision(5, 2).IsRequired();
        builder.Property(x => x.CreditCredited).HasColumnName("CREDIT_CREDITED").HasPrecision(5, 2).IsRequired();
        builder.Property(x => x.CreditUtilized).HasColumnName("CREDIT_UTILIZED").HasPrecision(5, 2).IsRequired();
        builder.Property(x => x.CreditClosing).HasColumnName("CREDIT_CLOSING").HasPrecision(5, 2).IsRequired();
        builder.Property(x => x.CreditLastModifiedBy).HasColumnName("CREDIT_LASTMODIFIEDBY").IsRequired();
        builder.Property(x => x.CreditLastModifiedOn).HasColumnName("CREDIT_LASTMODIFIEDON").HasColumnType("DATETIME2(3)").IsRequired();
        builder.HasIndex(x => new { x.CreditEmpSysId, x.CreditLeaveId, x.CreditYear }).IsUnique().HasDatabaseName("UQ_LEAVE_CREDIT");
        builder.Ignore(x => x.AvailableBalance);
    }
}

public sealed class LeaveDetailsApprovalConfiguration : IEntityTypeConfiguration<LeaveDetailsApproval>
{
    public void Configure(EntityTypeBuilder<LeaveDetailsApproval> builder)
    {
        builder.ToTable("LEAVE_DETAILSAPR");
        builder.HasKey(x => x.LeaveAprId);
        builder.Property(x => x.LeaveAprId).HasColumnName("LEAVEAPR_ID").ValueGeneratedNever();
        builder.Ignore(x => x.Id);
        builder.Property(x => x.LeaveAprDetailId).HasColumnName("LEAVEAPR_DETAILID").IsRequired();
        builder.Property(x => x.LeaveAprApproveStatus).HasColumnName("LEAVEAPR_APPROVESTATUS").HasColumnType("CHAR(1)").IsRequired();
        builder.Property(x => x.LeaveAprRemarks).HasColumnName("LEAVEAPR_REMARKS").HasMaxLength(500);
        builder.Property(x => x.LeaveAprApprovedOn).HasColumnName("LEAVEAPR_APPROVEDON").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(x => x.LeaveAprApprovedBy).HasColumnName("LEAVEAPR_APPROVEDBY").IsRequired();
        builder.Property(x => x.LeaveAprLastModifiedBy).HasColumnName("LEAVEAPR_LASTMODIFIEDBY").IsRequired();
        builder.Property(x => x.LeaveAprLastModifiedOn).HasColumnName("LEAVEAPR_LASTMODIFIEDON").HasColumnType("DATETIME2(3)").IsRequired();
    }
}

public sealed class LeaveRulesConfiguration : IEntityTypeConfiguration<LeaveRules>
{
    public void Configure(EntityTypeBuilder<LeaveRules> builder)
    {
        builder.ToTable("LEAVE_RULES");
        builder.HasKey(x => x.RuleId);
        builder.Property(x => x.RuleId).HasColumnName("RULE_ID").ValueGeneratedNever();
        builder.Ignore(x => x.Id);
        builder.Property(x => x.RuleLeaveId).HasColumnName("RULE_LEAVEID").IsRequired();
        builder.Property(x => x.RuleMaxDaysInAppl).HasColumnName("RULE_MAXDAYSINAPPL").IsRequired();
        builder.Property(x => x.RuleMinDaysInAppl).HasColumnName("RULE_MINDAYSINAPPL").IsRequired();
        builder.Property(x => x.RuleMaxYearLimit).HasColumnName("RULE_MAXYEARLIMIT").IsRequired();
        builder.Property(x => x.RuleClubbing).HasColumnName("RULE_CLUBBING").HasColumnType("CHAR(1)").IsRequired();
        builder.Property(x => x.RuleLastModifiedBy).HasColumnName("RULE_LASTMODIFIEDBY").IsRequired();
        builder.Property(x => x.RuleLastModifiedOn).HasColumnName("RULE_LASTMODIFIEDON").HasColumnType("DATETIME2(3)").IsRequired();
    }
}

public sealed class CompOffAdjustConfiguration : IEntityTypeConfiguration<CompOffAdjust>
{
    public void Configure(EntityTypeBuilder<CompOffAdjust> builder)
    {
        builder.ToTable("COMPOFF_ADJUST");
        builder.HasKey(x => x.CompOffId);
        builder.Property(x => x.CompOffId).HasColumnName("COMPOFF_ID").ValueGeneratedNever();
        builder.Ignore(x => x.Id);
        builder.Property(x => x.CompOffEmpSysId).HasColumnName("COMPOFF_EMPSYSID").IsRequired();
        builder.Property(x => x.CompOffCompOffDate).HasColumnName("COMPOFF_COMPOFFDATE").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(x => x.CompOffUsedDate).HasColumnName("COMPOFF_USEDDATE").HasColumnType("DATETIME2(3)");
        builder.Property(x => x.CompOffStatus).HasColumnName("COMPOFF_STATUS").HasMaxLength(1).IsRequired();
        builder.Property(x => x.CompOffLastModifiedBy).HasColumnName("COMPOFF_LASTMODIFIEDBY").IsRequired();
        builder.Property(x => x.CompOffLastModifiedOn).HasColumnName("COMPOFF_LASTMODIFIEDON").HasColumnType("DATETIME2(3)").IsRequired();
    }
}
