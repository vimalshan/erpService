using MediatR;

namespace HRService.Application.Queries;

public class GetEmployeeByIdQuery : IRequest<DTOs.EmployeeDto>
{
    public Guid EmployeeId { get; set; }

    public GetEmployeeByIdQuery(Guid employeeId)
    {
        EmployeeId = employeeId;
    }
}

public class GetAllEmployeesQuery : IRequest<List<DTOs.EmployeeDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetEmployeesByDepartmentQuery : IRequest<List<DTOs.EmployeeDto>>
{
    public Guid DepartmentId { get; set; }
}

public class GetEmployeesByStatusQuery : IRequest<List<DTOs.EmployeeDto>>
{
    public string Status { get; set; }
}
