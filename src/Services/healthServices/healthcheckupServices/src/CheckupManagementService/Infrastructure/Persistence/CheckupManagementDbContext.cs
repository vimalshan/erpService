namespace CheckupManagementService.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using CheckupManagementService.Domain.Entities;
using Shared.Core.Repositories;
using System.Linq.Expressions;

/// <summary>
/// DbContext for Checkup Management Service
/// </summary>
public class CheckupManagementDbContext : DbContext
{
    public CheckupManagementDbContext(DbContextOptions<CheckupManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<FieldTypeMaster> FieldTypeMasters { get; set; } = null!;
    public DbSet<CheckupSymptomMaster> CheckupSymptomMasters { get; set; } = null!;
    public DbSet<TestMaster> TestMasters { get; set; } = null!;
    public DbSet<CheckupMaster> CheckupMasters { get; set; } = null!;
    public DbSet<CheckupOthers> CheckupOthers { get; set; } = null!;
    public DbSet<CheckupOthersListOfValues> CheckupOthersListOfValues { get; set; } = null!;
    public DbSet<CheckupTest> CheckupTests { get; set; } = null!;
    public DbSet<CheckupPersonalFamilyHistory> CheckupPersonalFamilyHistories { get; set; } = null!;
    public DbSet<HealthCounter> HealthCounters { get; set; } = null!;
    public DbSet<HealthMinMaxValue> HealthMinMaxValues { get; set; } = null!;
    public DbSet<HealthEntryLov> HealthEntryLovs { get; set; } = null!;
    public DbSet<HealthMain> HealthMains { get; set; } = null!;
    public DbSet<HealthSub> HealthSubs { get; set; } = null!;
    public DbSet<HealthDynamicDetail> HealthDynamicDetails { get; set; } = null!;
    public DbSet<PreEmploymentCheckupMain> PreEmploymentCheckupMains { get; set; } = null!;
    public DbSet<HealthCheckCard> HealthCheckCards { get; set; } = null!;
    public DbSet<HealthCheckCardSub> HealthCheckCardSubs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ==================== MASTER RECORDS ====================
        
        // Field Type Master
        modelBuilder.Entity<FieldTypeMaster>(entity =>
        {
            entity.HasKey(e => e.FieldTypeCode);
            entity.Property(e => e.FieldTypeName).HasMaxLength(30);
            entity.Property(e => e.ControlSource).HasMaxLength(50);
            entity.ToTable("FIELD_TYP_MAST");
        });

        // Checkup Symptom Master
        modelBuilder.Entity<CheckupSymptomMaster>(entity =>
        {
            entity.HasKey(e => e.SymptomId);
            entity.Property(e => e.SymptomName).HasMaxLength(50);
            entity.Property(e => e.SymptomFlag).HasMaxLength(3);
            entity.ToTable("CHKUP_SYMP_MAST");
        });

        // Test Master
        modelBuilder.Entity<TestMaster>(entity =>
        {
            entity.HasKey(e => e.TestCode);
            entity.Property(e => e.TestName).HasMaxLength(50).IsRequired();
            entity.Property(e => e.CheckboxFlag).HasColumnType("char(1)");
            entity.Property(e => e.CloseFlag).HasColumnType("char(1)");
            entity.Property(e => e.RangeValue).HasMaxLength(100);
            entity.Property(e => e.TestGroup).HasMaxLength(100);
            entity.ToTable("TEST_MAST");
        });

        // Checkup Master
        modelBuilder.Entity<CheckupMaster>(entity =>
        {
            entity.HasKey(e => new { e.CompanyCode, e.CheckupCode });
            entity.Property(e => e.CompanyCode).HasMaxLength(3).IsRequired();
            entity.Property(e => e.CheckupCode).IsRequired();
            entity.Property(e => e.CheckupName).HasMaxLength(50).IsRequired();
            entity.Property(e => e.CloseDate).HasMaxLength(255);
            entity.Property(e => e.Flag).HasColumnType("char(1)");

            entity.HasIndex(e => e.CheckupCode).HasName("IDX_CHECKUP_MAST_CM_CHK_COD");

            entity.HasMany(e => e.CheckupTests)
                .WithOne(e => e.CheckupMaster)
                .HasForeignKey(e => new { e.CompanyCode, e.CheckupCode })
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.HealthMains)
                .WithOne(e => e.CheckupMaster)
                .HasForeignKey(e => new { e.CompanyCode, e.CheckupCode })
                .OnDelete(DeleteBehavior.Restrict);

            entity.ToTable("CHECKUP_MAST");
        });

        // ==================== CHECKUP RELATED ====================

        // Checkup Others
        modelBuilder.Entity<CheckupOthers>(entity =>
        {
            entity.HasKey(e => new { e.CompanyCode, e.CheckupCode, e.OtherSerialNumber });
            entity.Property(e => e.CompanyCode).HasMaxLength(3);
            entity.Property(e => e.FieldLabel).HasMaxLength(200);
            entity.Property(e => e.MandatoryFlag).HasColumnType("char(1)");
            entity.Property(e => e.FieldTypeName).HasMaxLength(50);

            entity.HasOne(e => e.FieldType)
                .WithMany(e => e.CheckupOthers)
                .HasForeignKey(e => e.FieldTypeCode)
                .OnDelete(DeleteBehavior.SetNull);

            entity.ToTable("CHKUP_OTHERS");
        });

        // Checkup Others List of Values
        modelBuilder.Entity<CheckupOthersListOfValues>(entity =>
        {
            entity.HasKey(e => e.ListOfValueSerialNumber);
            entity.Property(e => e.CompanyCode).HasMaxLength(10);
            entity.Property(e => e.ListOfValueDescription).HasMaxLength(50);
            entity.ToTable("CHKUP_OTHERS_LOV");
        });

        // Checkup Test
        modelBuilder.Entity<CheckupTest>(entity =>
        {
            entity.HasKey(e => e.SerialNumber);
            entity.Property(e => e.CompanyCode).HasMaxLength(10);
            entity.Property(e => e.CheckboxFlag).HasColumnType("char(1)");
            entity.Property(e => e.CloseFlag).HasColumnType("char(1)");

            entity.HasOne(e => e.TestMaster)
                .WithMany(e => e.CheckupTests)
                .HasForeignKey(e => e.TestCode)
                .OnDelete(DeleteBehavior.SetNull);

            entity.ToTable("CHKUP_TEST");
        });

        // Checkup Personal & Family History
        modelBuilder.Entity<CheckupPersonalFamilyHistory>(entity =>
        {
            entity.HasKey(e => new { e.HealthNumber, e.EmployeeNumber, e.SymptomId });
            entity.Property(e => e.YesNoFlag).HasColumnType("char(1)");

            entity.HasOne(e => e.Symptom)
                .WithMany(e => e.PersonalFamilyHistories)
                .HasForeignKey(e => e.SymptomId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.ToTable("CHKUP_PFI_HIST");
        });

        // ==================== HEALTH RECORDS ====================

        // Health Counter
        modelBuilder.Entity<HealthCounter>(entity =>
        {
            entity.HasKey(e => new { e.CompanyCode, e.CounterCode });
            entity.Property(e => e.CompanyCode).HasMaxLength(3).IsRequired();
            entity.Property(e => e.CounterCode).HasMaxLength(3).IsRequired();
            entity.ToTable("HEALTH_COUNTER");
        });

        // Health Min/Max Value
        modelBuilder.Entity<HealthMinMaxValue>(entity =>
        {
            entity.HasKey(e => new { e.TestCode, e.TypeCode, e.UnitCode });
            entity.Property(e => e.TypeCode).HasMaxLength(2);
            entity.Property(e => e.UnitCode).HasMaxLength(10);
            entity.Property(e => e.MinText).HasMaxLength(20);
            entity.Property(e => e.MaxText).HasMaxLength(20);
            entity.Property(e => e.LovText).HasMaxLength(20);

            entity.HasOne(e => e.TestMaster)
                .WithMany(e => e.HealthMinMaxValues)
                .HasForeignKey(e => e.TestCode)
                .OnDelete(DeleteBehavior.Cascade);

            entity.ToTable("HEALTH_MINMAX_VAL");
        });

        // Health Entry List of Values
        modelBuilder.Entity<HealthEntryLov>(entity =>
        {
            entity.HasKey(e => new { e.TestCode, e.ListOfValueText });
            entity.Property(e => e.ListOfValueText).HasMaxLength(50);

            entity.HasOne(e => e.TestMaster)
                .WithMany(e => e.HealthEntryLovs)
                .HasForeignKey(e => e.TestCode)
                .OnDelete(DeleteBehavior.Cascade);

            entity.ToTable("HEALTH_ENTRY_LOV");
        });

        // Health Main
        modelBuilder.Entity<HealthMain>(entity =>
        {
            entity.HasKey(e => e.HealthNumber);
            entity.Property(e => e.EmployeeNumber).HasMaxLength(10).IsRequired();
            entity.Property(e => e.CompanyCode).HasMaxLength(10).IsRequired();
            entity.Property(e => e.CheckupDate).HasMaxLength(100);
            entity.Property(e => e.EntryEmployeeNumber).HasMaxLength(10).IsRequired();
            entity.Property(e => e.CheckupCode).IsRequired();
            entity.Property(e => e.TextField2).HasMaxLength(10);
            entity.Property(e => e.TextField3).HasMaxLength(10);
            entity.Property(e => e.TextField4).HasMaxLength(10);
            entity.Property(e => e.TextField5).HasMaxLength(10);

            entity.HasIndex(e => e.EmployeeNumber).HasName("IDX_HEALTH_MAIN_HM_EMP_NUM");
            entity.HasIndex(e => e.HealthNumber).HasName("IDX_HEALTH_MAIN_HM_HLT_NUM");

            entity.HasOne(e => e.CheckupMaster)
                .WithMany(e => e.HealthMains)
                .HasForeignKey(e => new { e.CompanyCode, e.CheckupCode })
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.HealthSubs)
                .WithOne(e => e.HealthMain)
                .HasForeignKey(e => e.HealthNumber)
                .OnDelete(DeleteBehavior.Cascade);

            entity.ToTable("HEALTH_MAIN");
        });

        // Health Sub
        modelBuilder.Entity<HealthSub>(entity =>
        {
            entity.HasKey(e => new { e.HealthNumber, e.TestCode });
            entity.Property(e => e.TestCode).HasMaxLength(10);
            entity.Property(e => e.TestType).HasMaxLength(50);
            entity.Property(e => e.TestValue).HasMaxLength(50);
            entity.Property(e => e.EmployeeNumber).HasMaxLength(20).IsRequired();
            entity.Property(e => e.TestRemarks).HasMaxLength(200);
            entity.Property(e => e.ValidationFlag).HasColumnType("char(1)");
            entity.Property(e => e.TextField2).HasMaxLength(255);
            entity.Property(e => e.TextField3).HasMaxLength(255);
            entity.Property(e => e.TextField4).HasMaxLength(255);
            entity.Property(e => e.TextField5).HasMaxLength(255);
            entity.Property(e => e.DoctorRemarks).HasMaxLength(255);

            entity.HasIndex(e => e.HealthNumber).HasName("IDX_HEALTH_SUB_HM_HLT_NUM");

            entity.ToTable("HEALTH_SUB");
        });

        // Health Dynamic Details
        modelBuilder.Entity<HealthDynamicDetail>(entity =>
        {
            entity.HasNoKey();
            entity.Property(e => e.CompanyCode).HasMaxLength(3);
            entity.Property(e => e.DynamicValue).HasMaxLength(100);
            entity.ToTable("HEALTH_DYN_DET");
        });

        // ==================== PRE-EMPLOYMENT & CARDS ====================

        // Pre-Employment Checkup Main
        modelBuilder.Entity<PreEmploymentCheckupMain>(entity =>
        {
            entity.HasNoKey();
            entity.Property(e => e.CompanyCode).HasMaxLength(3);
            entity.Property(e => e.PhysicalHandicapDescription).HasMaxLength(150);
            entity.Property(e => e.ProposedDesignation).HasMaxLength(30);
            entity.Property(e => e.IdentificationMarks).HasMaxLength(30);
            entity.Property(e => e.FinalRemarks).HasMaxLength(15);
            entity.Property(e => e.FitPhysical).HasMaxLength(3);
            entity.Property(e => e.FitFinal).HasMaxLength(6);
            entity.ToTable("CHKUP_PRE_MAIN");
        });

        // Health Check Card
        modelBuilder.Entity<HealthCheckCard>(entity =>
        {
            entity.HasNoKey();
            entity.Property(e => e.CompanyCode).HasMaxLength(3);
            entity.Property(e => e.PersonalDetails).HasMaxLength(200);
            entity.Property(e => e.ScreeningDetails).HasMaxLength(150);
            entity.Property(e => e.AdviceRemark1).HasMaxLength(150);
            entity.Property(e => e.AdviceFollowup1).HasMaxLength(150);
            entity.Property(e => e.AdviceRemark2).HasMaxLength(150);
            entity.Property(e => e.AdviceFollowup2).HasMaxLength(150);
            entity.ToTable("HLTH_CHKUP_CARD");
        });

        // Health Check Card Sub
        modelBuilder.Entity<HealthCheckCardSub>(entity =>
        {
            entity.HasNoKey();
            entity.Property(e => e.FlagYesNo).HasMaxLength(30);
            entity.Property(e => e.SymptomValue).HasMaxLength(150);

            entity.ToTable("HLTH_CHKCARD_SUB");
        });

    }
}

/// <summary>
/// Generic Repository for CRUD operations
/// </summary>
public class Repository<T, TKey> : IRepository<T, TKey> where T : class where TKey : notnull
{
    private readonly CheckupManagementDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(CheckupManagementDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> FindAsync(
        Func<T, bool> predicate,
        CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(_dbSet.Where(predicate).ToList());
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
