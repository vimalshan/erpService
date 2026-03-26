using MediatR;
using ReferenceDataService.Application.DTOs;

namespace ReferenceDataService.Application.Commands.CreatePathToSqlServer;

public record CreatePathToSqlServerCommand(
    string? CompanyCode,
    string? ServerName,
    string? DatabaseName,
    string? UserId,
    string? DbPassword) : IRequest<PathToSqlServerDto>;
