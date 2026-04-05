namespace ApprovalService.API.GraphQL;

using HotChocolate;
using MediatR;
using ApprovalService.Application.CQRS.Commands;

/// <summary>
/// HotChocolate GraphQL Mutation type for Approval Service
/// </summary>
public class ApprovalMutation
{
    /// <summary>Create a new approval master</summary>
    public async Task<CreateApprovalMasterCommandResult> CreateApproval(
        string code,
        string name,
        string module,
        int level,
        long userId,
        [Service] IMediator mediator)
        => await mediator.Send(new CreateApprovalMasterCommand
        {
            Code = code,
            Name = name,
            Module = module,
            Level = level,
            UserId = userId
        });

    /// <summary>Update an existing approval master</summary>
    public async Task<bool> UpdateApproval(
        long id,
        string name,
        int level,
        long userId,
        [Service] IMediator mediator)
        => await mediator.Send(new UpdateApprovalMasterCommand
        {
            Id = id,
            Name = name,
            Level = level,
            UserId = userId
        });

    /// <summary>Deactivate an approval master</summary>
    public async Task<bool> DeactivateApproval(
        long id,
        long userId,
        [Service] IMediator mediator)
        => await mediator.Send(new DeactivateApprovalMasterCommand
        {
            Id = id,
            UserId = userId
        });

    /// <summary>Activate an approval master</summary>
    public async Task<bool> ActivateApproval(
        long id,
        long userId,
        [Service] IMediator mediator)
        => await mediator.Send(new ActivateApprovalMasterCommand
        {
            Id = id,
            UserId = userId
        });

    /// <summary>Create a new approver employee assignment</summary>
    public async Task<CreateApproverEmployeeCommandResult> CreateApproverEmployee(
        long approvalMasterId,
        long employeeSysId,
        int approverLevel,
        DateTime effectiveFrom,
        DateTime? effectiveTo,
        long userId,
        [Service] IMediator mediator)
        => await mediator.Send(new CreateApproverEmployeeCommand
        {
            ApprovalMasterId = approvalMasterId,
            EmployeeSysId = employeeSysId,
            ApproverLevel = approverLevel,
            EffectiveFrom = effectiveFrom,
            EffectiveTo = effectiveTo,
            UserId = userId
        });

    /// <summary>Update an approver employee assignment</summary>
    public async Task<bool> UpdateApproverEmployee(
        long id,
        int approverLevel,
        DateTime? effectiveTo,
        long userId,
        [Service] IMediator mediator)
        => await mediator.Send(new UpdateApproverEmployeeCommand
        {
            Id = id,
            ApproverLevel = approverLevel,
            EffectiveTo = effectiveTo,
            UserId = userId
        });
}
