using EmployeeTransactionsService.Domain.Entities;
using EmployeeTransactionsService.Domain.Interfaces;
using EmployeeTransactionsService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EmployeeTransactionsService.Infrastructure.Repositories;

internal static class RepositoryIdExtensions
{
    public static async Task<decimal> GetNextDecimalIdAsync<T>(this IQueryable<T> query, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken)
        where T : class
    {
        var values = await query.Select(selector).ToListAsync(cancellationToken);
        return (values.Count == 0 ? 0 : values.Max()) + 1;
    }
}

public sealed class EmployeeRepository(EmployeeTransactionsDbContext dbContext) : IEmployeeRepository
{
    public async Task<decimal> GetNextIdAsync(CancellationToken cancellationToken) =>
        await dbContext.Employees.AsNoTracking().GetNextDecimalIdAsync(static x => x.EmpSysId, cancellationToken);

    public Task AddAsync(EmployeeMain employee, CancellationToken cancellationToken)
        => dbContext.Employees.AddAsync(employee, cancellationToken).AsTask();

    public Task<EmployeeMain?> GetByIdAsync(decimal employeeId, CancellationToken cancellationToken)
        => dbContext.Employees.FirstOrDefaultAsync(x => x.EmpSysId == employeeId, cancellationToken);

    public async Task<IReadOnlyList<EmployeeMain>> ListAsync(int take, CancellationToken cancellationToken)
        => await dbContext.Employees.AsNoTracking().OrderByDescending(x => x.EmpCreatedOn).Take(take).ToListAsync(cancellationToken);
}

public sealed class EmployeeGradeRepository(EmployeeTransactionsDbContext dbContext) : IEmployeeGradeRepository
{
    public async Task<decimal> GetNextTransactionIdAsync(CancellationToken cancellationToken) =>
        await dbContext.EmployeeGrades.AsNoTracking().GetNextDecimalIdAsync(static x => x.GradeTranId, cancellationToken);

    public Task<EmployeeGrade?> GetCurrentByEmployeeAsync(decimal employeeId, CancellationToken cancellationToken)
        => dbContext.EmployeeGrades.FirstOrDefaultAsync(x => x.GradeEmpSysId == employeeId && x.GradeLivFlag == "Y", cancellationToken);

    public Task AddAsync(EmployeeGrade grade, CancellationToken cancellationToken)
    {
        dbContext.Entry(grade).State = dbContext.EmployeeGrades.Any(x => x.GradeEmpSysId == grade.GradeEmpSysId)
            ? EntityState.Modified
            : EntityState.Added;
        return Task.CompletedTask;
    }
}

public sealed class EmployeeGradeChangeRepository(EmployeeTransactionsDbContext dbContext) : IEmployeeGradeChangeRepository
{
    public async Task<decimal> GetNextIdAsync(CancellationToken cancellationToken) =>
        await dbContext.EmployeeGradeChanges.AsNoTracking().GetNextDecimalIdAsync(static x => x.EmpGradeChangeId, cancellationToken);

    public Task AddAsync(EmployeeGradeChange change, CancellationToken cancellationToken)
        => dbContext.EmployeeGradeChanges.AddAsync(change, cancellationToken).AsTask();

    public async Task<IReadOnlyList<EmployeeGradeChange>> GetByEmployeeAsync(decimal employeeId, CancellationToken cancellationToken)
        => await dbContext.EmployeeGradeChanges.AsNoTracking()
            .Where(x => x.EmpEmpSysId == employeeId)
            .OrderByDescending(x => x.EmpCreatedOn)
            .ToListAsync(cancellationToken);
}

public sealed class EmployeeProbationRepository(EmployeeTransactionsDbContext dbContext) : IEmployeeProbationRepository
{
    public async Task<decimal> GetNextIdAsync(CancellationToken cancellationToken) =>
        await dbContext.EmployeeProbations.AsNoTracking().GetNextDecimalIdAsync(static x => x.ProbId, cancellationToken);

    public Task AddAsync(EmployeeProbation probation, CancellationToken cancellationToken)
        => dbContext.EmployeeProbations.AddAsync(probation, cancellationToken).AsTask();

    public Task<EmployeeProbation?> GetByIdAsync(decimal probationId, CancellationToken cancellationToken)
        => dbContext.EmployeeProbations.FirstOrDefaultAsync(x => x.ProbId == probationId, cancellationToken);

    public Task<EmployeeProbation?> GetByEmployeeAsync(decimal employeeId, CancellationToken cancellationToken)
        => dbContext.EmployeeProbations.AsNoTracking().FirstOrDefaultAsync(x => x.ProbEmpSysId == employeeId, cancellationToken);
}

public sealed class AlertGroupRepository(EmployeeTransactionsDbContext dbContext) : IAlertGroupRepository
{
    public async Task<decimal> GetNextIdAsync(CancellationToken cancellationToken) =>
        await dbContext.AlertGroups.AsNoTracking().GetNextDecimalIdAsync(static x => x.AlgrpId, cancellationToken);

    public async Task<decimal> GetNextMapIdAsync(CancellationToken cancellationToken) =>
        await dbContext.AlertGroupEmployeeMaps.AsNoTracking().GetNextDecimalIdAsync(static x => x.AlmapId, cancellationToken);

    public Task AddAsync(AlertGroup alertGroup, CancellationToken cancellationToken)
        => dbContext.AlertGroups.AddAsync(alertGroup, cancellationToken).AsTask();

    public Task<AlertGroup?> GetByIdAsync(decimal alertGroupId, CancellationToken cancellationToken)
        => dbContext.AlertGroups.Include("_members").FirstOrDefaultAsync(x => x.AlgrpId == alertGroupId, cancellationToken);

    public async Task<IReadOnlyList<AlertGroup>> ListAsync(CancellationToken cancellationToken)
        => await dbContext.AlertGroups.Include("_members").AsNoTracking().OrderBy(x => x.AlgrpName).ToListAsync(cancellationToken);
}

public sealed class StationeryItemImageRepository(EmployeeTransactionsDbContext dbContext) : IStationeryItemImageRepository
{
    public Task AddAsync(StationeryItemImage image, CancellationToken cancellationToken)
        => dbContext.StationeryItemImages.AddAsync(image, cancellationToken).AsTask();

    public async Task<IReadOnlyList<StationeryItemImage>> ListAsync(CancellationToken cancellationToken)
        => await dbContext.StationeryItemImages.AsNoTracking().OrderByDescending(x => x.UploadedOnUtc).ToListAsync(cancellationToken);
}