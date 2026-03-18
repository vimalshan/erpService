using MediatR;
using ProjectService.Application.DTOs;

namespace ProjectService.Application.Commands;

public record CreateProjectMasterCommand : IRequest<ProjectMasterDto>
{
    public string ProjectName { get; init; } = null!;
    public long ProjectCategoryId { get; init; }
    public DateTime ProjectEffDate { get; init; }
    public decimal ProjectTeamId { get; init; }
    public char ProjectListAll { get; init; }
    public long LastModifiedBy { get; init; }
}

public record UpdateProjectMasterCommand : IRequest<ProjectMasterDto>
{
    public long ProjectId { get; init; }
    public string ProjectName { get; init; } = null!;
    public long ProjectCategoryId { get; init; }
    public DateTime ProjectEffDate { get; init; }
    public decimal ProjectTeamId { get; init; }
    public char ProjectListAll { get; init; }
    public long LastModifiedBy { get; init; }
}

public record CreateProjectTypeCommand : IRequest<ProjectTypeMasterDto>
{
    public string ProjTypeName { get; init; } = null!;
    public string ProjTypeCode { get; init; } = null!;
    public decimal ProjTypeDepId { get; init; }
    public decimal ProjTypeCatId { get; init; }
}

public record CreateProjectLocationCommand(string LocName, long LastModifiedBy) : IRequest<ProjectLocationDto>;
public record CreateProjectProcessCommand(string ProcName, long LastModifiedBy) : IRequest<ProjectProcessDto>;
public record CreateProjectDepartmentCommand(string ProjDepName, string ProjDepCode, long LastModifiedBy) : IRequest<ProjectDepartmentDto>;
public record CreateProjectFunctionCommand(string ProjFuncName, long LastModifiedBy) : IRequest<ProjectFunctionDto>;
public record CreateProjectCategoryCommand(string CategoryName, long CategoryTeamId, long LastModifiedBy) : IRequest<ProjectCategoryDto>;
