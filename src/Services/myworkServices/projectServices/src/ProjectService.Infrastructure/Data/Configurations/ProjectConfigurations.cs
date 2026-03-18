using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectService.Domain.Entities;

namespace ProjectService.Infrastructure.Data.Configurations;

public class ProjectMainConfiguration : IEntityTypeConfiguration<ProjectMain>
{
    public void Configure(EntityTypeBuilder<ProjectMain> builder)
    {
        builder.ToTable("PROJECT_MAIN");
        builder.HasKey(e => e.ProjId);
        builder.Property(e => e.ProjId).HasColumnName("PROJ_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ProjName).HasColumnName("PROJ_NAME").HasMaxLength(50).IsRequired();
        builder.Property(e => e.ProjCharterNo).HasColumnName("PROJ_CHARTERNO").HasColumnType("decimal(38,0)");
        builder.Property(e => e.ProjLeaderId).HasColumnName("PROJ_LEADERID");
        builder.Property(e => e.ProjTypeId).HasColumnName("PROJ_TYPEID");
        builder.Property(e => e.ProjLocId).HasColumnName("PROJ_LOCID");
        builder.Property(e => e.ProjProcessId).HasColumnName("PROJ_PROCESSID");
        builder.Property(e => e.ProjStartDate).HasColumnName("PROJ_STARTDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.ProjEndDate).HasColumnName("PROJ_ENDDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.ProjEstEndDate).HasColumnName("PROJ_ESTENDDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.ProjStatus).HasColumnName("PROJ_STATUS").HasColumnType("char(1)");
        builder.Property(e => e.ProjRevNo).HasColumnName("PROJ_REVNO");
        builder.Property(e => e.ProjVerNo).HasColumnName("PROJ_VERNO");
        builder.Property(e => e.ProjObjId).HasColumnName("PROJ_OBJID");
        builder.Property(e => e.ProjObjDesc).HasColumnName("PROJ_OBJDESC").HasMaxLength(150);
        builder.Property(e => e.ProjTargetProd).HasColumnName("PROJ_TARGETPROD").HasMaxLength(50);
        builder.Property(e => e.ProjTargetProdRem).HasColumnName("PROJ_TARGETPRODREM").HasMaxLength(150);
        builder.Property(e => e.ProjTargetSpecFile).HasColumnName("PROJ_TARGETSPECFILE").HasMaxLength(150);
        builder.Property(e => e.ProjTargetSpecRem).HasColumnName("PROJ_TARGETSPECREM").HasMaxLength(150);
        builder.Property(e => e.ProjTargetYieldFile).HasColumnName("PROJ_TARGETYIELDFILE").HasMaxLength(150);
        builder.Property(e => e.ProjTargetYieldRem).HasColumnName("PROJ_TARGETYIELDREM").HasMaxLength(150);
        builder.Property(e => e.ProjNotes).HasColumnName("PROJ_NOTES").HasMaxLength(2000);
        builder.Property(e => e.ProjActualProd).HasColumnName("PROJ_ACTUALPROD").HasMaxLength(50);
        builder.Property(e => e.ProjActualProdRem).HasColumnName("PROJ_ACTUALPRODREM").HasMaxLength(150);
        builder.Property(e => e.ProjActualSpecFile).HasColumnName("PROJ_ACTUALSPECFILE").HasMaxLength(150);
        builder.Property(e => e.ProjActualSpecRem).HasColumnName("PROJ_ACTUALSPECREM").HasMaxLength(150);
        builder.Property(e => e.ProjActualYieldFile).HasColumnName("PROJ_ACTUALYIELDFILE").HasMaxLength(150);
        builder.Property(e => e.ProjActualYieldRem).HasColumnName("PROJ_ACTUALYIELDREM").HasMaxLength(150);
        builder.Property(e => e.ProjClsDate).HasColumnName("PROJ_CLSDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.ProjLastModifiedOn).HasColumnName("PROJ_LASTMODIFIEDON").HasColumnType("datetime2(3)");
        builder.Property(e => e.ProjAppEmpSysId).HasColumnName("PROJ_APPEMPSYSID").HasColumnType("decimal(38)");
        builder.Property(e => e.ProjPlanFile).HasColumnName("PROJ_PLANFILE").HasMaxLength(150);
        builder.Property(e => e.ProjTargetLbl1).HasColumnName("PROJ_TARGETLBL1").HasMaxLength(100);
        builder.Property(e => e.ProjTargetLbl2).HasColumnName("PROJ_TARGETLBL2").HasMaxLength(100);
        builder.Property(e => e.ProjTargetLbl3).HasColumnName("PROJ_TARGETLBL3").HasMaxLength(100);
        builder.Property(e => e.ProjPptxFile).HasColumnName("PROJ_PPTXFILE").HasMaxLength(150);

        builder.HasMany(e => e.Members).WithOne(m => m.Project).HasForeignKey(m => m.ProjMemProjId);
        builder.HasMany(e => e.Scopes).WithOne(s => s.Project).HasForeignKey(s => s.ProjScopeProjId);
        builder.HasMany(e => e.StatusHistory).WithOne(s => s.Project).HasForeignKey(s => s.ProjStatusProjId);
        builder.HasMany(e => e.AdditionalDeliverables).WithOne(a => a.Project).HasForeignKey(a => a.ProjAdlDelProjId);
        builder.HasMany(e => e.AdditionalScopes).WithOne(a => a.Project).HasForeignKey(a => a.ProjAdScopeProjId);
        builder.HasMany(e => e.ApprovalDetails).WithOne(a => a.Project).HasForeignKey(a => a.ProjApprProjId);
        builder.HasMany(e => e.Deliverables).WithOne(d => d.Project).HasForeignKey(d => d.ProjDelProjId);
        builder.HasMany(e => e.Holds).WithOne(h => h.Project).HasForeignKey(h => h.ProjHoldProjId);

        builder.HasOne(e => e.ProjectType).WithMany().HasForeignKey(e => e.ProjTypeId).HasPrincipalKey(t => t.ProjTypeId);
        builder.HasOne(e => e.Location).WithMany().HasForeignKey(e => e.ProjLocId);
        builder.HasOne(e => e.Process).WithMany().HasForeignKey(e => e.ProjProcessId);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectMasterConfiguration : IEntityTypeConfiguration<ProjectMaster>
{
    public void Configure(EntityTypeBuilder<ProjectMaster> builder)
    {
        builder.ToTable("PROJECT_MASTER");
        builder.HasKey(e => e.ProjectId);
        builder.Property(e => e.ProjectId).HasColumnName("PROJECT_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ProjectName).HasColumnName("PROJECT_NAME").HasMaxLength(200).IsRequired();
        builder.Property(e => e.ProjectCategoryId).HasColumnName("PROJECT_CATEGORYID");
        builder.Property(e => e.ProjectEffDate).HasColumnName("PROJECT_EFFDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.ProjectClsDate).HasColumnName("PROJECT_CLSDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.ProjectTeamId).HasColumnName("PROJECT_TEAMID").HasColumnType("decimal(38)");
        builder.Property(e => e.ProjectListAll).HasColumnName("PROJECT_LISTALL").HasColumnType("char(1)");
        builder.Property(e => e.LastModifiedBy).HasColumnName("PROJECT_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("PROJECT_LASTMODIFIEDON").HasColumnType("datetime2(3)");

        builder.HasOne(e => e.Category).WithMany(c => c.Projects).HasForeignKey(e => e.ProjectCategoryId);
        builder.HasMany(e => e.EmployeeMaps).WithOne(em => em.Project).HasForeignKey(em => em.ProjEmpProjectId);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
{
    public void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        builder.ToTable("PROJECT_MEMBERS");
        builder.HasKey(e => e.ProjMemId);
        builder.Property(e => e.ProjMemId).HasColumnName("PROJMEM_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ProjMemProjId).HasColumnName("PROJMEM_PROJID");
        builder.Property(e => e.ProjMemFuncId).HasColumnName("PROJMEM_FUNCID");
        builder.Property(e => e.ProjMemEmpSysId).HasColumnName("PROJMEM_EMPSYSID");

        builder.HasOne(e => e.Function).WithMany().HasForeignKey(e => e.ProjMemFuncId);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectScopeConfiguration : IEntityTypeConfiguration<ProjectScope>
{
    public void Configure(EntityTypeBuilder<ProjectScope> builder)
    {
        builder.ToTable("PROJECT_SCOPE");
        builder.HasKey(e => e.ProjScopeId);
        builder.Property(e => e.ProjScopeId).HasColumnName("PROJSCOPE_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ProjScopeProjId).HasColumnName("PROJSCOPE_PROJID");
        builder.Property(e => e.ProjScopeScopeId).HasColumnName("PROJSCOPE_SCOPEID");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectStatusHistoryConfiguration : IEntityTypeConfiguration<ProjectStatusHistory>
{
    public void Configure(EntityTypeBuilder<ProjectStatusHistory> builder)
    {
        builder.ToTable("PROJECT_STATUS");
        builder.HasKey(e => e.ProjStatusId);
        builder.Property(e => e.ProjStatusId).HasColumnName("PROJSTATUS_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ProjStatusProjId).HasColumnName("PROJSTATUS_PROJID");
        builder.Property(e => e.ProjStatusFile).HasColumnName("PROJSTATUS_FILE").HasMaxLength(150);
        builder.Property(e => e.ProjStatusDate).HasColumnName("PROJSTATUS_DATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.ProjStatusRem).HasColumnName("PROJSTATUS_REM").HasMaxLength(150).IsRequired();
        builder.Property(e => e.ProjStatusRevNo).HasColumnName("PROJSTATUS_REVNO");
        builder.Property(e => e.ProjStatusVerNo).HasColumnName("PROJSTATUS_VERNO");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectAdditionalDeliverableConfiguration : IEntityTypeConfiguration<ProjectAdditionalDeliverable>
{
    public void Configure(EntityTypeBuilder<ProjectAdditionalDeliverable> builder)
    {
        builder.ToTable("PROJECT_ADDLDEL");
        builder.HasKey(e => e.ProjAdlDelId);
        builder.Property(e => e.ProjAdlDelId).HasColumnName("PROJADLDEL_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ProjAdlDelProjId).HasColumnName("PROJADLDEL_PROJID");
        builder.Property(e => e.ProjAdlDelDesc).HasColumnName("PROJADLDEL_DESC").HasMaxLength(50).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectAdditionalScopeConfiguration : IEntityTypeConfiguration<ProjectAdditionalScope>
{
    public void Configure(EntityTypeBuilder<ProjectAdditionalScope> builder)
    {
        builder.ToTable("PROJECT_ADDLSCOPE");
        builder.HasKey(e => e.ProjAdScopeId);
        builder.Property(e => e.ProjAdScopeId).HasColumnName("PROJADSCOPE_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ProjAdScopeProjId).HasColumnName("PROJADSCOPE_PROJID");
        builder.Property(e => e.ProjAdScopeDesc).HasColumnName("PROJADSCOPE_DESC").HasMaxLength(50).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectApprovalDetailConfiguration : IEntityTypeConfiguration<ProjectApprovalDetail>
{
    public void Configure(EntityTypeBuilder<ProjectApprovalDetail> builder)
    {
        builder.ToTable("PROJECT_APPRDETAILS");
        builder.HasKey(e => e.ProjApprId);
        builder.Property(e => e.ProjApprId).HasColumnName("PROJ_APPRID").ValueGeneratedOnAdd();
        builder.Property(e => e.ProjApprProjId).HasColumnName("PROJ_APPRPROJID");
        builder.Property(e => e.ProjApprType).HasColumnName("PROJ_APPRTYPE").HasColumnType("char(1)");
        builder.Property(e => e.ProjApprSentOn).HasColumnName("PROJ_APPRSENTON").HasColumnType("datetime2(3)");
        builder.Property(e => e.ProjAppEmpSysId).HasColumnName("PROJ_APPEMPSYSID");
        builder.Property(e => e.ProjApprAppDate).HasColumnName("PROJ_APPRAPPDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.ProjApprStatus).HasColumnName("PROJ_APPRSTATUS").HasColumnType("char(1)");
        builder.Property(e => e.ProjApprRemarks).HasColumnName("PROJ_APPREMARKS").HasMaxLength(150).IsRequired();
        builder.Property(e => e.ProjApprDropRemarks).HasColumnName("PROJ_APPRDROPREMARKS").HasMaxLength(150).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectDeliverableConfiguration : IEntityTypeConfiguration<ProjectDeliverable>
{
    public void Configure(EntityTypeBuilder<ProjectDeliverable> builder)
    {
        builder.ToTable("PROJECT_DEL");
        builder.HasKey(e => e.ProjDelId);
        builder.Property(e => e.ProjDelId).HasColumnName("PROJDEL_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ProjDelProjId).HasColumnName("PROJDEL_PROJID");
        builder.Property(e => e.ProjDelDelId).HasColumnName("PROJDEL_DELID");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectHoldConfiguration : IEntityTypeConfiguration<ProjectHold>
{
    public void Configure(EntityTypeBuilder<ProjectHold> builder)
    {
        builder.ToTable("PROJECT_HOLD");
        builder.HasKey(e => e.ProjHoldId);
        builder.Property(e => e.ProjHoldId).HasColumnName("PROJHOLD_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ProjHoldProjId).HasColumnName("PROJHOLD_PROJID");
        builder.Property(e => e.ProjHoldType).HasColumnName("PROJHOLD_TYPE").HasColumnType("char(1)");
        builder.Property(e => e.ProjHoldDate).HasColumnName("PROJHOLD_DATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.ProjHoldReason).HasColumnName("PROJHOLD_REASON").HasMaxLength(150).IsRequired();
        builder.Property(e => e.ProjHoldUpdatedBy).HasColumnName("PROJHOLD_UPDATEDBY");
        builder.Property(e => e.ProjHoldUpdatedOn).HasColumnName("PROJHOLD_UPDATEDON").HasColumnType("datetime2(3)");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectEmployeeMapConfiguration : IEntityTypeConfiguration<ProjectEmployeeMap>
{
    public void Configure(EntityTypeBuilder<ProjectEmployeeMap> builder)
    {
        builder.ToTable("PROJECT_EMPMAP");
        builder.HasKey(e => e.ProjEmpId);
        builder.Property(e => e.ProjEmpId).HasColumnName("PROJEMP_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ProjEmpProjectId).HasColumnName("PROJEMP_PROJECTID");
        builder.Property(e => e.ProjEmpEmpSysId).HasColumnName("PROJEMP_EMPSYSID");
        builder.Property(e => e.LastModifiedBy).HasColumnName("PROJEMP_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("PROJEMP_LASTMODIFIEDON").HasColumnType("datetime2(3)");
        builder.Property(e => e.ProjEmpEffDate).HasColumnName("PROJEMP_EFFDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.ProjEmpCloseDate).HasColumnName("PROJEMP_CLOSEDATE").HasColumnType("datetime2(3)");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectAccessConfiguration : IEntityTypeConfiguration<ProjectAccess>
{
    public void Configure(EntityTypeBuilder<ProjectAccess> builder)
    {
        builder.ToTable("PROJ_ACCESS");
        builder.HasKey(e => e.ProjAccId);
        builder.Property(e => e.ProjAccId).HasColumnName("PROJACC_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ProjAccEmpSysId).HasColumnName("PROJACC_EMPSYSID");
        builder.Property(e => e.ProjAccType).HasColumnName("PROJACC_TYPE").HasColumnType("char(1)");
        builder.Property(e => e.ProjAccDepId).HasColumnName("PROJACC_DEPID");
        builder.Ignore(e => e.DomainEvents);
    }
}
