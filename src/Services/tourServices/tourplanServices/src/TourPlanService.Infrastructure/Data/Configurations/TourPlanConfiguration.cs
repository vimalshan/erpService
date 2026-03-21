using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourPlanService.Domain.Entities;

namespace TourPlanService.Infrastructure.Data.Configurations;

public sealed class TourPlanConfiguration : IEntityTypeConfiguration<TourPlan>
{
    public void Configure(EntityTypeBuilder<TourPlan> builder)
    {
        builder.ToTable("TOURPLAN_MAIN");
        builder.HasKey(x => x.TpId);

        builder.Property(x => x.TpId).HasColumnName("TP_ID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpEmpSysId).HasColumnName("TP_EMPSYSID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpStartDate).HasColumnName("TP_STARTDATE").IsRequired();
        builder.Property(x => x.TpEndDate).HasColumnName("TP_ENDDATE");
        builder.Property(x => x.TpPurpose).HasColumnName("TP_PURPOSE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpRemarks).HasColumnName("TP_REMARKS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpStatus).HasColumnName("TP_STATUS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpCategory).HasColumnName("TP_CATEGORY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpBookInc).HasColumnName("TP_BOOKINC").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpType).HasColumnName("TP_TYPE").HasMaxLength(255);
        builder.Property(x => x.TpCreatedBy).HasColumnName("TP_CREATEDBY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpCreatedOn).HasColumnName("TP_CREATEDON").IsRequired();
        builder.Property(x => x.TpApprovedBy).HasColumnName("TP_APPROVEDBY").HasMaxLength(255);
        builder.Property(x => x.TpApprovedOn).HasColumnName("TP_APPROVEDON");
        builder.Property(x => x.TpLastModifiedBy).HasColumnName("TP_LASTMODIFIEDBY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpLastModifiedOn).HasColumnName("TP_LASTMODIFIEDON").IsRequired();
        builder.Property(x => x.TpFromCityId).HasColumnName("TP_FROMCITYID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpFromCityName).HasColumnName("TP_FROMCITYNAME").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpToCityId).HasColumnName("TP_TOCITYID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpToCityName).HasColumnName("TP_TOCITYNAME").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpSupRemarks).HasColumnName("TP_SUPREMARKS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpContactNo).HasColumnName("TP_CONTACTNO").HasMaxLength(255);
        builder.Property(x => x.TpGradeType).HasColumnName("TP_GRADETYPE").HasMaxLength(255);
        builder.Property(x => x.TpHomeCountryId).HasColumnName("TP_HOMECOUNTRYID").HasMaxLength(255);
        builder.Property(x => x.TpTravelSectorId).HasColumnName("TP_TRAVELSECTORID").HasMaxLength(255);
        builder.Property(x => x.TpCostEffective).HasColumnName("TP_COSTEFFECTIVE").HasMaxLength(255);
        builder.Property(x => x.TpCostJustify).HasColumnName("TP_COSTJUSTIFY").HasMaxLength(255);
        builder.Property(x => x.TpClaimType).HasColumnName("TP_CLAIMTYPE").HasMaxLength(255);
        builder.Property(x => x.TpSpecialRemarks).HasColumnName("TP_SPECIALREMARKS").HasMaxLength(255);
        builder.Property(x => x.TpAppRemarks).HasColumnName("TP_APPREMARKS").HasMaxLength(255);
        builder.Property(x => x.TpAppLevel).HasColumnName("TP_APPLEVEL").HasMaxLength(255);
        builder.Property(x => x.TpBalPayAmt).HasColumnName("TP_BALPAYAMT").HasMaxLength(255);
        builder.Property(x => x.TpCeoEmpSysId).HasColumnName("TP_CEOEMPSYSID").HasMaxLength(255);
        builder.Property(x => x.TpDaEffDate).HasColumnName("TP_DAEFFDATE");
        builder.Property(x => x.TpDaClsDate).HasColumnName("TP_DACLSDATE");
        builder.Property(x => x.TpDaValue).HasColumnName("TP_DAVALUE").HasMaxLength(255);
        builder.Property(x => x.TpDaToolTip).HasColumnName("TP_DATOOLTIP").HasMaxLength(255);
        builder.Property(x => x.TpExpStatus).HasColumnName("TP_EXPSTATUS").HasMaxLength(255);
        builder.Property(x => x.TpExpApprovedBy).HasColumnName("TP_EXPAPPROVEDBY").HasMaxLength(255);
        builder.Property(x => x.TpExpApprovedOn).HasColumnName("TP_EXPAPPROVEDON");
        builder.Property(x => x.TpRecommenderSysId).HasColumnName("TP_RECOMMENDERSYSID").HasMaxLength(255);
        builder.Property(x => x.TpPayUnitId).HasColumnName("TP_PAYUNITID").HasMaxLength(255);
        builder.Property(x => x.TpDaDays).HasColumnName("TP_DADAYS").HasMaxLength(255);
        builder.Property(x => x.TpDaRate).HasColumnName("TP_DARATE").HasMaxLength(255);
        builder.Property(x => x.TpExpPayMode).HasColumnName("TP_EXPPAYMODE").HasMaxLength(255);
        builder.Property(x => x.TpExpJvId).HasColumnName("TP_EXPJVID").HasMaxLength(255);
        builder.Property(x => x.TpExpSubmitedOn).HasColumnName("TP_EXPSUBMITEDON");
        builder.Property(x => x.TpExpSubmitedBy).HasColumnName("TP_EXPSUBMITEDBY").HasMaxLength(255);
        builder.Property(x => x.TpEstimateConvRate1).HasColumnName("TP_ESTIMATECONVRATE1").HasMaxLength(255);
        builder.Property(x => x.TpEstimateConvRate2).HasColumnName("TP_ESTIMATECONVRATE2").HasMaxLength(255);
        builder.Property(x => x.TpActRemarks).HasColumnName("TP_ACTREMARKS").HasMaxLength(255);
        builder.Property(x => x.TpEstimateConvRate3).HasColumnName("TP_ESTIMATECONVRATE3").HasMaxLength(255);
        builder.Property(x => x.TpClosureStatus).HasColumnName("TP_CLOSURESTATUS").HasMaxLength(1);

        // Navigation - use HasMany with private backing fields
        builder.HasMany(x => x.Advances).WithOne(x => x.TourPlan).HasForeignKey(x => x.AdvTpId);
        builder.HasMany(x => x.Agendas).WithOne(x => x.TourPlan).HasForeignKey(x => x.AgendaTpId);
        builder.HasMany(x => x.CostCentres).WithOne(x => x.TourPlan).HasForeignKey(x => x.TpCostTpId);
        builder.HasMany(x => x.DaBreaks).WithOne(x => x.TourPlan).HasForeignKey(x => x.TpDaTpId);
        builder.HasMany(x => x.Expenses).WithOne(x => x.TourPlan).HasForeignKey(x => x.TpExpTpId);
        builder.HasMany(x => x.IntSchedules).WithOne(x => x.TourPlan).HasForeignKey(x => x.IntSchTpId);
        builder.HasMany(x => x.Leaves).WithOne(x => x.TourPlan).HasForeignKey(x => x.LeaveTpId);
        builder.HasMany(x => x.NmsSchedules).WithOne(x => x.TourPlan).HasForeignKey(x => x.NmsSchTpId);
        builder.HasMany(x => x.SelfExpenses).WithOne(x => x.TourPlan).HasForeignKey(x => x.ExpTpId);
        builder.HasMany(x => x.ForexRequisitions).WithOne(x => x.TourPlan).HasForeignKey(x => x.ForReqTpId);
        builder.HasMany(x => x.DomesticDaBreaks).WithOne(x => x.TourPlan).HasForeignKey(x => x.DomDaTpId);
        builder.HasMany(x => x.ForeignExpenses).WithOne(x => x.TourPlan).HasForeignKey(x => x.TpExpMainTpId);

        builder.Ignore(x => x.DomainEvents);
    }
}
