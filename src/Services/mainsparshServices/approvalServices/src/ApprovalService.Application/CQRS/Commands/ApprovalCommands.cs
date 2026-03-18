namespace ApprovalService.Application.CQRS.Commands;

using MediatR;

/// <summary>
/// Command to create a new approval master
/// </summary>
public class CreateApprovalMasterCommand : IRequest<CreateApprovalMasterCommandResult>
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Module { get; set; }
    public int Level { get; set; } = 1;
    public long UserId { get; set; }
}

public record CreateApprovalMasterCommandResult
{
    public long Id { get; set; }
    public string Code { get; set; } = "";
}

/// <summary>
/// Command to update an approval master
/// </summary>
public class UpdateApprovalMasterCommand : IRequest<bool>
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public int Level { get; set; }
    public long UserId { get; set; }
}

/// <summary>
/// Command to deactivate an approval master
/// </summary>
public class DeactivateApprovalMasterCommand : IRequest<bool>
{
    public long Id { get; set; }
    public long UserId { get; set; }
}

/// <summary>
/// Command to activate an approval master
/// </summary>
public class ActivateApprovalMasterCommand : IRequest<bool>
{
    public long Id { get; set; }
    public long UserId { get; set; }
}

/// <summary>
/// Command to create an approver employee
/// </summary>
public class CreateApproverEmployeeCommand : IRequest<CreateApproverEmployeeCommandResult>
{
    public long ApprovalMasterId { get; set; }
    public long EmployeeSysId { get; set; }
    public int ApproverLevel { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public long UserId { get; set; }
}

public record CreateApproverEmployeeCommandResult
{
    public long Id { get; set; }
}

/// <summary>
/// Command to update an approver employee
/// </summary>
public class UpdateApproverEmployeeCommand : IRequest<bool>
{
    public long Id { get; set; }
    public int ApproverLevel { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public long UserId { get; set; }
}

/// <summary>
/// Command to deactivate an approver employee
/// </summary>
public class DeactivateApproverEmployeeCommand : IRequest<bool>
{
    public long Id { get; set; }
    public long UserId { get; set; }
}

/// <summary>
/// Command to activate an approver employee
/// </summary>
public class ActivateApproverEmployeeCommand : IRequest<bool>
{
    public long Id { get; set; }
    public long UserId { get; set; }
}
