using MediatR;
using ProjectService.Application.DTOs;

namespace ProjectService.Application.Commands;

public record CreateProjectCommand : IRequest<ProjectMainDto>
{
    public string ProjName { get; init; } = null!;
    public decimal ProjCharterNo { get; init; }
    public long ProjLeaderId { get; init; }
    public long ProjTypeId { get; init; }
    public long ProjLocId { get; init; }
    public long ProjProcessId { get; init; }
    public DateTime ProjStartDate { get; init; }
    public DateTime ProjEndDate { get; init; }
    public DateTime ProjEstEndDate { get; init; }
    public long? ProjObjId { get; init; }
    public string? ProjObjDesc { get; init; }
    public string? ProjTargetProd { get; init; }
    public string? ProjTargetProdRem { get; init; }
    public string? ProjNotes { get; init; }
}

public record UpdateProjectCommand : IRequest<ProjectMainDto>
{
    public long ProjId { get; init; }
    public string ProjName { get; init; } = null!;
    public decimal ProjCharterNo { get; init; }
    public long ProjLeaderId { get; init; }
    public long ProjTypeId { get; init; }
    public long ProjLocId { get; init; }
    public long ProjProcessId { get; init; }
    public DateTime ProjStartDate { get; init; }
    public DateTime ProjEndDate { get; init; }
    public DateTime ProjEstEndDate { get; init; }
    public long? ProjObjId { get; init; }
    public string? ProjObjDesc { get; init; }
    public string? ProjTargetProd { get; init; }
    public string? ProjTargetProdRem { get; init; }
    public string? ProjNotes { get; init; }
}

public record DeleteProjectCommand(long ProjId) : IRequest<bool>;

public record ChangeProjectStatusCommand(long ProjId, char NewStatus) : IRequest<ProjectMainDto>;

public record HoldProjectCommand(long ProjId, string Reason, long UpdatedBy) : IRequest<ProjectHoldDto>;

public record UnholdProjectCommand(long ProjId, string Reason, long UpdatedBy) : IRequest<ProjectHoldDto>;

public record CloseProjectCommand(long ProjId) : IRequest<ProjectMainDto>;

public record AddProjectMemberCommand(long ProjId, long FuncId, long EmpSysId) : IRequest<ProjectMemberDto>;

public record RemoveProjectMemberCommand(long MemberId) : IRequest<bool>;

public record RequestApprovalCommand(long ProjId, char ApprType, long ApproverEmpSysId) : IRequest<ProjectApprovalDetailDto>;

public record ProcessApprovalCommand(long ApprovalId, char Status, string Remarks) : IRequest<ProjectApprovalDetailDto>;

public record AddProjectStatusCommand(long ProjId, string Remarks, string? StatusFile) : IRequest<ProjectStatusHistoryDto>;
