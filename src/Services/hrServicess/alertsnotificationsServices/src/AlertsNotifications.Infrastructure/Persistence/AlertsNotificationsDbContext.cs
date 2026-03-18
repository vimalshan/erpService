using AlertsNotifications.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlertsNotifications.Infrastructure.Persistence;

public class AlertsNotificationsDbContext : DbContext
{
    public AlertsNotificationsDbContext(DbContextOptions<AlertsNotificationsDbContext> options)
        : base(options)
    {
    }

    public DbSet<AlertMaster> AlertMasters => Set<AlertMaster>();
    public DbSet<AlertGroup> AlertGroups => Set<AlertGroup>();
    public DbSet<ProbationConfirmationAlert> ProbationConfirmationAlerts => Set<ProbationConfirmationAlert>();
    public DbSet<Circular> Circulars => Set<Circular>();
    public DbSet<CircularSignatory> CircularSignatories => Set<CircularSignatory>();
    public DbSet<CircularTemplate> CircularTemplates => Set<CircularTemplate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ALERT_MASTER
        modelBuilder.Entity<AlertMaster>(entity =>
        {
            entity.ToTable("ALERT_MASTER");
            entity.HasKey(e => e.AlertId);
            entity.Property(e => e.AlertId).HasColumnName("ALERT_ID").HasColumnType("decimal(38,0)").ValueGeneratedNever();
            entity.Property(e => e.AlertApps).HasColumnName("ALERT_APPS").HasMaxLength(10).IsRequired();
            entity.Property(e => e.AlertName).HasColumnName("ALERT_NAME").HasMaxLength(50).IsRequired();
            entity.Property(e => e.AlertType).HasColumnName("ALERT_TYPE").HasMaxLength(10).IsRequired();
            entity.Property(e => e.AlertDesc).HasColumnName("ALERT_DESC").HasMaxLength(200).IsRequired();
            entity.Property(e => e.AlertToDesc).HasColumnName("ALERT_TODESC").HasMaxLength(200);
            entity.Property(e => e.AlertCcDesc).HasColumnName("ALERT_CCDESC").HasMaxLength(200);
            entity.Property(e => e.AlertGradeCat).HasColumnName("ALERT_GRADECAT").HasMaxLength(3);
            entity.Property(e => e.AlertUnitSpecific).HasColumnName("ALERT_UNITSPECIFIC").HasColumnType("char(1)");
            entity.Ignore(e => e.DomainEvents);
        });

        // ALERTGRP_MASTER
        modelBuilder.Entity<AlertGroup>(entity =>
        {
            entity.ToTable("ALERTGRP_MASTER");
            entity.HasKey(e => e.AlertGroupId);
            entity.Property(e => e.AlertGroupId).HasColumnName("ALGRP_ID").HasColumnType("decimal(22,0)").ValueGeneratedNever();
            entity.Property(e => e.AlertGroupName).HasColumnName("ALGRP_NAME").HasMaxLength(100).IsRequired();
            entity.Property(e => e.AlertGroupType).HasColumnName("ALGRP_TYPE").HasColumnType("char(1)").IsRequired();
            entity.Property(e => e.CreatedBy).HasColumnName("ALGRP_CREATEDBY").HasColumnType("decimal(38,0)");
            entity.Property(e => e.CreatedOn).HasColumnName("ALGRP_CREATEDON").HasColumnType("datetime2(3)");
            entity.Property(e => e.ModifiedBy).HasColumnName("ALGRP_MODIFIEDBY").HasColumnType("decimal(38,0)");
            entity.Property(e => e.ModifiedOn).HasColumnName("ALGRP_MODIFIEDON").HasColumnType("datetime2(3)");
            entity.Ignore(e => e.DomainEvents);
        });

        // PROBCONFALERT
        modelBuilder.Entity<ProbationConfirmationAlert>(entity =>
        {
            entity.ToTable("PROBCONFALERT");
            entity.HasKey(e => e.ProbationId);
            entity.Property(e => e.ProbationId).HasColumnName("PROBATION_ID").ValueGeneratedNever();
            entity.Property(e => e.ProbationEmpSysId).HasColumnName("PROBATION_EMPSYSID");
            entity.Property(e => e.ProbationGrade).HasColumnName("PROBATION_GRADE");
            entity.Property(e => e.ProbationDate).HasColumnName("PROBATION_DATE").HasColumnType("datetime2(3)");
            entity.Property(e => e.SelfAppraisal).HasColumnName("SELFAPPRAISAL").HasColumnType("char(1)");
            entity.Property(e => e.AlertSentOn).HasColumnName("ALERT_SENTON").HasColumnType("datetime2(3)");
            entity.Ignore(e => e.DomainEvents);
        });

        // CIRCULAR_LIST
        modelBuilder.Entity<Circular>(entity =>
        {
            entity.ToTable("CIRCULAR_LIST");
            entity.HasKey(e => e.CircularId);
            entity.Property(e => e.CircularId).HasColumnName("CIRCULAR_ID").ValueGeneratedNever();
            entity.Property(e => e.CircularNo).HasColumnName("CIRCULAR_NO").HasMaxLength(100);
            entity.Property(e => e.CircularYearId).HasColumnName("CIRCULAR_YEARID");
            entity.Property(e => e.CircularType).HasColumnName("CIRCULAR_TYPE");
            entity.Property(e => e.CircularOrgId).HasColumnName("CIRCULAR_ORGID");
            entity.Property(e => e.CircularBuSpecific).HasColumnName("CIRCULAR_BUSPECIFIC");
            entity.Property(e => e.CircularUnitSpecific).HasColumnName("CIRCULAR_UNITSPECIFIC");
            entity.Property(e => e.CircularHrRoleId).HasColumnName("CIRCULAR_HRROLEID");
            entity.Property(e => e.CircularVersionNo).HasColumnName("CIRCULAR_VERSIONNO");
            entity.Property(e => e.CircularTemplateId).HasColumnName("CIRCULAR_TEMPLATEID");
            entity.Property(e => e.CircularPdfFileName).HasColumnName("CIRCULAR_PDFFILENAME").HasMaxLength(200);
            entity.Property(e => e.CircularRtf).HasColumnName("CIRCULAR_RTF").HasMaxLength(255);
            entity.Property(e => e.CircularSignatoryId).HasColumnName("CIRCULAR_SIGNATORYID");
            entity.Property(e => e.CircularSparshFlag).HasColumnName("CIRCULAR_SPARSHFLAG").HasColumnType("char(1)");
            entity.Property(e => e.CircularPostDate).HasColumnName("CIRCULAR_POSTDATE").HasColumnType("datetime2(3)");
            entity.Property(e => e.CircularRemoveDate).HasColumnName("CIRCULAR_REMOVEDATE").HasColumnType("datetime2(3)");
            entity.Property(e => e.CircularDesc).HasColumnName("CIRCULAR_DESC").HasMaxLength(4000).IsRequired();
            entity.Property(e => e.CircularSubject).HasColumnName("CIRCULAR_SUBJECT").HasMaxLength(1000).IsRequired();
            entity.Property(e => e.CircularToList).HasColumnName("CIRCULAR_TOLIST").HasMaxLength(4000).IsRequired();
            entity.Property(e => e.CircularCcList).HasColumnName("CIRCULAR_CCLIST").HasMaxLength(4000);
            entity.Property(e => e.CircularStatus).HasColumnName("CIRCULAR_STATUS").HasColumnType("char(1)").IsRequired();
            entity.Property(e => e.CircularAttachEmpFlag).HasColumnName("CIRCULAR_ATTACHEMPFLAG").HasColumnType("char(1)");
            entity.Property(e => e.CreatedBy).HasColumnName("CIRCULAR_CREATEDBY");
            entity.Property(e => e.CreatedOn).HasColumnName("CIRCULAR_CREATEDON").HasColumnType("datetime2(3)");
            entity.Property(e => e.ModifiedBy).HasColumnName("CIRCULAR_MODIFIEDBY").HasColumnType("decimal(38,0)");
            entity.Property(e => e.ModifiedOn).HasColumnName("CIRCULAR_MODIFIEDON").HasColumnType("datetime2(3)");
            entity.Property(e => e.CircularApprovedBy).HasColumnName("CIRCULAR_APPROVEDBY");
            entity.Property(e => e.CircularApprovedOn).HasColumnName("CIRCULAR_APPROVEDON").HasColumnType("datetime2(3)");
            entity.Property(e => e.CircularAppRemarks).HasColumnName("CIRCULAR_APPREMARKS").HasMaxLength(2000);
            entity.Ignore(e => e.DomainEvents);

            entity.HasMany(e => e.Signatories).WithOne().HasForeignKey(s => s.CircularSignatoryId);
            entity.HasOne(e => e.Template).WithMany().HasForeignKey(e => e.CircularTemplateId);
        });

        // CIRCULAR_SIGNATORY
        modelBuilder.Entity<CircularSignatory>(entity =>
        {
            entity.ToTable("CIRCULAR_SIGNATORY");
            entity.HasKey(e => e.CircularSignatoryId);
            entity.Property(e => e.CircularSignatoryId).HasColumnName("CIRSIGNATORY_ID").ValueGeneratedNever();
            entity.Property(e => e.CircularSignatoryUnitId).HasColumnName("CIRSIGNATORY_UNITID");
            entity.Property(e => e.CircularSignatoryTypeId).HasColumnName("CIRSIGNATORY_TYPEID");
            entity.Property(e => e.CircularSignatorySignId).HasColumnName("CIRSIGNATORY_SIGNID");
            entity.Property(e => e.CircularSignatoryStatus).HasColumnName("CIRSIGNATORY_STATUS").HasColumnType("char(1)").IsRequired();
            entity.Property(e => e.CircularSignatoryCreatedBy).HasColumnName("CIRSIGNATORY_CREATEDBY");
            entity.Property(e => e.CircularSignatoryCreatedOn).HasColumnName("CIRSIGNATORY_CREATEDON").HasColumnType("datetime2(3)");
            entity.Ignore(e => e.DomainEvents);
        });

        // CIRCULAR_TEMPLATE
        modelBuilder.Entity<CircularTemplate>(entity =>
        {
            entity.ToTable("CIRCULAR_TEMPLATE");
            entity.HasKey(e => e.CircularTemplateId);
            entity.Property(e => e.CircularTemplateId).HasColumnName("CIRTEMPLATE_ID").ValueGeneratedNever();
            entity.Property(e => e.CircularTemplateApplyToUnit).HasColumnName("CIRTEMPLATE_APPLYTOUNIT");
            entity.Property(e => e.CircularTemplateUnitId).HasColumnName("CIRTEMPLATE_UNITID");
            entity.Property(e => e.CircularTemplateTypeId).HasColumnName("CIRTEMPLATE_TYPEID");
            entity.Property(e => e.CircularTemplateName).HasColumnName("CIRTEMPLATE_NAME").HasMaxLength(200).IsRequired();
            entity.Property(e => e.CircularTemplateHtml).HasColumnName("CIRTEMPLATE_HTML").HasMaxLength(255).IsRequired();
            entity.Property(e => e.CircularTemplateClsDate).HasColumnName("CIRTEMPLATE_CLSDATE").HasColumnType("datetime2(3)");
            entity.Property(e => e.CircularTemplateModifiedBy).HasColumnName("CIRTEMPLATE_MODIFIEDBY");
            entity.Property(e => e.CircularTemplateModifiedOn).HasColumnName("CIRTEMPLATE_MODIFIEDON").HasColumnType("datetime2(3)");
            entity.Ignore(e => e.DomainEvents);
        });
    }
}
