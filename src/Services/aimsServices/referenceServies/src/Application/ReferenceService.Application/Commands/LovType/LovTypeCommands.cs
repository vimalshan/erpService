using MediatR;

namespace ReferenceService.Application.Commands.LovType;

/// <summary>
/// Command to create a new LOV Type.
/// </summary>
public record CreateLovTypeCommand(
    string TypeName,
    string? Description,
    int Sequence,
    long ModifiedBy
) : IRequest<CreateLovTypeResponse>;

public record CreateLovTypeResponse(int Id, string TypeName, bool Success, string? Message);

/// <summary>
/// Handler for CreateLovTypeCommand.
/// </summary>
public class CreateLovTypeCommandHandler : IRequestHandler<CreateLovTypeCommand, CreateLovTypeResponse>
{
    public Task<CreateLovTypeResponse> Handle(CreateLovTypeCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("To be implemented with infrastructure");
    }
}

/// <summary>
/// Command to update a LOV Type.
/// </summary>
public record UpdateLovTypeCommand(
    int Id,
    string TypeName,
    string? Description,
    int Sequence,
    long ModifiedBy
) : IRequest<UpdateLovTypeResponse>;

public record UpdateLovTypeResponse(bool Success, string? Message);

/// <summary>
/// Command to deactivate a LOV Type.
/// </summary>
public record DeactivateLovTypeCommand(
    int Id,
    long ModifiedBy
) : IRequest<DeactivateLovTypeResponse>;

public record DeactivateLovTypeResponse(bool Success, string? Message);
