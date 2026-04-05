using MediatR;
using Microsoft.EntityFrameworkCore;
using TimeSheetService.Domain.Common;
using TimeSheetService.Domain.Entities;
using TimeSheetService.Domain.ValueObjects;

namespace TimeSheetService.Infrastructure.Persistence;

public class TimeSheetDbContext : DbContext
{
    private readonly IMediator _mediator;

    public DbSet<TimesheetEntry> TimesheetEntries => Set<TimesheetEntry>();
    public DbSet<TimesheetDetail> TimesheetDetails => Set<TimesheetDetail>();
    public DbSet<TcTimesheetEntry> TcTimesheetEntries => Set<TcTimesheetEntry>();
    public DbSet<TcTimesheetDetail> TcTimesheetDetails => Set<TcTimesheetDetail>();
    public DbSet<TcProject> TcProjects => Set<TcProject>();
    public DbSet<TcProjectCategory> TcProjectCategories => Set<TcProjectCategory>();
    public DbSet<TcSubCategory> TcSubCategories => Set<TcSubCategory>();
    public DbSet<TcSubCategoryEmpMap> TcSubCategoryEmpMaps => Set<TcSubCategoryEmpMap>();
    public DbSet<TsProject> TsProjects => Set<TsProject>();
    public DbSet<TsStage> TsStages => Set<TsStage>();
    public DbSet<TsStageEmpMap> TsStageEmpMaps => Set<TsStageEmpMap>();
    public DbSet<TsTimesheetDetail> TsTimesheetDetails => Set<TsTimesheetDetail>();
    public DbSet<TsActivity> TsActivities => Set<TsActivity>();

    public TimeSheetDbContext(DbContextOptions<TimeSheetDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // TIMESHEET_MAIN
        modelBuilder.Entity<TimesheetEntry>(entity =>
        {
            entity.ToTable("TIMESHEET_MAIN");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("TIME_ID").ValueGeneratedOnAdd();
            entity.Property(e => e.EmployeeSysId).HasColumnName("TIME_EMPSYSID").IsRequired();
            entity.Property(e => e.TimeDate).HasColumnName("TIME_DATE").HasColumnType("datetime2(3)").IsRequired();
            entity.Property(e => e.TimeIn).HasColumnName("TIME_IN").HasColumnType("datetime2(3)");
            entity.Property(e => e.TimeOut).HasColumnName("TIME_OUT").HasColumnType("datetime2(3)");
            entity.Property(e => e.TotalHours).HasColumnName("TIME_HOURS").IsRequired();
            entity.Property(e => e.Remarks).HasColumnName("TIME_REMARKS").HasColumnType("varchar(500)").HasMaxLength(500);
            entity.Property(e => e.EntryType).HasColumnName("TIME_ENTRYTYPE")
                .HasColumnType("char(1)").HasMaxLength(1).IsRequired()
                .HasConversion(v => v.Code.ToString(), v => EntryType.FromCode(v[0]));
            entity.Property(e => e.LastModifiedBy).HasColumnName("TIME_LASTMODIFIEDBY").IsRequired();
            entity.Property(e => e.LastModifiedOn).HasColumnName("TIME_LASTMODIFIEDON").HasColumnType("datetime2(3)").IsRequired();
            entity.HasMany(e => e.Details).WithOne()
                .HasForeignKey("TimeId").HasConstraintName("FK_TIMESHEET_DET_MAIN").OnDelete(DeleteBehavior.Cascade);
            entity.Ignore(e => e.DomainEvents);
        });

        // TIMESHEET_DET
        modelBuilder.Entity<TimesheetDetail>(entity =>
        {
            entity.ToTable("TIMESHEET_DET");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("TIMEDET_ID").ValueGeneratedOnAdd();
            entity.Property<long>("TimeId").HasColumnName("TIMEDET_TIMEID").IsRequired();
            entity.Property(e => e.Hours).HasColumnName("TIMEDET_HOURS").IsRequired();
            entity.Property(e => e.ProjectId).HasColumnName("TIMEDET_PROJECTID").IsRequired();
            entity.Property(e => e.SubCategoryId).HasColumnName("TIMEDET_SUBCATID").IsRequired();
            entity.Property(e => e.Remarks).HasColumnName("TIMEDET_REMARKS").HasColumnType("varchar(500)").HasMaxLength(500);
            entity.Property(e => e.CallNo).HasColumnName("TIMEDET_CALLNO");
            entity.Property(e => e.LastModifiedBy).HasColumnName("TIMEDET_LASTMODIFIEDBY").IsRequired();
            entity.Property(e => e.LastModifiedOn).HasColumnName("TIMEDET_LASTMODIFIEDON").HasColumnType("datetime2(3)").IsRequired();
            entity.Ignore(e => e.DomainEvents);
        });

        // TCTIMESHEET_MAIN
        modelBuilder.Entity<TcTimesheetEntry>(entity =>
        {
            entity.ToTable("TCTIMESHEET_MAIN");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("TIME_ID").ValueGeneratedOnAdd();
            entity.Property(e => e.EmployeeSysId).HasColumnName("TIME_EMPSYSID").IsRequired();
            entity.Property(e => e.TimeDate).HasColumnName("TIME_DATE").HasColumnType("datetime2(3)").IsRequired();
            entity.Property(e => e.TimeIn).HasColumnName("TIME_IN").HasColumnType("datetime2(3)");
            entity.Property(e => e.TimeOut).HasColumnName("TIME_OUT").HasColumnType("datetime2(3)");
            entity.Property(e => e.TotalHours).HasColumnName("TIME_HOURS").IsRequired();
            entity.Property(e => e.Remarks).HasColumnName("TIME_REMARKS").HasColumnType("varchar(500)").HasMaxLength(500);
            entity.Property(e => e.EntryType).HasColumnName("TIME_ENTRYTYPE")
                .HasColumnType("char(1)").HasMaxLength(1).IsRequired()
                .HasConversion(v => v.Code.ToString(), v => EntryType.FromCode(v[0]));
            entity.Property(e => e.LastModifiedBy).HasColumnName("TIME_LASTMODIFIEDBY").IsRequired();
            entity.Property(e => e.LastModifiedOn).HasColumnName("TIME_LASTMODIFIEDON").HasColumnType("datetime2(3)").IsRequired();
            entity.HasMany(e => e.Details).WithOne()
                .HasForeignKey("TimeId").HasConstraintName("FK_TCTIMESHEET_DET_MAIN").OnDelete(DeleteBehavior.Cascade);
            entity.Ignore(e => e.DomainEvents);
        });

        // TCTIMESHEET_DET
        modelBuilder.Entity<TcTimesheetDetail>(entity =>
        {
            entity.ToTable("TCTIMESHEET_DET");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("TIMEDET_ID").ValueGeneratedOnAdd();
            entity.Property<long>("TimeId").HasColumnName("TIMEDET_TIMEID").IsRequired();
            entity.Property(e => e.Hours).HasColumnName("TIMEDET_HOURS").IsRequired();
            entity.Property(e => e.ProjectId).HasColumnName("TIMEDET_PROJECTID").IsRequired();
            entity.Property(e => e.SubCategoryId).HasColumnName("TIMEDET_SUBCATID").IsRequired();
            entity.Property(e => e.Remarks).HasColumnName("TIMEDET_REMARKS").HasColumnType("varchar(500)").HasMaxLength(500);
            entity.Property(e => e.CallNo).HasColumnName("TIMEDET_CALLNO");
            entity.Property(e => e.LastModifiedBy).HasColumnName("TIMEDET_LASTMODIFIEDBY").IsRequired();
            entity.Property(e => e.LastModifiedOn).HasColumnName("TIMEDET_LASTMODIFIEDON").HasColumnType("datetime2(3)").IsRequired();
            entity.Ignore(e => e.DomainEvents);
        });

        // TCPROJECT_MASTER
        modelBuilder.Entity<TcProject>(entity =>
        {
            entity.ToTable("TCPROJECT_MASTER");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("PROJECT_ID").ValueGeneratedOnAdd();
            entity.Property(e => e.ProjectName).HasColumnName("PROJECT_NAME").HasColumnType("varchar(200)").HasMaxLength(200).IsRequired();
            entity.Property(e => e.CategoryId).HasColumnName("PROJECT_CATEGORYID").IsRequired();
            entity.Property(e => e.EffectiveDate).HasColumnName("PROJECT_EFFDATE").HasColumnType("datetime2(3)").IsRequired();
            entity.Property(e => e.CloseDate).HasColumnName("PROJECT_CLSDATE").HasColumnType("datetime2(3)");
            entity.Property(e => e.TeamId).HasColumnName("PROJECT_TEAMID").IsRequired();
            entity.Property(e => e.ListAll).HasColumnName("PROJECT_LISTALL").HasColumnType("char(1)").HasMaxLength(1).IsRequired();
            entity.Property(e => e.LastModifiedBy).HasColumnName("PROJECT_LASTMODIFIEDBY").IsRequired();
            entity.Property(e => e.LastModifiedOn).HasColumnName("PROJECT_LASTMODIFIEDON").HasColumnType("datetime2(3)").IsRequired();
            entity.Property(e => e.OldProjectId).HasColumnName("PROJECT_OLDPROJID");
            entity.Ignore(e => e.DomainEvents);
        });

        // TCPROJECTCAT_MASTER
        modelBuilder.Entity<TcProjectCategory>(entity =>
        {
            entity.ToTable("TCPROJECTCAT_MASTER");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("CATEGORY_ID").ValueGeneratedOnAdd();
            entity.Property(e => e.CategoryName).HasColumnName("CATEGORY_NAME").HasColumnType("varchar(200)").HasMaxLength(200).IsRequired();
            entity.Property(e => e.TeamId).HasColumnName("CATEGORY_TEAMID").IsRequired();
            entity.Property(e => e.LastModifiedBy).HasColumnName("CATEGORY_LASTMODIFIEDBY").IsRequired();
            entity.Property(e => e.LastModifiedOn).HasColumnName("CATEGORY_LASTMODIFIEDON").HasColumnType("datetime2(3)").IsRequired();
            entity.Property(e => e.OldCategoryId).HasColumnName("CATEGORY_OLDCATID");
            entity.Ignore(e => e.DomainEvents);
        });

        // TCSUBCAT_MASTER
        modelBuilder.Entity<TcSubCategory>(entity =>
        {
            entity.ToTable("TCSUBCAT_MASTER");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("SUBCAT_ID").ValueGeneratedOnAdd();
            entity.Property(e => e.SubCategoryName).HasColumnName("SUBCAT_NAME").HasColumnType("varchar(200)").HasMaxLength(200).IsRequired();
            entity.Property(e => e.ProjectId).HasColumnName("SUBCAT_PROJECTID").IsRequired();
            entity.Property(e => e.LastModifiedBy).HasColumnName("SUBCAT_LASTMODIFIEDBY").IsRequired();
            entity.Property(e => e.LastModifiedOn).HasColumnName("SUBCAT_LASTMODIFIEDON").HasColumnType("datetime2(3)").IsRequired();
            entity.Property(e => e.OldSubCategoryId).HasColumnName("SUBCAT_OLDSUBCATID");
            entity.Ignore(e => e.DomainEvents);
        });

        // TCSUBCAT_EMPMAP
        modelBuilder.Entity<TcSubCategoryEmpMap>(entity =>
        {
            entity.ToTable("TCSUBCAT_EMPMAP");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("SUBCAT_MAPID").ValueGeneratedOnAdd();
            entity.Property(e => e.SubCategoryId).HasColumnName("SUBCAT_ID").IsRequired();
            entity.Property(e => e.EmployeeSysId).HasColumnName("SUBCAT_EMPSYSID").IsRequired();
            entity.Property(e => e.StartDate).HasColumnName("SUBCAT_STARTDATE").HasColumnType("datetime2(3)").IsRequired();
            entity.Property(e => e.EndDate).HasColumnName("SUBCAT_ENDDATE").HasColumnType("datetime2(3)");
            entity.Property(e => e.PlannedEndDate).HasColumnName("SUBCAT_PLANNEDENDDATE").HasColumnType("datetime2(3)");
            entity.Property(e => e.PlannedHours).HasColumnName("SUBCAT_PLANNEDHRS");
            entity.Property(e => e.LastModifiedBy).HasColumnName("SUBCAT_LASTMODIFIEDBY").IsRequired();
            entity.Property(e => e.LastModifiedOn).HasColumnName("SUBCAT_LASTMODIFIEDON").HasColumnType("datetime2(3)").IsRequired();
            entity.Ignore(e => e.DomainEvents);
        });

        // TSPROJECT_MASTER — varchar PK, no BaseEntity.Id
        modelBuilder.Entity<TsProject>(entity =>
        {
            entity.ToTable("TSPROJECT_MASTER");
            entity.HasKey(e => e.ProjectCode);
            entity.Ignore(e => e.Id);
            entity.Property(e => e.ProjectCode).HasColumnName("PROJECT_ID").HasColumnType("varchar(50)").HasMaxLength(50).IsRequired();
            entity.Property(e => e.ProjectGroup).HasColumnName("PROJECT_GROUP").HasColumnType("varchar(100)").HasMaxLength(100);
            entity.Property(e => e.ProjectName).HasColumnName("PROJECT_NAME").HasColumnType("varchar(200)").HasMaxLength(200).IsRequired();
            entity.Property(e => e.EffectiveDate).HasColumnName("PROJECT_EFFDATE").HasColumnType("datetime2(3)").IsRequired();
            entity.Property(e => e.CloseDate).HasColumnName("PROJECT_CLSDATE").HasColumnType("datetime2(3)");
            entity.Property(e => e.ProjectType).HasColumnName("PROJECT_TYPE").HasColumnType("char(1)").HasMaxLength(1);
            entity.Property(e => e.AppId).HasColumnName("PROJECT_APPID");
            entity.Property(e => e.ApplyAll).HasColumnName("PROJECT_APPLYALL").HasColumnType("char(1)").HasMaxLength(1);
            entity.Property(e => e.LastModifiedBy).HasColumnName("PROJECT_LASTMODIFIEDBY").IsRequired();
            entity.Property(e => e.LastModifiedOn).HasColumnName("PROJECT_LASTMODIFIEDON").HasColumnType("datetime2(3)").IsRequired();
            entity.Ignore(e => e.DomainEvents);
        });

        // TSSTAGE_MASTER — varchar PK
        modelBuilder.Entity<TsStage>(entity =>
        {
            entity.ToTable("TSSTAGE_MASTER");
            entity.HasKey(e => e.StageCode);
            entity.Ignore(e => e.Id);
            entity.Property(e => e.StageCode).HasColumnName("STAGE_ID").HasColumnType("varchar(50)").HasMaxLength(50).IsRequired();
            entity.Property(e => e.StageName).HasColumnName("STAGE_NAME").HasColumnType("varchar(200)").HasMaxLength(200).IsRequired();
            entity.Property(e => e.ProjectCode).HasColumnName("STAGE_PROJECTID").HasColumnType("varchar(50)").HasMaxLength(50).IsRequired();
            entity.Property(e => e.LastModifiedBy).HasColumnName("STAGE_LASTMODIFIEDBY").IsRequired();
            entity.Property(e => e.LastModifiedOn).HasColumnName("STAGE_LASTMODIFIEDON").HasColumnType("datetime2(3)").IsRequired();
            entity.Ignore(e => e.DomainEvents);
        });

        // TSSTAGE_EMPMAP
        modelBuilder.Entity<TsStageEmpMap>(entity =>
        {
            entity.ToTable("TSSTAGE_EMPMAP");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("STMAP_ID").ValueGeneratedOnAdd();
            entity.Property(e => e.StageId).HasColumnName("STMAP_STAGEID").IsRequired();
            entity.Property(e => e.EmployeeSysId).HasColumnName("STMAP_EMPSYSID").IsRequired();
            entity.Property(e => e.BudgetedHours).HasColumnName("STMAP_HOURS");
            entity.Property(e => e.StartDate).HasColumnName("STMAP_STARTDATE").HasColumnType("datetime2(3)").IsRequired();
            entity.Property(e => e.PlannedEndDate).HasColumnName("STMAP_PLANNEDENDDATE").HasColumnType("datetime2(3)");
            entity.Property(e => e.ClosureDate).HasColumnName("STMAP_CLOSUREDATE").HasColumnType("datetime2(3)");
            entity.Property(e => e.LastModifiedBy).HasColumnName("STMAP_LASTMODIFIEDBY").IsRequired();
            entity.Property(e => e.LastModifiedOn).HasColumnName("STMAP_LASTMODIFIEDON").HasColumnType("datetime2(3)").IsRequired();
            entity.Ignore(e => e.DomainEvents);
        });

        // TSTIMESHEET_DET
        modelBuilder.Entity<TsTimesheetDetail>(entity =>
        {
            entity.ToTable("TSTIMESHEET_DET");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("TIME_ID").ValueGeneratedOnAdd();
            entity.Property(e => e.EmployeeSysId).HasColumnName("TIME_EMPSYSID").IsRequired();
            entity.Property(e => e.TimeDate).HasColumnName("TIME_DATE").HasColumnType("datetime2(3)").IsRequired();
            entity.Property(e => e.ProjectId).HasColumnName("TIME_PROJECTID").IsRequired();
            entity.Property(e => e.StageId).HasColumnName("TIME_STAGEID").IsRequired();
            entity.Property(e => e.ActivityId).HasColumnName("TIME_ACTIVITYID").IsRequired();
            entity.Property(e => e.Hours).HasColumnName("TIME_HOURS").IsRequired();
            entity.Property(e => e.Remarks).HasColumnName("TIME_REMARKS").HasColumnType("varchar(500)").HasMaxLength(500);
            entity.Property(e => e.ModuleId).HasColumnName("TIME_MODULEID");
            entity.Property(e => e.RefId).HasColumnName("TIME_REFID").HasColumnType("varchar(100)").HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasColumnName("TIME_CREATEDBY").IsRequired();
            entity.Property(e => e.CreatedOn).HasColumnName("TIME_CREATEDON").HasColumnType("datetime2(3)").IsRequired();
            entity.Property(e => e.LastModifiedBy).HasColumnName("TIME_MODIFIEDBY").IsRequired();
            entity.Property(e => e.LastModifiedOn).HasColumnName("TIME_MODIFIEDON").HasColumnType("datetime2(3)").IsRequired();
            entity.Ignore(e => e.DomainEvents);
        });

        // TSACTIVITY_MASTER
        modelBuilder.Entity<TsActivity>(entity =>
        {
            entity.ToTable("TSACTIVITY_MASTER");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ACTIVITY_ID").ValueGeneratedOnAdd();
            entity.Property(e => e.ActivityName).HasColumnName("ACTIVITY_NAME").HasColumnType("varchar(200)").HasMaxLength(200).IsRequired();
            entity.Property(e => e.ActivityRole).HasColumnName("ACTIVITY_ROLE").HasColumnType("varchar(200)").HasMaxLength(200);
            entity.Property(e => e.LastModifiedBy).HasColumnName("ACTIVITY_LASTMODIFIEDBY").IsRequired();
            entity.Property(e => e.LastModifiedOn).HasColumnName("ACTIVITY_LASTMODIFIEDON").HasColumnType("datetime2(3)").IsRequired();
            entity.Ignore(e => e.DomainEvents);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);

        return result;
    }
}
