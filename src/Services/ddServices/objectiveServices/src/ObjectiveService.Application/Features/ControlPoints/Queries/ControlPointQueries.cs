using ObjectiveService.Application.Common;
using ObjectiveService.Application.DTOs;

namespace ObjectiveService.Application.Features.ControlPoints.Queries;

public class GetControlPointByIdQuery : QueryBase<CommandResult<ControlPointDto>>
{
    public decimal Id { get; set; }

    public GetControlPointByIdQuery(decimal id)
    {
        Id = id;
    }
}

public class GetControlPointsByEmployeeQuery : QueryBase<CommandResult<List<ControlPointDto>>>
{
    public decimal EmployeeSysId { get; set; }
    public decimal DDYearId { get; set; }

    public GetControlPointsByEmployeeQuery(decimal employeeSysId, decimal ddYearId)
    {
        EmployeeSysId = employeeSysId;
        DDYearId = ddYearId;
    }
}

public class GetAllControlPointsQuery : QueryBase<CommandResult<List<ControlPointDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
