using Microsoft.EntityFrameworkCore;
using AccidentManagementService.Domain.Entities;
using AccidentManagementService.Infrastructure.Persistence.Configuration;

namespace AccidentManagementService.Infrastructure.Persistence
{
    /// <summary>
    /// Generic repository interface
    /// </summary>
    public interface IRepository<T, TKey> where T : class
    {
        Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate, CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public class AccidentManagementDbContext : DbContext
    {
        public AccidentManagementDbContext(DbContextOptions<AccidentManagementDbContext> options)
            : base(options)
        {
        }

        #region Legacy DbSets (maintain backward compatibility)
        public DbSet<DailyAccidentFIR> DailyAccidentFIRs { get; set; } = null!;
        public DbSet<AccidentContractorList> AccidentContractors { get; set; } = null!;
        public DbSet<PersonalInjury> PersonalInjuries { get; set; } = null!;
        public DbSet<InjuryCategory> InjuryCategories { get; set; } = null!;
        public DbSet<NatureOfInjury> NaturesOfInjury { get; set; } = null!;
        public DbSet<DoctorAttendant> DoctorAttendants { get; set; } = null!;
        #endregion

        #region Domain Model DbSets
        /// <summary>
        /// Main accident report aggregate root
        /// </summary>
        public DbSet<AccidentReport> AccidentReports { get; set; } = null!;

        /// <summary>
        /// Reference table for accident severity levels
        /// </summary>
        public DbSet<AccidentSeverity> AccidentSeverities { get; set; } = null!;

        /// <summary>
        /// Reference table for accident status types
        /// </summary>
        public DbSet<AccidentStatus> AccidentStatuses { get; set; } = null!;

        /// <summary>
        /// Reference table for injury categories (enhanced)
        /// </summary>
        public DbSet<InjuryCategory> DomainInjuryCategories { get; set; } = null!;

        /// <summary>
        /// Reference table for injury nature types (enhanced)
        /// </summary>
        public DbSet<InjuryNature> DomainInjuryNatures { get; set; } = null!;

        /// <summary>
        /// Reference table for contractors (enhanced)
        /// </summary>
        public DbSet<Contractor> DomainContractors { get; set; } = null!;

        /// <summary>
        /// Reference table for injured persons (enhanced)
        /// </summary>
        public DbSet<InjuredPerson> DomainInjuredPersons { get; set; } = null!;
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all domain entity configurations
            ApplyDomainConfigurations(modelBuilder);

            // Keep legacy configurations for backward compatibility
            ApplyLegacyConfigurations(modelBuilder);
        }

        /// <summary>
        /// Apply configurations for new domain entities
        /// </summary>
        private void ApplyDomainConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccidentSeverityConfiguration());
            modelBuilder.ApplyConfiguration(new AccidentStatusConfiguration());
            modelBuilder.ApplyConfiguration(new InjuryCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new InjuryNatureConfiguration());
            modelBuilder.ApplyConfiguration(new ContractorConfiguration());
            modelBuilder.ApplyConfiguration(new InjuredPersonConfiguration());
            modelBuilder.ApplyConfiguration(new AccidentReportConfiguration());
        }

        /// <summary>
        /// Apply configurations for legacy entities (maintain backward compatibility)
        /// </summary>
        private void ApplyLegacyConfigurations(ModelBuilder modelBuilder)
        {
            // DailyAccidentFIR configuration
            modelBuilder.Entity<DailyAccidentFIR>(entity =>
            {
                entity.HasKey(e => e.AccidentNumber);
                entity.Property(e => e.AccidentNumber).ValueGeneratedNever();
                entity.Property(e => e.EmployeeNumber).HasMaxLength(20);
                entity.Property(e => e.EmployeeName).HasMaxLength(50);
                entity.Property(e => e.WorkerName).HasMaxLength(50);
                entity.Property(e => e.ContractorId).HasMaxLength(50);
                entity.Property(e => e.ContractorName).HasMaxLength(50);
                entity.Property(e => e.EmployeeDepartment).HasMaxLength(50);
                entity.Property(e => e.AccidentLocation).HasMaxLength(100);
                entity.Property(e => e.NatureOfInjury).HasMaxLength(100);
                entity.Property(e => e.BodyPartAffected).HasMaxLength(100);
                entity.Property(e => e.ShiftName).HasMaxLength(255);
                entity.Property(e => e.MedicalCentreName).HasMaxLength(50);
                entity.Property(e => e.TreatmentGiven).HasMaxLength(200);
                entity.Property(e => e.CompanyCode).HasMaxLength(3);
                entity.Property(e => e.EnteredUserID).HasMaxLength(100);
                entity.Property(e => e.PreventiveMeasures).HasMaxLength(200);
                entity.Property(e => e.CauseOfIncident).HasMaxLength(200);
                entity.Property(e => e.ShiftInChargePersonName).HasMaxLength(50);
                entity.Property(e => e.Status).HasMaxLength(10);
                entity.Property(e => e.Remarks);

                entity.HasIndex(e => e.EmployeeNumber).HasName("IDX_DAF_EMP_NUM");
                entity.HasIndex(e => e.CompanyCode).HasName("IDX_DAF_COM_COD");
                entity.HasIndex(e => e.AccidentDateTime).HasName("IDX_DAF_ACC_DAT");
            });

            // Other legacy entity configurations remain unchanged...
            modelBuilder.Entity<AccidentContractorList>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ContractorName).HasMaxLength(50);
                entity.Property(e => e.Status).HasDefaultValue('A');
            });

            modelBuilder.Entity<PersonalInjury>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PersonInjuredName).HasMaxLength(100);
                entity.Property(e => e.EmployeeStatus).HasDefaultValue('S');
            });

            // NatureOfInjury configuration
            modelBuilder.Entity<NatureOfInjury>(entity =>
            {
                entity.HasKey(e => e.NatureId);
                entity.Property(e => e.NatureName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            // DoctorAttendant configuration
            modelBuilder.Entity<DoctorAttendant>(entity =>
            {
                entity.HasKey(e => e.DoctorAttendantId);
                entity.Property(e => e.Code).HasMaxLength(20);
                entity.Property(e => e.Flag).HasDefaultValue('D');
                entity.Property(e => e.Name).HasMaxLength(30);
                entity.Property(e => e.Specialization).HasMaxLength(100);
                entity.Property(e => e.ContactNumber).HasMaxLength(20);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });
        }
    }

    /// <summary>
    /// Generic Repository Implementation
    /// </summary>
    public class GenericRepository<T> : IRepository<T, long> where T : class
    {
        private readonly AccidentManagementDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AccidentManagementDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(_dbSet.Where(predicate));
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
}
