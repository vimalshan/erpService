using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectService.Domain.Entities;

namespace ProjectService.Infrastructure.Data.Configurations;

public class ProjectTypeMasterConfiguration : IEntityTypeConfiguration<ProjectTypeMaster>
{
    public void Configure(EntityTypeBuilder<ProjectTypeMaster> builder)
    {
        builder.ToTable("PROJTYPE_MAST");
        builder.HasKey(e => e.ProjTypeId);
        builder.Property(e => e.ProjTypeId).HasColumnName("PROJTYPE_ID");
        builder.Property(e => e.ProjTypeName).HasColumnName("PROJTYPE_NAME").HasMaxLength(50).IsRequired();
        builder.Property(e => e.ProjTypeCode).HasColumnName("PROJTYPE_CODE").HasMaxLength(50).IsRequired();
        builder.Property(e => e.ProjTypeDepId).HasColumnName("PROJTYPE_DEPID").HasColumnType("decimal(38)");
        builder.Property(e => e.ProjTypeCatId).HasColumnName("PROJTYPE_CATID");
        builder.Property(e => e.ProjTypeModifiedBy).HasColumnName("PROJTYPE_MODIFIEDBY").HasColumnType("decimal(38)");
        builder.Property(e => e.ProjTypeModifiedOn).HasColumnName("PROJTYPE_MODIFIEDON").HasColumnType("datetime2(3)");

        builder.HasMany(e => e.DeliverableMaps).WithOne(d => d.ProjectType).HasForeignKey(d => d.DelProjTypeId);
        builder.HasMany(e => e.ObjectiveMaps).WithOne(o => o.ProjectType).HasForeignKey(o => o.ObjProjTypeId);
        builder.HasMany(e => e.ScopeMaps).WithOne(s => s.ProjectType).HasForeignKey(s => s.ScopeProjTypeId);
        builder.HasMany(e => e.FunctionMaps).WithOne(f => f.ProjectType).HasForeignKey(f => f.ProjTypeFuncTypeId);
        builder.HasOne(e => e.Category).WithMany(c => c.ProjectTypes).HasForeignKey(e => e.ProjTypeCatId).HasPrincipalKey(c => c.ProjCatId);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectCategoryMasterConfiguration : IEntityTypeConfiguration<ProjectCategoryMaster>
{
    public void Configure(EntityTypeBuilder<ProjectCategoryMaster> builder)
    {
        builder.ToTable("PROJECTCAT_MASTER");
        builder.HasKey(e => e.CategoryId);
        builder.Property(e => e.CategoryId).HasColumnName("CATEGORY_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.CategoryName).HasColumnName("CATEGORY_NAME").HasMaxLength(200).IsRequired();
        builder.Property(e => e.CategoryTeamId).HasColumnName("CATEGORY_TEAMID");
        builder.Property(e => e.LastModifiedBy).HasColumnName("CATEGORY_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("CATEGORY_LASTMODIFIEDON").HasColumnType("datetime2(3)");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectTypeCategoryMasterConfiguration : IEntityTypeConfiguration<ProjectTypeCategoryMaster>
{
    public void Configure(EntityTypeBuilder<ProjectTypeCategoryMaster> builder)
    {
        builder.ToTable("PROJTYPE_CATEGORYMAST");
        builder.HasKey(e => e.ProjCatId);
        builder.Property(e => e.ProjCatId).HasColumnName("PROJCAT_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ProjCatName).HasColumnName("PROJCAT_NAME").HasMaxLength(50).IsRequired();
        builder.Property(e => e.LastModifiedBy).HasColumnName("PROJCAT_UPDATEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("PROJCAT_UPDATEDON").HasColumnType("datetime2(3)");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectTypeDeliverableMapConfiguration : IEntityTypeConfiguration<ProjectTypeDeliverableMap>
{
    public void Configure(EntityTypeBuilder<ProjectTypeDeliverableMap> builder)
    {
        builder.ToTable("PROJTYPE_DELMAP");
        builder.HasKey(e => e.DelId);
        builder.Property(e => e.DelId).HasColumnName("DEL_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.DelProjTypeId).HasColumnName("DEL_PROJTYPEID");
        builder.Property(e => e.DelDesc).HasColumnName("DEL_DESC").HasMaxLength(500).IsRequired();
        builder.Property(e => e.LastModifiedBy).HasColumnName("DEL_MODIFIEDBY").HasColumnType("decimal(38)");
        builder.Property(e => e.LastModifiedOn).HasColumnName("DEL_MODIFIEDON").HasColumnType("datetime2(3)");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectTypeObjectiveMapConfiguration : IEntityTypeConfiguration<ProjectTypeObjectiveMap>
{
    public void Configure(EntityTypeBuilder<ProjectTypeObjectiveMap> builder)
    {
        builder.ToTable("PROJTYPE_OBJMAP");
        builder.HasKey(e => e.ObjId);
        builder.Property(e => e.ObjId).HasColumnName("OBJ_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ObjProjTypeId).HasColumnName("OBJ_PROJTYPEID");
        builder.Property(e => e.ObjDesc).HasColumnName("OBJ_DESC").HasMaxLength(500).IsRequired();
        builder.Property(e => e.LastModifiedBy).HasColumnName("OBJ_MODIFIEDBY").HasColumnType("decimal(38)");
        builder.Property(e => e.LastModifiedOn).HasColumnName("OBJ_MODIFIEDON").HasColumnType("datetime2(3)");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectTypeScopeMapConfiguration : IEntityTypeConfiguration<ProjectTypeScopeMap>
{
    public void Configure(EntityTypeBuilder<ProjectTypeScopeMap> builder)
    {
        builder.ToTable("PROJTYPE_SCOPEMAP");
        builder.HasKey(e => e.ScopeId);
        builder.Property(e => e.ScopeId).HasColumnName("SCOPE_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ScopeProjTypeId).HasColumnName("SCOPE_PROJTYPEID");
        builder.Property(e => e.ScopeDesc).HasColumnName("SCOPE_DESC").HasMaxLength(500).IsRequired();
        builder.Property(e => e.LastModifiedBy).HasColumnName("SCOPE_MODIFIEDBY").HasColumnType("decimal(38)");
        builder.Property(e => e.LastModifiedOn).HasColumnName("SCOPE_MODIFIEDON").HasColumnType("datetime2(3)");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectTypeFinYearSeqConfiguration : IEntityTypeConfiguration<ProjectTypeFinYearSeq>
{
    public void Configure(EntityTypeBuilder<ProjectTypeFinYearSeq> builder)
    {
        builder.ToTable("PROJTYPE_FINYEARSEQ");
        builder.HasKey(e => e.ProjTypeId);
        builder.Property(e => e.ProjTypeId).HasColumnName("PROJTYPE_ID").ValueGeneratedNever();
        builder.Property(e => e.ProjTypeYear).HasColumnName("PROJTYPE_YEAR");
        builder.Property(e => e.ProjTypeSeq).HasColumnName("PROJTYPE_SEQ");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectDepartmentConfiguration : IEntityTypeConfiguration<ProjectDepartment>
{
    public void Configure(EntityTypeBuilder<ProjectDepartment> builder)
    {
        builder.ToTable("PROJDEP_MAST");
        builder.HasKey(e => e.ProjDepId);
        builder.Property(e => e.ProjDepId).HasColumnName("PROJDEP_ID").HasColumnType("decimal(38)");
        builder.Property(e => e.ProjDepName).HasColumnName("PROJDEP_NAME").HasMaxLength(50).IsRequired();
        builder.Property(e => e.ProjDepCode).HasColumnName("PROJDEP_CODE").HasMaxLength(50).IsRequired();
        builder.Property(e => e.LastModifiedBy).HasColumnName("PROJDEP_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("PROJDEP_LASTMODIFIEDON").HasColumnType("datetime2(3)");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectLocationConfiguration : IEntityTypeConfiguration<ProjectLocation>
{
    public void Configure(EntityTypeBuilder<ProjectLocation> builder)
    {
        builder.ToTable("PROJLOC_MAST");
        builder.HasKey(e => e.LocId);
        builder.Property(e => e.LocId).HasColumnName("LOC_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.LocName).HasColumnName("LOC_NAME").HasMaxLength(250).IsRequired();
        builder.Property(e => e.LastModifiedBy).HasColumnName("LOC_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("LOC_LASTMODIFIEDON").HasColumnType("datetime2(3)");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectProcessConfiguration : IEntityTypeConfiguration<ProjectProcess>
{
    public void Configure(EntityTypeBuilder<ProjectProcess> builder)
    {
        builder.ToTable("PROJPROC_MAST");
        builder.HasKey(e => e.ProcId);
        builder.Property(e => e.ProcId).HasColumnName("PROC_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ProcName).HasColumnName("PROC_NAME").HasMaxLength(50).IsRequired();
        builder.Property(e => e.LastModifiedBy).HasColumnName("PROC_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("PROC_LASTMODIFIEDON").HasColumnType("datetime2(3)");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectFunctionConfiguration : IEntityTypeConfiguration<ProjectFunction>
{
    public void Configure(EntityTypeBuilder<ProjectFunction> builder)
    {
        builder.ToTable("PROJFUNC_MAST");
        builder.HasKey(e => e.ProjFuncId);
        builder.Property(e => e.ProjFuncId).HasColumnName("PROJFUNC_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ProjFuncName).HasColumnName("PROJFUNC_NAME").HasMaxLength(50).IsRequired();
        builder.Property(e => e.LastModifiedBy).HasColumnName("PROJFUNC_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("PROJFUNC_LASTMODIFIEDON").HasColumnType("datetime2(3)");

        builder.HasMany(e => e.EmployeeMaps).WithOne(em => em.Function).HasForeignKey(em => em.ProjFuncEmpMapFuncId);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectFunctionEmployeeMapConfiguration : IEntityTypeConfiguration<ProjectFunctionEmployeeMap>
{
    public void Configure(EntityTypeBuilder<ProjectFunctionEmployeeMap> builder)
    {
        builder.ToTable("PROJFUNCEMP_MAP");
        builder.HasKey(e => e.ProjFuncEmpMapId);
        builder.Property(e => e.ProjFuncEmpMapId).HasColumnName("PROJFUNCEMP_MAPID").ValueGeneratedOnAdd();
        builder.Property(e => e.ProjFuncEmpMapFuncId).HasColumnName("PROJFUNCEMP_MAPFUNCID");
        builder.Property(e => e.ProjFuncEmpMapEmpSysId).HasColumnName("PROJFUNCEMP_MAPEMPSYSID");
        builder.Property(e => e.LastModifiedBy).HasColumnName("PROJFUNCEMP_UPDATEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("PROJFUNCEMP_UPDATEDON").HasColumnType("datetime2(3)");
        builder.Property(e => e.ProjFuncEmpLiveFlag).HasColumnName("PROJFUNCEMP_LIVEFLAG").HasColumnType("char(1)");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProjectTypeFunctionMapConfiguration : IEntityTypeConfiguration<ProjectTypeFunctionMap>
{
    public void Configure(EntityTypeBuilder<ProjectTypeFunctionMap> builder)
    {
        builder.ToTable("PROJTYPEFUNC_MAP");
        builder.HasKey(e => e.ProjTypeFuncMapId);
        builder.Property(e => e.ProjTypeFuncMapId).HasColumnName("PROJTYPEFUNC_MAPID").ValueGeneratedOnAdd();
        builder.Property(e => e.ProjTypeFuncTypeId).HasColumnName("PROJTYPEFUNC_TYPEID");
        builder.Property(e => e.ProjTypeFuncFuncId).HasColumnName("PROJTYPEFUNC_FUNCID");
        builder.Property(e => e.ProjTypeFuncAddlNo).HasColumnName("PROJTYPEFUNC_ADDLNO");
        builder.Property(e => e.LastModifiedBy).HasColumnName("PROJTYPEFUNC_UPDATEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("PROJTYPEFUNC_UPDATEDON").HasColumnType("datetime2(3)");

        builder.HasOne(e => e.Function).WithMany().HasForeignKey(e => e.ProjTypeFuncFuncId);
        builder.Ignore(e => e.DomainEvents);
    }
}
