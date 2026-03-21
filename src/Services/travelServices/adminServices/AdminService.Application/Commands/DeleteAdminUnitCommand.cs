using MediatR;

namespace AdminService.Application.Commands;

/// <summary>
/// Command to delete an admin unit
/// </summary>
public record DeleteAdminUnitCommand(long Id) : IRequest<bool>;
