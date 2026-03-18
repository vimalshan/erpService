namespace ApprovalService.Domain.Interfaces;

using ApprovalService.Domain.Entities;

/// <summary>
/// Repository interface for approval master domain
/// </summary>
public interface IApprovalMasterRepository
{
    Task<ApprovalMaster?> GetByIdAsync(long id);
    Task<ApprovalMaster?> GetByCodeAsync(string code);
    Task<IEnumerable<ApprovalMaster>> GetByModuleAsync(string module);
    Task<IEnumerable<ApprovalMaster>> GetAllAsync();
    Task AddAsync(ApprovalMaster approval);
    Task UpdateAsync(ApprovalMaster approval);
    Task DeleteAsync(long id);
}

/// <summary>
/// Repository interface for approver employee domain
/// </summary>
public interface IApproverEmployeeRepository
{
    Task<ApproverEmployee?> GetByIdAsync(long id);
    Task<IEnumerable<ApproverEmployee>> GetByApprovalMasterAsync(long approvalMasterId);
    Task<IEnumerable<ApproverEmployee>> GetByEmployeeAsync(long employeeSysId);
    Task AddAsync(ApproverEmployee approver);
    Task UpdateAsync(ApproverEmployee approver);
    Task DeleteAsync(long id);
}

/// <summary>
/// Interface for handling domain events
/// </summary>
public interface IDomainEventPublisher
{
    Task PublishAsync<TEvent>(TEvent domainEvent) where TEvent : class;
}

/// <summary>
/// Interface for unit of work pattern
/// </summary>
public interface IUnitOfWork
{
    IApprovalMasterRepository ApprovalMasters { get; }
    IApproverEmployeeRepository ApproverEmployees { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
