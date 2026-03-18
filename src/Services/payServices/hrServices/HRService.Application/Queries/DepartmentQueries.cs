using MediatR;

namespace HRService.Application.Queries;

public class GetDepartmentByIdQuery : IRequest<DTOs.DepartmentDto>
{
    public Guid DepartmentId { get; set; }

    public GetDepartmentByIdQuery(Guid departmentId)
    {
        DepartmentId = departmentId;
    }
}

public class GetAllDepartmentsQuery : IRequest<List<DTOs.DepartmentDto>>
{
}

public class GetActiveDepartmentsQuery : IRequest<List<DTOs.DepartmentDto>>
{
}
