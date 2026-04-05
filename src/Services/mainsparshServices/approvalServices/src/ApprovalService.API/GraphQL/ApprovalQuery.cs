namespace ApprovalService.API.GraphQL;

using HotChocolate;
using MediatR;
using ApprovalService.Application.CQRS.Queries;
using ApprovalService.Application.DTOs;

/// <summary>
/// HotChocolate GraphQL Query type for Approval Service
/// </summary>
public class ApprovalQuery
{
    /// <summary>Get all approval masters</summary>
    public async Task<List<ApprovalMasterDto>> GetApprovals([Service] IMediator mediator)
        => await mediator.Send(new GetAllApprovalsQuery());

    /// <summary>Get approval master by ID</summary>
    public async Task<ApprovalMasterDto?> GetApprovalById(long id, [Service] IMediator mediator)
        => await mediator.Send(new GetApprovalMasterByIdQuery { Id = id });

    /// <summary>Get approval master by code</summary>
    public async Task<ApprovalMasterDto?> GetApprovalByCode(string code, [Service] IMediator mediator)
        => await mediator.Send(new GetApprovalMasterByCodeQuery { Code = code });

    /// <summary>Get approvals by module</summary>
    public async Task<List<ApprovalMasterDto>> GetApprovalsByModule(string module, [Service] IMediator mediator)
        => await mediator.Send(new GetApprovalsByModuleQuery { Module = module });

    /// <summary>Get paginated approvals</summary>
    public async Task<PaginatedDto<ApprovalMasterDto>> GetApprovalsPaginated(
        int pageNumber, int pageSize, string? module,
        [Service] IMediator mediator)
        => await mediator.Send(new GetPaginatedApprovalsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            Module = module
        });

    /// <summary>Get approvers by approval master ID</summary>
    public async Task<List<ApproverEmployeeDto>> GetApproversByApprovalMaster(
        long approvalMasterId, [Service] IMediator mediator)
        => await mediator.Send(new GetApproversByApprovalMasterQuery { ApprovalMasterId = approvalMasterId });

    /// <summary>Get active approvers for a module</summary>
    public async Task<List<ApproverEmployeeDto>> GetActiveApproversByModule(
        string module, [Service] IMediator mediator)
        => await mediator.Send(new GetActiveApproversByModuleQuery { Module = module });

    /// <summary>Get approvals by employee ID</summary>
    public async Task<List<ApproverEmployeeDto>> GetApproversByEmployee(
        long employeeId, [Service] IMediator mediator)
        => await mediator.Send(new GetApproversByEmployeeQuery { EmployeeSysId = employeeId });
}
