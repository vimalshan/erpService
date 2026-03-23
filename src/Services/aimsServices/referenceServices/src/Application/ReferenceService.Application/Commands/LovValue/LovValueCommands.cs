using MediatR;

namespace ReferenceService.Application.Commands.LovValue;

/// <summary>
/// Command to create a new LOV Value.
/// </summary>
public record CreateLovValueCommand(
    int TypeId,
    string Code,
    string Description,
    string? LongDescription,
    int Sequence,
    long ModifiedBy
) : IRequest<CreateLovValueResponse>;

public record CreateLovValueResponse(int Id, string Code, bool Success, string? Message);

/// <summary>
/// Command to update a LOV Value.
/// </summary>
public record UpdateLovValueCommand(
    int Id,
    int TypeId,
    string Description,
    string? LongDescription,
    int Sequence,
    long ModifiedBy
) : IRequest<UpdateLovValueResponse>;

public record UpdateLovValueResponse(bool Success, string? Message);

/// <summary>
/// Command to deactivate a LOV Value.
/// </summary>
public record DeactivateLovValueCommand(
    int Id,
    long ModifiedBy
) : IRequest<DeactivateLovValueResponse>;

public record DeactivateLovValueResponse(bool Success, string? Message);
