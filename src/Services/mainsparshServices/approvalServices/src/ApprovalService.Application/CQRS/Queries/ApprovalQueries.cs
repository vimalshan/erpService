namespace ApprovalService.Application.CQRS.Queries;

using MediatR;
using ApprovalService.Application.DTOs;

/// <summary>
/// Query to get approval master by ID
/// </summary>
public class GetApprovalMasterByIdQuery : IRequest<ApprovalMasterDto?>
{
    public long Id { get; set; }
}

/// <summary>
/// Query to get approval master by code
/// </summary>
public class GetApprovalMasterByCodeQuery : IRequest<ApprovalMasterDto?>
{
    public required string Code { get; set; }
}

/// <summary>
/// Query to get all approvals by module
/// </summary>
public class GetApprovalsByModuleQuery : IRequest<List<ApprovalMasterDto>>
{
    public required string Module { get; set; }
}

/// <summary>
/// Query to get all approval masters
/// </summary>
public class GetAllApprovalsQuery : IRequest<List<ApprovalMasterDto>>
{
}

/// <summary>
/// Query to get paginated approvals
/// </summary>
public class GetPaginatedApprovalsQuery : IRequest<PaginatedDto<ApprovalMasterDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Module { get; set; }
}

/// <summary>
/// Query to get approver employee by ID
/// </summary>
public class GetApproverEmployeeByIdQuery : IRequest<ApproverEmployeeDto?>
{
    public long Id { get; set; }
}

/// <summary>
/// Query to get approvers by approval master ID
/// </summary>
public class GetApproversByApprovalMasterQuery : IRequest<List<ApproverEmployeeDto>>
{
    public long ApprovalMasterId { get; set; }
}

/// <summary>
/// Query to get current active approvers for a module
/// </summary>
public class GetActiveApproversByModuleQuery : IRequest<List<ApproverEmployeeDto>>
{
    public required string Module { get; set; }
}

/// <summary>
/// Query to get approvers by employee ID
/// </summary>
public class GetApproversByEmployeeQuery : IRequest<List<ApproverEmployeeDto>>
{
    public long EmployeeSysId { get; set; }
}
