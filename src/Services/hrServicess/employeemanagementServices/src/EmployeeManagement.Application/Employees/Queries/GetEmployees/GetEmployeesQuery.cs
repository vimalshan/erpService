using EmployeeManagement.Application.Common.Models;
using EmployeeManagement.Application.Employees.DTOs;
using EmployeeManagement.Domain.Interfaces;
using MediatR;

namespace EmployeeManagement.Application.Employees.Queries.GetEmployees;

public sealed record GetEmployeesQuery(int Page = 1, int PageSize = 20) : IRequest<PaginatedResult<EmployeeSummaryDto>>;

public sealed class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, PaginatedResult<EmployeeSummaryDto>>
{
    private readonly IEmployeeRepository _repository;

    public GetEmployeesQueryHandler(IEmployeeRepository repository) => _repository = repository;

    public async Task<PaginatedResult<EmployeeSummaryDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _repository.GetAllAsync(request.Page, request.PageSize, cancellationToken);
        var total = await _repository.CountAsync(cancellationToken);

        var dtos = employees.Select(e => new EmployeeSummaryDto(e.Id, e.EmployeeNo, e.Designation, e.Unit, e.IsActive)).ToList();
        return new PaginatedResult<EmployeeSummaryDto>(dtos, total, request.Page, request.PageSize);
    }
}
