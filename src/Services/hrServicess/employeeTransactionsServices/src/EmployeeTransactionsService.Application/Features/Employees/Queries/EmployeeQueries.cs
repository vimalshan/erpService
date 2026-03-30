using EmployeeTransactionsService.Application.Contracts;
using EmployeeTransactionsService.Application.DTOs;
using EmployeeTransactionsService.Domain.Interfaces;
using MediatR;

namespace EmployeeTransactionsService.Application.Features.Employees.Queries;

public sealed record GetEmployeeByIdQuery(decimal EmployeeId) : IRequest<EmployeeTransactionDto?>;

public sealed class GetEmployeeByIdQueryHandler(
    IEmployeeRepository employeeRepository,
    IEmployeeGradeRepository gradeRepository,
    IEmployeeProbationRepository probationRepository) : IRequestHandler<GetEmployeeByIdQuery, EmployeeTransactionDto?>
{
    public async Task<EmployeeTransactionDto?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);
        if (employee is null)
            return null;

        var currentGrade = await gradeRepository.GetCurrentByEmployeeAsync(request.EmployeeId, cancellationToken);
        var probation = await probationRepository.GetByEmployeeAsync(request.EmployeeId, cancellationToken);
        return employee.ToDto(currentGrade, probation);
    }
}

public sealed record ListEmployeesQuery(int Take = 50) : IRequest<IReadOnlyList<EmployeeTransactionDto>>;

public sealed class ListEmployeesQueryHandler(
    IEmployeeRepository employeeRepository,
    IEmployeeGradeRepository gradeRepository,
    IEmployeeProbationRepository probationRepository) : IRequestHandler<ListEmployeesQuery, IReadOnlyList<EmployeeTransactionDto>>
{
    public async Task<IReadOnlyList<EmployeeTransactionDto>> Handle(ListEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await employeeRepository.ListAsync(request.Take, cancellationToken);
        var results = new List<EmployeeTransactionDto>(employees.Count);
        foreach (var employee in employees)
        {
            var currentGrade = await gradeRepository.GetCurrentByEmployeeAsync(employee.EmpSysId, cancellationToken);
            var probation = await probationRepository.GetByEmployeeAsync(employee.EmpSysId, cancellationToken);
            results.Add(employee.ToDto(currentGrade, probation));
        }

        return results;
    }
}

public sealed record GetEmployeeTimelineQuery(decimal EmployeeId) : IRequest<IReadOnlyList<TransactionTimelineItemDto>>;

public sealed class GetEmployeeTimelineQueryHandler(ITransactionReadService transactionReadService)
    : IRequestHandler<GetEmployeeTimelineQuery, IReadOnlyList<TransactionTimelineItemDto>>
{
    public async Task<IReadOnlyList<TransactionTimelineItemDto>> Handle(GetEmployeeTimelineQuery request, CancellationToken cancellationToken)
        => await transactionReadService.GetEmployeeTimelineAsync(request.EmployeeId, cancellationToken);
}