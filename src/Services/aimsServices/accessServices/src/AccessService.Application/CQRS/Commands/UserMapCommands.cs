namespace AccessService.Application.CQRS.Commands;

using MediatR;

/// <summary>
/// Commands for UserMap aggregate
/// </summary>

public class CreateUserMapCommand : IRequest<Guid>
{
    public long EmployeeSystemId { get; set; }
}

public class ActivateUserMapCommand : IRequest
{
    public long EmployeeSystemId { get; set; }
    
    public DateTime EffectiveDate { get; set; }
}

public class DeactivateUserMapCommand : IRequest
{
    public long EmployeeSystemId { get; set; }
    
    public DateTime ClosureDate { get; set; }
}


