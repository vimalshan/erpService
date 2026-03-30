using EmployeeTransactionsService.Domain.Entities;

namespace EmployeeTransactionsService.Domain.Interfaces;

public interface IEmployeeRepository
{
    Task<decimal> GetNextIdAsync(CancellationToken cancellationToken);
    Task AddAsync(EmployeeMain employee, CancellationToken cancellationToken);
    Task<EmployeeMain?> GetByIdAsync(decimal employeeId, CancellationToken cancellationToken);
    Task<IReadOnlyList<EmployeeMain>> ListAsync(int take, CancellationToken cancellationToken);
}

public interface IEmployeeGradeRepository
{
    Task<decimal> GetNextTransactionIdAsync(CancellationToken cancellationToken);
    Task<EmployeeGrade?> GetCurrentByEmployeeAsync(decimal employeeId, CancellationToken cancellationToken);
    Task AddAsync(EmployeeGrade grade, CancellationToken cancellationToken);
}

public interface IEmployeeGradeChangeRepository
{
    Task<decimal> GetNextIdAsync(CancellationToken cancellationToken);
    Task AddAsync(EmployeeGradeChange change, CancellationToken cancellationToken);
    Task<IReadOnlyList<EmployeeGradeChange>> GetByEmployeeAsync(decimal employeeId, CancellationToken cancellationToken);
}

public interface IEmployeeProbationRepository
{
    Task<decimal> GetNextIdAsync(CancellationToken cancellationToken);
    Task AddAsync(EmployeeProbation probation, CancellationToken cancellationToken);
    Task<EmployeeProbation?> GetByIdAsync(decimal probationId, CancellationToken cancellationToken);
    Task<EmployeeProbation?> GetByEmployeeAsync(decimal employeeId, CancellationToken cancellationToken);
}

public interface IAlertGroupRepository
{
    Task<decimal> GetNextIdAsync(CancellationToken cancellationToken);
    Task<decimal> GetNextMapIdAsync(CancellationToken cancellationToken);
    Task AddAsync(AlertGroup alertGroup, CancellationToken cancellationToken);
    Task<AlertGroup?> GetByIdAsync(decimal alertGroupId, CancellationToken cancellationToken);
    Task<IReadOnlyList<AlertGroup>> ListAsync(CancellationToken cancellationToken);
}

public interface IStationeryItemImageRepository
{
    Task AddAsync(StationeryItemImage image, CancellationToken cancellationToken);
    Task<IReadOnlyList<StationeryItemImage>> ListAsync(CancellationToken cancellationToken);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}