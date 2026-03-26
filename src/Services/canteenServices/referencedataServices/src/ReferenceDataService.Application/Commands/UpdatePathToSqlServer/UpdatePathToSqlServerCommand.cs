using MediatR;
using ReferenceDataService.Application.DTOs;

namespace ReferenceDataService.Application.Commands.UpdatePathToSqlServer;

public record UpdatePathToSqlServerCommand(
    int Id,
    string? ServerName,
    string? DatabaseName,
    string? UserId,
    string? DbPassword) : IRequest<PathToSqlServerDto>;
