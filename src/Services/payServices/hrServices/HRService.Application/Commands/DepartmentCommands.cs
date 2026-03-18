using MediatR;

namespace HRService.Application.Commands;

public class CreateDepartmentCommand : IRequest<Guid>
{
    public string DepartmentCode { get; set; }
    public string DepartmentName { get; set; }
    public string? Description { get; set; }
}

public class UpdateDepartmentManagerCommand : IRequest<bool>
{
    public Guid DepartmentId { get; set; }
    public Guid ManagerId { get; set; }
}

public class DeactivateDepartmentCommand : IRequest<bool>
{
    public Guid DepartmentId { get; set; }
}
