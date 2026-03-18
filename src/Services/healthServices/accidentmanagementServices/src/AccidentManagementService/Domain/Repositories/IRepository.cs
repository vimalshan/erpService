using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccidentManagementService.Domain.Entities;

namespace AccidentManagementService.Domain.Repositories
{
    /// <summary>
    /// Repository interface for AccidentReport aggregate root
    /// </summary>
    public interface IAccidentReportRepository
    {
        /// <summary>
        /// Add a new accident report to the repository
        /// </summary>
        Task<AccidentReport> AddAsync(AccidentReport accidentReport);

        /// <summary>
        /// Update an existing accident report
        /// </summary>
        Task<AccidentReport> UpdateAsync(AccidentReport accidentReport);

        /// <summary>
        /// Get accident report by ID
        /// </summary>
        Task<AccidentReport?> GetByIdAsync(long id);

        /// <summary>
        /// Get accident report by GUID
        /// </summary>
        Task<AccidentReport?> GetByGuidAsync(Guid guid);

        /// <summary>
        /// Get accident report by accident number
        /// </summary>
        Task<AccidentReport?> GetByAccidentNumberAsync(long accidentNumber);

        /// <summary>
        /// Get all accident reports for a company
        /// </summary>
        Task<IEnumerable<AccidentReport>> GetByCompanyCodeAsync(string companyCode);

        /// <summary>
        /// Get accident reports by status
        /// </summary>
        Task<IEnumerable<AccidentReport>> GetByStatusAsync(long statusId);

        /// <summary>
        /// Get accident reports by severity
        /// </summary>
        Task<IEnumerable<AccidentReport>> GetBySeverityAsync(long severityId);

        /// <summary>
        /// Get accident reports by date range
        /// </summary>
        Task<IEnumerable<AccidentReport>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, string? companyCode = null);

        /// <summary>
        /// Get accident reports by employee number
        /// </summary>
        Task<IEnumerable<AccidentReport>> GetByEmployeeNumberAsync(string employeeNumber);

        /// <summary>
        /// Delete an accident report (soft delete)
        /// </summary>
        Task<bool> DeleteAsync(long id);

        /// <summary>
        /// Restore a deleted accident report
        /// </summary>
        Task<bool> RestoreAsync(long id);
    }

    /// <summary>
    /// Repository interface for InjuryCategory
    /// </summary>
    public interface IInjuryCategoryRepository
    {
        Task<InjuryCategory> AddAsync(InjuryCategory category);
        Task<InjuryCategory> UpdateAsync(InjuryCategory category);
        Task<InjuryCategory?> GetByIdAsync(long id);
        Task<InjuryCategory?> GetByGuidAsync(Guid guid);
        Task<IEnumerable<InjuryCategory>> GetAllAsync();
        Task<bool> DeleteAsync(long id);
    }

    /// <summary>
    /// Repository interface for InjuryNature
    /// </summary>
    public interface IInjuryNatureRepository
    {
        Task<InjuryNature> AddAsync(InjuryNature nature);
        Task<InjuryNature> UpdateAsync(InjuryNature nature);
        Task<InjuryNature?> GetByIdAsync(long id);
        Task<InjuryNature?> GetByGuidAsync(Guid guid);
        Task<IEnumerable<InjuryNature>> GetAllAsync();
        Task<bool> DeleteAsync(long id);
    }

    /// <summary>
    /// Repository interface for AccidentSeverity
    /// </summary>
    public interface IAccidentSeverityRepository
    {
        Task<AccidentSeverity> AddAsync(AccidentSeverity severity);
        Task<AccidentSeverity> UpdateAsync(AccidentSeverity severity);
        Task<AccidentSeverity?> GetByIdAsync(long id);
        Task<AccidentSeverity?> GetByGuidAsync(Guid guid);
        Task<AccidentSeverity?> GetByCodeAsync(string code);
        Task<IEnumerable<AccidentSeverity>> GetAllAsync();
        Task<bool> DeleteAsync(long id);
    }

    /// <summary>
    /// Repository interface for AccidentStatus
    /// </summary>
    public interface IAccidentStatusRepository
    {
        Task<AccidentStatus> AddAsync(AccidentStatus status);
        Task<AccidentStatus> UpdateAsync(AccidentStatus status);
        Task<AccidentStatus?> GetByIdAsync(long id);
        Task<AccidentStatus?> GetByGuidAsync(Guid guid);
        Task<AccidentStatus?> GetByCodeAsync(string code);
        Task<IEnumerable<AccidentStatus>> GetAllAsync();
        Task<bool> DeleteAsync(long id);
    }

    /// <summary>
    /// Repository interface for Contractor
    /// </summary>
    public interface IContractorRepository
    {
        Task<Contractor> AddAsync(Contractor contractor);
        Task<Contractor> UpdateAsync(Contractor contractor);
        Task<Contractor?> GetByIdAsync(long id);
        Task<Contractor?> GetByGuidAsync(Guid guid);
        Task<Contractor?> GetByContractorIdAsync(long contractorId);
        Task<IEnumerable<Contractor>> GetAllAsync();
        Task<IEnumerable<Contractor>> GetActiveAsync();
        Task<bool> DeleteAsync(long id);
    }

    /// <summary>
    /// Repository interface for InjuredPerson
    /// </summary>
    public interface IInjuredPersonRepository
    {
        Task<InjuredPerson> AddAsync(InjuredPerson person);
        Task<InjuredPerson> UpdateAsync(InjuredPerson person);
        Task<InjuredPerson?> GetByIdAsync(long id);
        Task<InjuredPerson?> GetByGuidAsync(Guid guid);
        Task<InjuredPerson?> GetBySerialNumberAsync(long serialNumber);
        Task<IEnumerable<InjuredPerson>> GetAllAsync();
        Task<bool> DeleteAsync(long id);
    }

    /// <summary>
    /// Unit of Work pattern for managing repositories
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IAccidentReportRepository AccidentReports { get; }
        IInjuryCategoryRepository InjuryCategories { get; }
        IInjuryNatureRepository InjuryNatures { get; }
        IAccidentSeverityRepository AccidentSeverities { get; }
        IAccidentStatusRepository AccidentStatuses { get; }
        IContractorRepository Contractors { get; }
        IInjuredPersonRepository InjuredPersons { get; }

        /// <summary>
        /// SaveAsync all changes to the database
        /// </summary>
        Task<int> SaveAsync();

        /// <summary>
        /// Begin a transaction
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commit the current transaction
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Rollback the current transaction
        /// </summary>
        Task RollbackAsync();
    }
}
